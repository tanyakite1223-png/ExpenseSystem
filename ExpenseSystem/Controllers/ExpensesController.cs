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


        public async Task<IActionResult> Index()
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

            foreach (var item in expenseList)
            {
                item.ApplicantId = await GetApplicantNameAsync(item.ApplicantId);
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
        public async Task<IActionResult> Edit(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null) return NotFound();

            ViewBag.username = await GetApplicantNameAsync(expense.ApplicantId);

            if (User.IsInRole(role: "Manager"))
            {
                ExpenseStatus statusReturned = ExpenseStatus.Returned;
                ExpenseStatus statusApproved = ExpenseStatus.Approved;
                ExpenseStatus statusRejected = ExpenseStatus.Rejected;

                string returned = EnumExtensions.GetDisplayName(statusReturned);
                string approved = EnumExtensions.GetDisplayName(statusApproved);
                string rejected = EnumExtensions.GetDisplayName(statusRejected);

                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = returned, Value = statusReturned.ToString() });
                items.Add(new SelectListItem { Text = approved, Value = statusApproved.ToString() });
                items.Add(new SelectListItem { Text = rejected, Value = statusRejected.ToString() });

                ViewBag.selectItem = items;
            }

            return View(expense);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Expense expense)
        {
            if (expense.Status == ExpenseStatus.Rejected && string.IsNullOrWhiteSpace(expense.RejectionReason))
            {
                ModelState.AddModelError("RejectionReason", "請輸入拒絕原因");
            }

            if (ModelState.IsValid)
            {
                if (User.IsInRole("Manager"))
                {
                    expense.ApplicantId = await GetApplicantIdAsync(expense.ApplicantId);
                }

                if (!User.IsInRole("Manager") && expense.Status == ExpenseStatus.Returned)
                {
                    expense.Status = ExpenseStatus.Submitted;
                }

                if (User.IsInRole("Employee") && expense.Status == ExpenseStatus.Returned)
                {
                    expense.Status = ExpenseStatus.Submitted;
                }

                _context.Expenses.Update(expense);
                _context.SaveChanges();

                return RedirectToAction("Index");

            }

            if (User.IsInRole(role: "Manager"))
            {
                ExpenseStatus statusReturned = ExpenseStatus.Returned;
                ExpenseStatus statusApproved = ExpenseStatus.Approved;
                ExpenseStatus statusRejected = ExpenseStatus.Rejected;

                string returned = EnumExtensions.GetDisplayName(statusReturned);
                string approved = EnumExtensions.GetDisplayName(statusApproved);
                string rejected = EnumExtensions.GetDisplayName(statusRejected);

                List<SelectListItem> items = new List<SelectListItem>();
                items.Add(new SelectListItem { Text = returned, Value = statusReturned.ToString() });
                items.Add(new SelectListItem { Text = approved, Value = statusApproved.ToString() });
                items.Add(new SelectListItem { Text = rejected, Value = statusRejected.ToString() });

                ViewBag.selectItem = items;
            }

            return View(expense);
        }


        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var expense = _context.Expenses.Find(id);

            if (expense == null) return NotFound();

            ViewBag.username = await GetApplicantNameAsync(expense.ApplicantId);
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

            if (expense == null) return NotFound();

            expense.Status = ExpenseStatus.Submitted;

            _context.Expenses.Update(expense);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private async Task<string> GetApplicantNameAsync(string id)
        {
            var user = (await _userManager.FindByIdAsync(id));
            return user?.UserName;
        }

        private async Task<string> GetApplicantIdAsync(string name)
        {
            var user = (await _userManager.FindByNameAsync(name));
            return user?.Id;
        }
    }
}