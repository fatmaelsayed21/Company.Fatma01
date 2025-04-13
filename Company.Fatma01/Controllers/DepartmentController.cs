using AutoMapper;
using Company.BLL.Interfaces;
using Company.BLL.Repositories;
using Company.DAL.Models;
using Company.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    //MVC Controller
    [Authorize]
    public class DepartmentController : Controller
    {
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentController( IUnitOfWork unitOfWork, 
            IMapper mapper)
        {
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet] //Get : Department/index
        public async  Task<IActionResult> Index()
        {
          
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
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

               await _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                { 
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);

        }



        [HttpGet]
        public async Task< IActionResult> Details(int? id , string ViewName="Details") {

            if (id == null)  return BadRequest("Invalid Id"); //400
           var department= await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value); //because get takes int id but details take nullable int id
            if (department is null) return NotFound(new {statusCode=404,message =$" Department with id {id} is not found" });
            var dto = _mapper.Map<Department>(department);

            return View(ViewName,dto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {

            if (id == null) return BadRequest("Invalid Id"); //400
            var department =await  _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value); //because get takes int id but details take nullable int id
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
        public async Task<IActionResult> Edit([FromRoute]int id,CreateDepartmentDto model)
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
                department.Id = id;
                _unitOfWork.DepartmentRepository.Update(department);
                var count =await  _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {

            //if (id == null) return BadRequest("Invalid Id"); //400
            //var department = _departmentRepository.Get(id.Value); //because get takes int id but details take nullable int id
            //if (department is null) return NotFound(new { statusCode = 404, message = $" Department with id {id} is not found" });

            return await Details(id,"Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
       
        public async Task<IActionResult> Delete([FromRoute] int? id, Department model)
        {
            if (id is null) return BadRequest($" This Id = {id} InValid");

            var department = await _unitOfWork.DepartmentRepository.GetByIdAsync(id.Value);
            _unitOfWork.DepartmentRepository.Delete(department);
            var Count = await _unitOfWork.CompleteAsync();

            if (Count > 0)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

    }
}
