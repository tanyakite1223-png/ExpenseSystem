using ExpenseSystem.Data;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseSystem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ExpenseDbContext _Context;

        public ProjectController(ExpenseDbContext context)
        {
            _Context = context;
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Index()
        {
            var projectList = new List<Project>();
            projectList = _Context.Projects.Where(p => p.IsActive == true).ToList();
            return View(projectList);

        }


        [Authorize(Roles = "Manager")]
        [HttpPost]
        public IActionResult Archive(int id)
        {
            var projectResult = _Context.Projects.Find(id);

            if (projectResult == null) return NotFound();

            if (projectResult.ProjectName != "一般支出")
            {
                projectResult.IsActive = false;

                _Context.Update(projectResult);
                _Context.SaveChanges();
            }
            return RedirectToAction("Index");

        }
    }
}