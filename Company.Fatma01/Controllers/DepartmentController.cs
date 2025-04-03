using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    //MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController( IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }
        [HttpGet] //Get : Department/index
        public IActionResult Index()
        {
          
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid)    //server side validation
            {
                var department= new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt

                };

               var count= _departmentRepository.Add(department);
                if (count > 0)
                { 
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);

        }
        [HttpGet]
        public IActionResult Details(int? id) {

            if (id == null)  return BadRequest("Invalid Id"); //400
           var department= _departmentRepository.Get(id.Value); //because get takes int id but details take nullable int id
            if (department is null) return NotFound(new {statusCode=404,message =$" Department with id {id} is not found" });
        
            return View(department);
        }

    }
}
