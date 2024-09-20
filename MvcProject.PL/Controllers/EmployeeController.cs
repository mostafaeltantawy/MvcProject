using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MvcProject.BLL.Interfaces;
using MvcProject.DAL.Models;
using MvcProject.PL.ViewModels;
using System.Collections.Generic;

namespace MvcProject.PL.Controllers
{
    public class EmployeeController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IActionResult Index(string SearchValue)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchValue))
            {
                 employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                 employees = _unitOfWork.EmployeeRepository.GetEmployeesByName(SearchValue);
            }
            var MappedEmployees = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(employees);
            return View(MappedEmployees);




        }
        public IActionResult Create()
        {
            ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeVM)
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
                _unitOfWork.EmployeeRepository.Add(MappedEmployee);

                var result =_unitOfWork.Complete() ;
                _unitOfWork.Dispose();

                if (result > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(employeeVM);
        }

        public IActionResult Details(int? id, string ViewName = "Details")
        {
            if (id == null)
                return BadRequest();
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value);
            if (employee == null)
                return NotFound();
            var MappedEmployee = _mapper.Map<Employee, EmployeeViewModel>(employee); 

            return View(ViewName, MappedEmployee);
        }

        public IActionResult Edit(int? id)
        {
            ViewBag.Departments =  _unitOfWork.DepartmentRepository.GetAll();

            return Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Update(MappedEmployee);
                    var result = _unitOfWork.Complete();
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

      
        public IActionResult Delete(int? id)
        {
            return Details(id, "Delete");
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(EmployeeViewModel employeeVM, [FromRoute] int id)
        {
            if (id != employeeVM.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    var MappedEmployee = _mapper.Map<EmployeeViewModel, Employee>(employeeVM);
                    _unitOfWork.EmployeeRepository.Delete(MappedEmployee);

                    var result = _unitOfWork.Complete() ;
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

    }
}
