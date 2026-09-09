using ExpenseSystem.Data;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpenseSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ApiExpensesController : ControllerBase
    {
        private readonly ExpenseDbContext _context;

        public ApiExpensesController(ExpenseDbContext context)
        {
            _context = context;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public ActionResult<List<Expense>> GetAll()
        {
            var expenses = _context.Expenses.ToList();
            return Ok(expenses);
        }


        /// <summary>
        /// 根據 ID 查詢單筆報銷紀錄
        /// </summary>
        /// <param name="id">報銷紀錄的 ID</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public ActionResult<Expense> GetById(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null)
            {
                return NotFound();
            }
            return Ok(expense);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = expense.ExpenseId }, expense);
        }


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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


        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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