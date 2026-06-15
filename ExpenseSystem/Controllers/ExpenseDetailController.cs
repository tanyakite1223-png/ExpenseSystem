using ExpenseSystem.Data;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ExpenseSystem.Controllers
{
    public class ExpenseDetailController : Controller
    {
        private readonly ExpenseDbContext _Context;

        public ExpenseDetailController(ExpenseDbContext context)
        {
            _Context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            //專案名稱SelectListItem
            List<SelectListItem> items = new List<SelectListItem>();
            var projectList = _Context.Projects.Where(p => p.IsActive == true).ToList();
            foreach (var item in projectList)
            {
                items.Add(new SelectListItem
                {
                    Text = item.ProjectName,
                    Value = item.ProjectId.ToString()
                });
            }
            ViewBag.projectSelect = items;

            //費土類型SelectListItem
            ViewBag.categorySelect = GetSelectListItems();
            return View();
        }

        [HttpPost]
        public IActionResult Create(ExpenseDetail expenseDetail)
        {
            expenseDetail.ExpenseId = 14;
            _Context.Add(expenseDetail);
            _Context.SaveChanges();

            return RedirectToAction("Index");
        }

        private List<SelectListItem> GetSelectListItems()
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
