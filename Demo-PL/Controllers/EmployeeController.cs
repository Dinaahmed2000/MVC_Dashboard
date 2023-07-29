using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo_PL.Helpers;
using Demo_PL.viewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo_PL.Controllers
{
    //[AllowAnonymous]   //مش لازم يكون عامل login => Default
    [Authorize]   //login
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        ///public IEmployeeRepository EmployeeRepository { get; }
        ///public EmployeeController(IEmployeeRepository employeeRepository , IDepartmentRepository departmentRepository , IMapper mapper)
        ///{
        ///    _employeeRepository = employeeRepository;
        ///    _departmentRepository = departmentRepository;
        ///    _mapper = mapper;
        ///}  
        public EmployeeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // /Employee/Index
        public async Task<IActionResult> Index(string searchValue)
        {
            //Binding is one way in MVC
            //1.ViewData => is a dectionary object (introduced in ASP.NET framework 3.5)
            //             it helps us to transfer data from controller [Action] to view
            ViewData["Message"] = "Hello view data";

            //2.ViewBag => is a dynamic property (introduced in ASP.NET framework 4.0 based on dynamic feature [keyword])
            //             it helps us to transfer data from controller [Action] to view
            ViewBag.Message = "Hello view Bag";

            IEnumerable<Employee> Employees;

            //if (string.IsNullOrEmpty(searchValue))
            //     Employees = _employeeRepository.GetAll();
            //else
            //    Employees = _employeeRepository.SearchEmployeeByName(searchValue);

            if (string.IsNullOrEmpty(searchValue))
                Employees =await _unitOfWork.EmployeeRepository.GetAll();
            else
                Employees = _unitOfWork.EmployeeRepository.SearchEmployeeByName(searchValue);
            var mappedEmp = _mapper.Map<IEnumerable<Employee>, IEnumerable<EmployeeViewModel>>(Employees);
            return View(mappedEmp);
        }

        // /Employee/Create
        //[HttpGet]
        public IActionResult Create()
        {
            //ViewBag.Departments= _departmentRepository.GetAll();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeVM)
        {
            if (ModelState.IsValid)  //server side validation
            {
                //Manual  mapping
                ///var mappedEmployee = new Employee()
                ///{
                ///    Name = employeeVM.Name,
                ///    Address = employeeVM.Address,
                ///    Age = employeeVM.Age,
                ///    EmaiL = employeeVM.EmaiL,
                ///    Salary = employeeVM.Salary,
                ///    DepartmentId = employeeVM.DepartmentId,
                ///    IsActive = employeeVM.IsActive,
                ///    HireDate = employeeVM.HireDate,
                ///    PhoneNumber = employeeVM.PhoneNumber,
                ///};

                //Employee mappedEmployee =(Employee) employeeVM;

                employeeVM.ImageName = DocumentSettings.UploadFile(employeeVM.Image, "images");
                var mappedEmployee = _mapper.Map<EmployeeViewModel,Employee>(employeeVM);
                //_employeeRepository.Add(mappedEmployee);
                await _unitOfWork.EmployeeRepository.Add(mappedEmployee);
                //update
                //delete
                await _unitOfWork.complete();
                
                return RedirectToAction(nameof(Index));
            }
            return View(employeeVM);
        }
        // /Employee/Details/1
        //[HttpGet]
        public async Task<IActionResult> Details(int? id, string viewName = "Details")
        {
            if (id is null)
            {
                return BadRequest();
            }

            //var Employee = _employeeRepository.Get(id.Value);
            var Employee =await _unitOfWork.EmployeeRepository.Get(id.Value);
            //NotFoundResult result = NotFound();
            //return result;
            if (Employee is null)
            {
                return NotFound();
            }

            var mappedEmp = _mapper.Map<Employee, EmployeeViewModel>(Employee);

            return View(viewName, mappedEmp);
        }

        // /Employee/Edit/1
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ///if (id is null)
            ///    return BadRequest();
            ///var Employee = _EmployeeRepository.Get(id.Value);
            ///if (Employee is null)
            ///    return NotFound();
            ///return View(Employee);
            ///

            return await Details(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, EmployeeViewModel employeeVm)
        {
            if (id != employeeVm.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedEmp= _mapper.Map<EmployeeViewModel,Employee>(employeeVm);
                    //_employeeRepository.Update(mappedEmp);
                    _unitOfWork.EmployeeRepository.Update(mappedEmp);
                    await _unitOfWork.complete();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1. Log Exception
                    //2. Friendly Message
                    //OR
                    ModelState.AddModelError(string.Empty, ex.Message);
                }

            }
            return View(employeeVm);
        }

        // /Employee/Delete/1
        //[HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, EmployeeViewModel employeeVm)
        {
            if (id != employeeVm.Id)
            {
                return BadRequest();
            }
                try
                {
                    var mappedEmp = _mapper.Map<EmployeeViewModel, Employee>(employeeVm);
                    //_employeeRepository.Delete(mappedEmp);
                    _unitOfWork.EmployeeRepository.Delete(mappedEmp);
                    int count=await _unitOfWork.complete();
                    if (count>0)
                        DocumentSettings.deleteFile(employeeVm.ImageName, "images");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    //1. Log Exception
                    //2. Friendly Message
                    //OR
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            return View(employeeVm);
        }
    }
}
