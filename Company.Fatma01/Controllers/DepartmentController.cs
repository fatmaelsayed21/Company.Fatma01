using AutoMapper;
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
        private readonly IMapper _mapper;

        public DepartmentController( IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
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
                //var department= new Department()
                //{
                //    Code = model.Code,
                //    Name = model.Name,
                //    CreateAt = model.CreateAt

                //};
                var department = _mapper.Map<Department>(model);

               var count= _departmentRepository.Add(department);
                if (count > 0)
                { 
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);

        }



        [HttpGet]
        public IActionResult Details(int? id , string ViewName="Details") {

            if (id == null)  return BadRequest("Invalid Id"); //400
           var department= _departmentRepository.Get(id.Value); //because get takes int id but details take nullable int id
            if (department is null) return NotFound(new {statusCode=404,message =$" Department with id {id} is not found" });
            var dto = _mapper.Map<CreateDepartmentDto>(department);

            return View(ViewName,dto);
        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {

            if (id == null) return BadRequest("Invalid Id"); //400
            var department = _departmentRepository.Get(id.Value); //because get takes int id but details take nullable int id
            if (department is null) return NotFound(new { statusCode = 404, message = $" Department with id {id} is not found" });

            //var departmentDto = new CreateDepartmentDto()
            //{
            //    Code = department.Code,
            //    Name = department.Name,
            //    CreateAt = department.CreateAt

            //};
            var dto = _mapper.Map<CreateDepartmentDto>(department);

            return View(dto);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,CreateDepartmentDto model)
        {

            if (ModelState.IsValid)
            {
                //if(id!=department.Id) return BadRequest(); //400

                //var department = new Department()
                //{
                //    Code = model.Code,
                //    Name = model.Name,
                //    CreateAt = model.CreateAt

                //};
                var department = _mapper.Map<Department>(model);
                var count = _departmentRepository.Update(department);
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

            return Details(id,"Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, CreateDepartmentDto model)
        {

            if (ModelState.IsValid)
            {
                var department = _mapper.Map<Department>(model);
                department.Id = id;
                if (id != department.Id) return BadRequest(); //400
                var count = _departmentRepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }

    }
}
