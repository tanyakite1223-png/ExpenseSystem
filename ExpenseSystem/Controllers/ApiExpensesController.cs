using ExpenseSystem.Data;
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

    }
}