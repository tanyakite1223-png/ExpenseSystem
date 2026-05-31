using ExpenseSystem.Data;
using Microsoft.AspNetCore.Mvc;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace ExpenseSystem.Controllers
{
    [Authorize]
    public class ExpensesController : Controller
    {
        private readonly ExpenseDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ExpensesController(ExpenseDbContext context, UserManager<IdentityUser> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }


        public IActionResult Index()
        {
            var expenseList = new List<Expense>();

            if (User.IsInRole(role: "Manager"))
            {
                expenseList = _context.Expenses.Where(e => e.IsDeleted == false && e.Status != ExpenseStatus.Draft).ToList();
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                expenseList = _context.Expenses.Where(e => e.IsDeleted == false && e.ApplicantId == userId).ToList();
            }
            return View(expenseList);

        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                expense.ApplicantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                _context.Expenses.Add(expense);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(expense);

        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null)
            {
                return NotFound();
            }

            if (User.IsInRole(role: "Manager"))
            {
                ExpenseStatus statusApproved = ExpenseStatus.Approved;
                ExpenseStatus statusRejected = ExpenseStatus.Rejected;

                string approved = statusApproved.ToString();
                string rejected = statusRejected.ToString();

                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = approved, Value = approved });
                items.Add(new SelectListItem { Text = rejected, Value = rejected });

                ViewBag.selectItem = items;
            }

            return View(expense);
        }

        [HttpPost]
        public IActionResult Edit(Expense expense)
        {

            if (expense.Status == ExpenseStatus.Rejected && string.IsNullOrWhiteSpace(expense.RejectionReason))
            {
                ModelState.AddModelError("RejectionReason", "請輸入拒絕原因");
            }

            if (ModelState.IsValid)
            {
                _context.Expenses.Update(expense);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            if (User.IsInRole(role: "Manager"))
            {
                ExpenseStatus statusApproved = ExpenseStatus.Approved;
                ExpenseStatus statusRejected = ExpenseStatus.Rejected;

                string approved = statusApproved.ToString();
                string rejected = statusRejected.ToString();

                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = approved, Value = approved });
                items.Add(new SelectListItem { Text = rejected, Value = rejected });

                ViewBag.selectItem = items;
            }
            return View(expense);

        }


        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = _context.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }

            var userName = (await _userManager.FindByIdAsync(expense.ApplicantId))?.UserName;

            ViewBag.username = userName;


            return View(expense);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var expense = _context.Expenses.Find(id);

            expense.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Submitted(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null)
            {
                return NotFound();
            }
            expense.Status = ExpenseStatus.Submitted;
            _context.Expenses.Update(expense);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}