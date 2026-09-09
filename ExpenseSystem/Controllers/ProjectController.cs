using ExpenseSystem.Data;
using ExpenseSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseSystem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ExpenseDbContext _context;

        public ProjectController(ExpenseDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Index()
        {
            var projectList = new List<Project>();
            projectList = _context.Projects.Where(p => p.IsActive == true).ToList();
            return View(projectList);

        }


        [Authorize(Roles = "Manager")]
        [HttpPost]
        public IActionResult Archive(int id)
        {
            var projectResult = _context.Projects.Find(id);

            if (projectResult == null) return NotFound();

            if (projectResult.ProjectName != "一般支出")
            {
                projectResult.IsActive = false;

                _context.Update(projectResult);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");

        }
    }
}