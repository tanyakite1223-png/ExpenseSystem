using ExpenseSystem.Data;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Expense expense)
        {
            if (id != expense.ExpenseId)
            {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                bool exists = _context.Expenses.Any(e => e.ExpenseId == id);

                if (!exists)
                {
                    return NotFound();
                }

                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);

            if (expense is null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return NoContent();
        }

    }
}