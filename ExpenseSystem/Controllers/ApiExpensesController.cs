using ExpenseSystem.Data;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Mvc;


namespace ExpenseSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApiExpensesController : ControllerBase
    {
        private readonly ExpenseDbContext _context;

        public ApiExpensesController(ExpenseDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var expenses = _context.Expenses.ToList();
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = expense.ExpenseId }, expense);
        }

    }
}