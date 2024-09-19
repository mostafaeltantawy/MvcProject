using Microsoft.AspNetCore.Mvc;
using MvcProject.BLL.Interfaces;
using MvcProject.BLL.Repositories;
using MvcProject.DAL.Models;
using System;
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
                    TempData["Message"] = "Department Is Created"; 
                    return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // BaseURl/Department/Details/100
        public IActionResult Details(int? id , string ViewName = "Details") 
        {
            if(id == null)
                return BadRequest(); // Stauts code 400

            var department = _departmentsRepo.Get(id.Value); 
            if(department == null)
                return NotFound();
            return View(ViewName , department);


        }


        public IActionResult Edit(int? id) 
        {
            //if (id == null)
            //{
            //    return BadRequest();    
            //}
            //var department = _departmentsRepo.Get(id.Value);
            //if(department == null)
            //{
            //    return NotFound();
            //}
            //return View(department);
            return Details( id , "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department department ,[FromRoute] int id) 
        {
            if (id != department.Id)
                return BadRequest(); 
            if (ModelState.IsValid) 
            {
                try
                {
                    _departmentsRepo.Update(department);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    
                }
           
            }
            return View(department); 

        }


        public IActionResult Delete(int? id) 
        {
            return Details(id  , "Delete"); 

        }

        [HttpPost]
        public IActionResult Delete(Department department , [FromRoute]int id) 
        {
            if(department.Id != id)
                return BadRequest();

            try
            {
				_departmentsRepo.Delete(department);
				return RedirectToAction(nameof(Index));


			}
			catch (Exception ex)
            {

                ModelState.AddModelError(string.Empty , ex.Message);
                return View(department);
            }
        }

    }
}
