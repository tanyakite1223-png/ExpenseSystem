using ExpenseSystem.Data;
using Microsoft.AspNetCore.Mvc;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


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


        public async Task<IActionResult> Index(int page = 1)
        {
            var expenseList = new List<Expense>();
            int TotalPages = 0;

            if (page < 1) page = 1;

            int pageSize = 10;
            int skipRows = (page - 1) * pageSize;

            if (User.IsInRole(role: "Manager"))
            {
                expenseList = _context.Expenses.Where(e => e.IsDeleted == false && e.Status != ExpenseStatus.Draft && e.Status != ExpenseStatus.Returned).Include(ed => ed.ExpenseDetails).OrderByDescending(e => e.ExpenseId).Skip(skipRows).Take(pageSize).ToList();
                TotalPages = _context.Expenses.Where(e => e.IsDeleted == false && e.Status != ExpenseStatus.Draft && e.Status != ExpenseStatus.Returned).Count();
            }
            else
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                expenseList = _context.Expenses.Where(e => e.IsDeleted == false && e.ApplicantId == userId).Include(ed => ed.ExpenseDetails).OrderByDescending(e => e.ExpenseId).Skip(skipRows).Take(pageSize).ToList();
                TotalPages = _context.Expenses.Where(e => e.IsDeleted == false && e.ApplicantId == userId).Count();
            }

            ViewBag.CurrentPage = page;


            if (TotalPages % pageSize == 0)
            {
                TotalPages = TotalPages / pageSize;
            }
            else
            {
                TotalPages = (TotalPages / pageSize) + 1;
            }

            ViewBag.TotalPages = TotalPages;

            foreach (var item in expenseList)
            {
                item.ApplicantId = await GetApplicantNameAsync(item.ApplicantId);
            }

            return View(expenseList);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var viewModel = new ExpenseWithDetailsViewModel()
            {
                Expense = new Expense(),
                ExpenseDetails = [
                    new ExpenseDetail(),
                    new ExpenseDetail()
                ]
            };

            ViewBag.expenseDetails = viewModel.ExpenseDetails.Count;


            //專案名稱SelectListItem
            List<SelectListItem> items = new List<SelectListItem>();
            var projectList = _context.Projects.Where(p => p.IsActive == true).ToList();
            foreach (var item in projectList)
            {
                items.Add(new SelectListItem
                {
                    Text = item.ProjectName,
                    Value = item.ProjectId.ToString()
                });
            }
            ViewBag.projectSelect = items;

            //單據類型
            ViewBag.Receipt = GetReceiptSelectListItems();

            //費用類型
            ViewBag.Category = GetCategorySelectListItems();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ExpenseWithDetailsViewModel viewModel)
        {
            var result = viewModel.ExpenseDetails;
            ViewBag.expenseDetails = result.Count;

            for (int i = 0; i < result.Count; i++)
            {
                if (result[i].ReceiptType == ExpenseReceipt.UniformInvoice && string.IsNullOrWhiteSpace(result[i].InvoiceNumber))
                {
                    ModelState.AddModelError($"ExpenseDetails[{i}].InvoiceNumber", "統一發票必填");
                }
            }


            if (ModelState.IsValid)
            {
                viewModel.Expense.ApplicantId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                viewModel.Expense.CreatedAt = DateTime.UtcNow;
                viewModel.Expense.ExpenseDetails = viewModel.ExpenseDetails;

                _context.Expenses.Add(viewModel.Expense);
                _context.SaveChanges();

                return RedirectToAction("Detail", new { id = viewModel.Expense.ExpenseId });
            }

            //專案名稱SelectListItem
            List<SelectListItem> items = new List<SelectListItem>();
            var projectList = _context.Projects.Where(p => p.IsActive == true).ToList();
            foreach (var item in projectList)
            {
                items.Add(new SelectListItem
                {
                    Text = item.ProjectName,
                    Value = item.ProjectId.ToString()
                });
            }
            ViewBag.projectSelect = items;

            //單據類型
            ViewBag.Receipt = GetReceiptSelectListItems();

            //費用類型
            ViewBag.Category = GetCategorySelectListItems();

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _context.Expenses.Include(ed => ed.ExpenseDetails).FirstOrDefaultAsync(e => e.ExpenseId == id);
            if (result is null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            ViewBag.username = await GetApplicantNameAsync(result.ApplicantId);

            if (result.ApplicantId != userId || (result.Status != ExpenseStatus.Draft && result.Status != ExpenseStatus.Returned))
            {
                return Forbid();
            }

            //專案名稱SelectListItem
            List<SelectListItem> items = new List<SelectListItem>();
            var projectList = _context.Projects.Where(p => p.IsActive == true).ToList();
            foreach (var item in projectList)
            {
                items.Add(new SelectListItem
                {
                    Text = item.ProjectName,
                    Value = item.ProjectId.ToString()
                });
            }
            ViewBag.projectSelect = items;

            //單據類型
            ViewBag.Receipt = GetReceiptSelectListItems();

            //費用類型
            ViewBag.Category = GetCategorySelectListItems();

            return View(result);

        }


        [HttpPost]
        public async Task<IActionResult> Edit(Expense expense)
        {
            var _expense = _context.Expenses.AsNoTracking().Include(ed => ed.ExpenseDetails).FirstOrDefault(e => e.ExpenseId == expense.ExpenseId);
            if (_expense is null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (_expense.ApplicantId != userId || (_expense.Status != ExpenseStatus.Draft && _expense.Status != ExpenseStatus.Returned))
            {
                return Forbid();
            }

            expense.ExpenseDetails ??= new List<ExpenseDetail>();

            for (int i = 0; i < expense.ExpenseDetails.Count; i++)
            {
                if (ExpenseReceipt.UniformInvoice == expense.ExpenseDetails[i].ReceiptType && string.IsNullOrWhiteSpace(expense.ExpenseDetails[i].InvoiceNumber))
                {
                    ModelState.AddModelError($"ExpenseDetails[{i}].InvoiceNumber", "請輸入統一發票號碼");
                }
            }

            if (ModelState.IsValid)
            {
                if (ExpenseStatus.Returned == expense.Status)
                {
                    _expense.Status = ExpenseStatus.Submitted;
                }
                expense.ApplicantId = _expense.ApplicantId;
                expense.IsDeleted = _expense.IsDeleted;
                expense.CreatedAt = _expense.CreatedAt;
                _context.Expenses.Update(expense);

                foreach (var item in expense.ExpenseDetails)
                {
                    _context.ExpenseDetails.Update(item);
                }

                _context.SaveChanges();
                return RedirectToAction("Index");
            }

            //專案名稱SelectListItem
            List<SelectListItem> items = new List<SelectListItem>();
            var projectList = _context.Projects.Where(p => p.IsActive == true).ToList();
            foreach (var item in projectList)
            {
                items.Add(new SelectListItem
                {
                    Text = item.ProjectName,
                    Value = item.ProjectId.ToString()
                });
            }
            ViewBag.projectSelect = items;

            //單據類型
            ViewBag.Receipt = GetReceiptSelectListItems();

            //費用類型
            ViewBag.Category = GetCategorySelectListItems();
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
            if (expense == null) return NotFound();

            expense.IsDeleted = true;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }


        [Authorize(Roles = "Manager")]
        [HttpGet]
        public async Task<IActionResult> Review(int id)
        {
            var expense = _context.Expenses.AsNoTracking().Include(ed => ed.ExpenseDetails).ThenInclude(p => p.Project).FirstOrDefault(e => e.ExpenseId == id);
            if (expense == null) return NotFound();

            ViewBag.username = await GetApplicantNameAsync(expense.ApplicantId);

            if (User.IsInRole(role: "Manager"))
            {
                ViewBag.selectItem = GetStatusSelectListItems();
            }

            return View(expense);
        }


        [Authorize(Roles = "Manager")]
        [HttpPost]
        public async Task<IActionResult> Review(Expense expense)
        {
            var _expense = _context.Expenses.AsNoTracking().FirstOrDefault(e => e.ExpenseId == expense.ExpenseId);

            if (expense.Status == ExpenseStatus.Rejected && string.IsNullOrWhiteSpace(expense.RejectionReason))
            {
                ModelState.AddModelError("RejectionReason", "請輸入拒絕原因");
            }

            if (ModelState.IsValid)
            {
                expense.ApplicantId = _expense.ApplicantId;
                expense.IsDeleted = _expense.IsDeleted;
                expense.CreatedAt = _expense.CreatedAt;
                _context.Expenses.Update(expense);
                _context.SaveChanges();

                return RedirectToAction("Index");

            }

            ViewBag.selectItem = GetStatusSelectListItems();

            return View(expense);
        }



        public async Task<IActionResult> Detail(int id)
        {
            var expense = await _context.Expenses.Include(ed => ed.ExpenseDetails)
                             .ThenInclude(p => p.Project)
                             .FirstOrDefaultAsync(e => e.ExpenseId == id);

            if (expense == null) return NotFound();

            return View(expense);
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
            var user = await _userManager.FindByIdAsync(id);
            return user?.UserName;
        }

        private async Task<string> GetApplicantIdAsync(string name)
        {
            var user = await _userManager.FindByNameAsync(name);
            return user?.Id;
        }


        private List<SelectListItem> GetStatusSelectListItems()
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

            return items;
        }

        private List<SelectListItem> GetReceiptSelectListItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (ExpenseReceipt Receipt in Enum.GetValues(typeof(ExpenseReceipt)))
            {
                items.Add(new SelectListItem
                {
                    Text = EnumExtensions.GetDisplayName(Receipt),
                    Value = Receipt.ToString()
                });
            }

            return items;
        }


        private List<SelectListItem> GetCategorySelectListItems()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            foreach (ExpenseCategory category in Enum.GetValues(typeof(ExpenseCategory)))
            {
                items.Add(new SelectListItem
                {
                    Text = EnumExtensions.GetDisplayName(category),
                    Value = category.ToString()
                });
            }

            return items;
        }

    }
}