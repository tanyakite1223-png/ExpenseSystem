using Microsoft.EntityFrameworkCore;
using ExpenseSystem.Models;
namespace ExpenseSystem.Data
{
    public class ExpenseDbContext : DbContext
    {
        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
        {
        }

        public DbSet<Expense> Expenses { get; set; }
    }
}