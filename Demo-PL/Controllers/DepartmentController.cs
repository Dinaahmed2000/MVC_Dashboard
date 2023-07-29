using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.BLL.Repositories;
using Demo.DAL.Models;
using Demo_PL.viewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo_PL.Controllers
{
    //Inheritance : DepartmentController is  a Controller
    //Aggregation : DepartmentController has a DepartmentRepository
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        ///public IDepartmentRepository DepartmentRepository { get; }
        ///public DepartmentController(IDepartmentRepository departmentRepository,IMapper mapper)  //Ask CLR for creating object from class implementing interface "IDepartmentRepository"
        ///{
        ///    _departmentRepository= departmentRepository;
        ///    _mapper = mapper;
        ///    //DepartmentRepository= departmentRepository;
        ///}

        public DepartmentController(IUnitOfWork unitOfWork, IMapper mapper)  //Ask CLR for creating object from class implementing interface "IDepartmentRepository"
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // /Department/Index
        public async Task<IActionResult> Index(string searchValue)
        {
            //var departments= _departmentRepository.GetAll();
            //var mappedDept = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            //return View(mappedDept);

            IEnumerable<Department> departments;
            if (string.IsNullOrEmpty(searchValue))
            {
                 //departments = _departmentRepository.GetAll();
                 departments =await _unitOfWork.DepartmentRepository.GetAll();
            }
            else
            {
                //departments = _departmentRepository.SearchDepartmentByName(searchValue);
                departments = _unitOfWork.DepartmentRepository.SearchDepartmentByName(searchValue);
            }
            var mappedDept = _mapper.Map<IEnumerable<Department>, IEnumerable<DepartmentViewModel>>(departments);
            return View(mappedDept);

        }

        // /Department/Create
        //[HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentViewModel departmentVM)
        {
            if (ModelState.IsValid)
            {
                var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                //int count=_departmentRepository.Add(mappedDept);
                //int count=_departmentRepository.Add(mappedDept);
               await _unitOfWork.DepartmentRepository.Add(mappedDept);
                int count =await _unitOfWork.complete();
                //3.TempData
                if (count>0)
                    TempData["Message"] = "Department is created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(departmentVM);
        }
        // /Department/Details/1
        //[HttpGet]
        public async Task<IActionResult> Details(int? id , string viewName="Details")
        {
            if (id is null)
            {
                return BadRequest();
            }

            //var department = _departmentRepository.Get(id.Value);
            var department =await _unitOfWork.DepartmentRepository.Get(id.Value);
            //NotFoundResult result = NotFound();
            //return result;
            if (department is null)
            {
                return NotFound();
            }

            var mappedDept = _mapper.Map<Department, DepartmentViewModel>(department);
            return View(viewName, mappedDept);
        }

        // /Department/Edit/1
        //[HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ///if (id is null)
            ///    return BadRequest();
            ///var department = _departmentRepository.Get(id.Value);
            ///if (department is null)
            ///    return NotFound();
            ///return View(department);
            ///

            return await Details(id,"Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    //_departmentRepository.Update(mappedDept);
                    _unitOfWork.DepartmentRepository.Update(mappedDept);
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
            return View(departmentVM);
        }

        // /Department/Delete/1
        //[HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            return await Details(id, "Delete");
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromRoute] int id, DepartmentViewModel departmentVM)
        {
            if (id != departmentVM.Id)
            {
                return BadRequest();
            }
                try
                {
                    var mappedDept = _mapper.Map<DepartmentViewModel, Department>(departmentVM);
                    //_departmentRepository.Delete(mappedDept);
                    _unitOfWork.DepartmentRepository.Delete(mappedDept);
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
            return View(departmentVM);
        }
    }
}
