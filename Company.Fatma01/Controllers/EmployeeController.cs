using AutoMapper;
using Company.BLL.Interfaces;
using Company.DAL.Models;
using Company.PL.Dtos;
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
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees= _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                 employees= _unitOfWork.EmployeeRepository.GetbyName(SearchInput);
            }

           
            //Dictionary:
            //1.view data
            //2.view bag
            //3.temp data
            return View(employees);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var departments=_unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"]=departments;
            return View();
        }


        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if (ModelState.IsValid)    //server side validation
            {
                
               var employee= _mapper.Map<Employee>(model);

                _unitOfWork.EmployeeRepository.Add(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    TempData["Message"] = "Employee is Created !!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);

        }



        [HttpGet]
        public IActionResult Details(int? id, string ViewName = "Details")
        {

            if (id == null) return BadRequest("Invalid Id"); //400
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value); //because get takes int id but details take nullable int id
            if (employee is null) return NotFound(new { statusCode = 404, message = $" Employee with id {id} is not found" });

            var Dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(ViewName, Dto);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;

            if (id == null) return BadRequest("Invalid Id"); //400
            var employee = _unitOfWork.EmployeeRepository.Get(id.Value); //because get takes int id but details take nullable int id
            if (employee is null) return NotFound(new { statusCode = 404, message = $" Department with id {id} is not found" });
            var Dto = _mapper.Map<CreateEmployeeDto>(employee);

            return View(Dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, CreateEmployeeDto model
            )
        {

            if (ModelState.IsValid)
            {
                //if (id != employee.Id) return BadRequest(); //400
                //var employee = new Employee()
                //{
                //    Id= id,
                //    Name = model.Name,
                //    Address = model.Address,
                //    Age = model.Age,
                //    CreateAt = model.CreateAt,
                //    HiringDate = model.HiringDate,
                //    Email = model.Email,
                //    IsActive = model.IsActive,
                //    IsDeleted = model.IsDeleted,
                //    Phone = model.Phone,
                //    Salary = model.Salary,
                //    DepartmentId = model.DepartmentId

                //};
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                 _unitOfWork.EmployeeRepository.Update(employee);
               var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Delete(int? id)
        {

            //if (id == null) return BadRequest("Invalid Id"); //400
            //var department = _departmentRepository.Get(id.Value); //because get takes int id but details take nullable int id
            //if (department is null) return NotFound(new { statusCode = 404, message = $" Department with id {id} is not found" });


            return Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateEmployeeDto model)
        {

            if (ModelState.IsValid)
            {
                var employee = _mapper.Map<Employee>(model);
                employee.Id = id;
                if (id != employee.Id) return BadRequest(); //400
                _unitOfWork.EmployeeRepository.Delete(employee);
                var count = _unitOfWork.Complete();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
    }
}
