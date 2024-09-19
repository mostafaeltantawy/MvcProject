using Microsoft.AspNetCore.Mvc;
using MvcProject.BLL.Interfaces;
using MvcProject.DAL.Models;

namespace MvcProject.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeController(IEmployeeRepository employeeRepository , IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }
        public IActionResult Index()
        {
            var employess = _employeeRepository.GetAll();
            // 1. ViewData
            //ViewData["Message"] = "Hello From View Data";
            ViewBag.Message= "Hello From View Bag"; 

            return View(employess);
        }
        public IActionResult Create()
        {
            ViewBag.Departments =  _departmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var result = _employeeRepository.Add(employee);

                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(employee);
        }

        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();
            var employee = _employeeRepository.Get(id.Value);
            if (employee == null)
                return NotFound();

            return View(ViewName, employee);
        }

        public IActionResult Edit(int? id)
        {
            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee employee, [FromRoute] int id)
        {
            if (id != employee.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _employeeRepository.Update(employee);
                    if (result > 0)
                        return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);

                }
            }
            return View(employee);


        }

      
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Employee employee, [FromRoute] int id)
        {
            if (id != employee.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var result = _employeeRepository.Delete(employee);
                    if (result > 0)
                        return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);

                }
            }
            return View(employee);
        }

    }
}
