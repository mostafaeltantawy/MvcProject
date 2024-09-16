using Microsoft.AspNetCore.Mvc;
using MvcProject.BLL.Interfaces;
using MvcProject.BLL.Repositories;
using MvcProject.DAL.Models;
using System.Linq;

namespace MvcProject.PL.Controllers
{
    public class DepartmentController : Controller
    {

        private readonly IDepartmentRepository _departmentsRepo;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentsRepo = departmentRepository;
        }

        // /Department/Index
        public IActionResult Index()
        {
            var departments = _departmentsRepo.GetAll();

            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Department department) // Take the field from the submitted for that their name matches classfield names
        {
            if (ModelState.IsValid) // server side validation
            {
                var count = _departmentsRepo.Add(department);
                if (count > 0) 
                    return RedirectToAction(nameof(Index));
            }

            return View(department);
        }
    }
}
