using Microsoft.EntityFrameworkCore;
using ExpenseSystem.Data;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;
using ExpenseSystem.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ExpenseDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login/Index";
    options.AccessDeniedPath = "/Expenses/Index";
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<ExpenseDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}


using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var expenseDbContext = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();

    if (expenseDbContext.Projects.Count() == 0)
    {
        expenseDbContext.Projects.AddRange(new Project { ProjectName = "一般支出", IsActive = true });
        expenseDbContext.SaveChanges();
    }


    // 在這裡建立角色和使用者
    if (!await roleManager.RoleExistsAsync("Manager"))
    {
        await roleManager.CreateAsync(new IdentityRole("Manager"));
    }

    if (!await roleManager.RoleExistsAsync("Employee"))
    {
        await roleManager.CreateAsync(new IdentityRole("Employee"));
    }

    if (await userManager.FindByNameAsync("admin") == null)
    {
        var admin = new IdentityUser { UserName = "admin", Email = "admin@example.com" };
        await userManager.CreateAsync(admin, "@Admin123");
        await userManager.AddToRoleAsync(admin, "Manager");
    }

    if (await userManager.FindByNameAsync("Amber") == null)
    {
        var amber = new IdentityUser { UserName = "Amber", Email = "amber@example.com" };
        await userManager.CreateAsync(amber, "@Amber123");
        await userManager.AddToRoleAsync(amber, "Employee");
    }
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");


app.Run();
