using Company.BLL.Interfaces;
using Company.BLL.Repositories;
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
    }
}
