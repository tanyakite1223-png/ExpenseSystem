using Microsoft.EntityFrameworkCore;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;


namespace ExpenseSystem.Data
{
    public class ExpenseDbContext : IdentityDbContext<IdentityUser>
    {
        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ExpenseDetail> ExpenseDetails { get; set; }
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // 必須靠它來幫 Identity 鋪路

            //ExpenseDetail
            modelBuilder.Entity<ExpenseDetail>()
            .HasOne(e => e.Expense)             // 以Entity<ExpenseDetail>為出發，指向最上一層Table  Expense
            .WithMany(ed => ed.ExpenseDetails)  // 多筆 Rows的集合
            .HasForeignKey(ed => ed.ExpenseId)  // ExpenseDetail裡面，用來指向Expense Table的 ID 欄位
            .OnDelete(DeleteBehavior.Restrict);

            //Project類型
            modelBuilder.Entity<ExpenseDetail>()
            .HasOne(p => p.Project)             // 以Entity<ExpenseDetail>為出發，指向最上一層Table  Project
            .WithMany(ed => ed.ExpenseDetails)  // 多筆 Rows的集合
            .HasForeignKey(ed => ed.ProjectId)  // ExpenseDetail裡面，用來指向Project Table的 ID 欄位
            .OnDelete(DeleteBehavior.Restrict);
        }



    }
}