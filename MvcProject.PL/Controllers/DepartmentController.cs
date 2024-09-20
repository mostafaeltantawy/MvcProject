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
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // /Department/Index
        public IActionResult Index()
        {
            var departments =  _unitOfWork.DepartmentRepository.GetAll();
            _unitOfWork.Dispose();


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
                 _unitOfWork.DepartmentRepository.Add(department);
                var count = _unitOfWork.Complete();
                _unitOfWork.Dispose();  
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

            var department =  _unitOfWork.DepartmentRepository.Get(id.Value);
            _unitOfWork.Dispose();
            if (department == null)
                return NotFound();
            return View(ViewName , department);


        }


        public IActionResult Edit(int? id) 
        {
            //if (id == null)
            //{
            //    return BadRequest();    
            //}
            //var department =  _unitOfWork.DepartmentRepository.Get(id.Value);
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
                     _unitOfWork.DepartmentRepository.Update(department);
                    _unitOfWork.Dispose();

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
				 _unitOfWork.DepartmentRepository.Delete(department);
                _unitOfWork.Complete();

                _unitOfWork.Dispose();

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
