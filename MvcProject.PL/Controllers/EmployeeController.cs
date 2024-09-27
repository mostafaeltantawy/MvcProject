using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcProject.BLL.Interfaces;
using MvcProject.DAL.Models;
using MvcProject.PL.Helpers;
using MvcProject.PL.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MvcProject.PL.Controllers
{
	[Authorize]

	public class EmployeeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                 employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                 employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
            }
            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);




        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            //Manual mapping
            //var MappedEmployee = new Employee()
            //{
            //    Name = employeeVM.Name,
            //    Age = employeeVM.Age,
            //    Address = employeeVM.Address,
            //    PhoneNumber = employeeVM.PhoneNumber,
            //    DepartmentId = employeeVM.DepartmentId,
            //};
            //Employee employee = (Employee) employeeVM;


            var MappedEmployee = _mapper.Map<EmployeeViewModel , Employee> (employeeVM);

            if (ModelState.IsValid)
            {
                if(employeeVM.Image != null)
                    MappedEmployee.ImageName =  DocumentSettings.UploadFile(employeeVM.Image, "Images"); 
                
                await _unitOfWork.EmployeeRepository.AddAsync(MappedEmployee);

                var result = await _unitOfWork.CompleteAsync() ;
                _unitOfWork.Dispose();

                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(employeeVM);
        }

        public async Task<IActionResult>  Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();
            var employee =  await _unitOfWork.EmployeeRepository.GetAsync(id.Value);
            if (employee == null)
                return NotFound();
            var MappedEmployee =  _mapper.Map<Employee, EmployeeViewModel>(employee); 

            return View(ViewName, MappedEmployee);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Departments = await   _unitOfWork.DepartmentRepository.GetAllAsync();

            return  await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if(employeeVM.Image is not null )
                        employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "Images");

                    var MappedEmployee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    var result = await _unitOfWork.CompleteAsync();
                    if (result > 0)
                        return RedirectToAction(nameof(Index));

                }
                catch (System.Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);

                }
            }
            return View(employeeVM);


        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee);

                    var result = await  _unitOfWork.CompleteAsync() ;

                    if (result > 0 && MappedEmployee.ImageName != null)
                        DocumentSettings.DeleteFile(MappedEmployee.ImageName, "Images");
                    return RedirectToAction("Index");


                }
                catch (System.Exception ex)
                {

                    ModelState.AddModelError(string.Empty, ex.Message);

                }
            }
            return View(employeeVM);
        }

    }
}
