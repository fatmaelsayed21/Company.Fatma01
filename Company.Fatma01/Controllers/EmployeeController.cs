using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.Dtos;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IEmployeeRepository _EmployeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            //_EmployeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _mapper = mapper;
        }
        [HttpGet] //Get : Department/index
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees=await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                 employees= await _unitOfWork.EmployeeRepository.GetbyNameAsync(SearchInput);
            }

           
            //Dictionary:
            //1.view data
            //2.view bag
            //3.temp data
            return View(employees);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments= await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"]=departments;
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)    //server side validation
            {

                if (model.Image is not null)
                {
                   model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                }



                var employee= _mapper.Map<Employee>(model);

               await _unitOfWork.EmployeeRepository.AddAsync(employee);
               
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {

                    TempData["Message"] = "Employee is Created !!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);

        }



        [HttpGet]
        public async Task<IActionResult> Details(int? id, string ViewName = "Details")
        {

            if (id == null) return BadRequest("Invalid Id"); //400
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value); //because get takes int id but details take nullable int id
            if (employee is null) return NotFound(new { statusCode = 404, message = $" Employee with id {id} is not found" });

            var Dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(ViewName, Dto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;

            if (id == null) return BadRequest("Invalid Id"); //400
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(id.Value); //because get takes int id but details take nullable int id
            if (employee is null) return NotFound(new { statusCode = 404, message = $" Department with id {id} is not found" });
            var Dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(Dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] int id, CreateEmployeeDto model
            )
        {

            if (ModelState.IsValid)
            {


                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName,"Images");
                }
                if (model.Image is not null)
                {
                  model.ImageName=  DocumentSettings.UploadFile(model.Image, "Images");
                }

                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                 _unitOfWork.EmployeeRepository.Update(employee);
               var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {

            //if (id == null) return BadRequest("Invalid Id"); //400
            //var department = _departmentRepository.Get(id.Value); //because get takes int id but details take nullable int id
            //if (department is null) return NotFound(new { statusCode = 404, message = $" Department with id {id} is not found" });


            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, CreateEmployeeDto model)
        {

            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                if (id != employee.Id) return BadRequest(); //400
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    if (model.ImageName is not null) {
                        DocumentSettings.DeleteFile(model.ImageName, "Images");
                    }
                    
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
