using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.BLL.Interfaces;
using MvcProject.BLL.Repositories;
using MvcProject.DAL.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MvcProject.PL.Controllers
{
	[Authorize]

	public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // /Department/Index
        public async Task<IActionResult> Index()
        {
            var departments = await  _unitOfWork.DepartmentRepository.GetAllAsync();
            _unitOfWork.Dispose();


            return View(departments);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department) // Take the field from the submitted for that their name matches classfield names
        {
            if (ModelState.IsValid) // server side validation
            {
                 _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                _unitOfWork.Dispose();  
                if (count > 0)
                    TempData["Message"] = "Department Is Created"; 
                    return RedirectToAction(nameof(Index));
            }

            return View(department);
        }

        // BaseURl/Department/Details/100
        public async Task<IActionResult> Details(int? id , string ViewName = "Details") 
        {
            if(id == null)
                return BadRequest(); // Stauts code 400

            var department =  await _unitOfWork.DepartmentRepository.GetAsync(id.Value);
            _unitOfWork.Dispose();
            if (department == null)
                return NotFound();
            return View(ViewName , department);


        }


        public async Task<IActionResult> Edit(int? id) 
        {
      
            return await Details( id , "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Department department ,[FromRoute] int id) 
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


        public async Task<IActionResult> Delete(int? id) 
        {
            return await Details(id  , "Delete"); 

        }

        [HttpPost]
        public async  Task<IActionResult> Delete(Department department , [FromRoute]int id) 
        {
            if(department.Id != id)
                return BadRequest();

            try
            {
				 _unitOfWork.DepartmentRepository.Delete(department);
                 await _unitOfWork.CompleteAsync();

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
