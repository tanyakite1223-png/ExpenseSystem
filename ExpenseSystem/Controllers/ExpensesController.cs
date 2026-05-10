using ExpenseSystem.Data;
using Microsoft.AspNetCore.Mvc;


namespace ExpenseSystem.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly ExpenseDbContext _context;

        public ExpensesController(ExpenseDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var expenses = _context.Expenses.ToList();
            return View(expenses);
        }
    }
}

