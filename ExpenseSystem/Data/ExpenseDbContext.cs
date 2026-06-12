using Microsoft.EntityFrameworkCore;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ExpenseSystem.Data
{
    public class ExpenseDbContext : IdentityDbContext<IdentityUser>
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ExpenseDetail> ExpenseDetails { get; set; }
    }
}