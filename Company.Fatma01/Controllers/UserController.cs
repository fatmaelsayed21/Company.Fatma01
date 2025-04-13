using Company.DAL.Models;
using Company.PL.Dtos;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Company.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        public UserController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<UserToReturnDto> users;
            if (string.IsNullOrEmpty(SearchInput))
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    Email = U.Email,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Roles = _userManager.GetRolesAsync(U).Result


                });

            }
            else
            {
                users = _userManager.Users.Select(U => new UserToReturnDto()
                {
                    Id = U.Id,
                    UserName = U.UserName,
                    Email = U.Email,
                    FirstName = U.FirstName,
                    LastName = U.LastName,
                    Roles = _userManager.GetRolesAsync(U).Result


                }).Where(U => U.FirstName.ToLower().Contains(SearchInput.ToLower()));

            }
            return View(users);
        }



        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {

            if (id == null) return BadRequest("Invalid Id"); //400
            var user = await _userManager.FindByIdAsync(id); //because get takes int id but details take nullable int id
            if (user is null) return NotFound(new { statusCode = 404, message = $" Employee with id {id} is not found" });

            var dto = new UserToReturnDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Roles = _userManager.GetRolesAsync(user).Result

            };

            return View(ViewName, dto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
           
           
            return await Details(id,"Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, UserToReturnDto model
            )
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Id");

               var user = await _userManager.FindByIdAsync(id);
                if (user is null) return BadRequest("Invalid Operation!!");
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;
               

                
                var result =await _userManager.UpdateAsync(user);
                if (result.Succeeded) 
                { 
                    return RedirectToAction(nameof(Index));
                }
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {

            //if (id == null) return BadRequest("Invalid Id"); //400
            //var department = _departmentRepository.Get(id.Value); //because get takes int id but details take nullable int id
            //if (department is null) return NotFound(new { statusCode = 404, message = $" Department with id {id} is not found" });


            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, UserToReturnDto model)
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Id");

                var user = await _userManager.FindByIdAsync(id);
                if (user is null) return BadRequest("Invalid Operation!!");
                user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Email = model.Email;


                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }


            }

            return View(model);
        }
    }
}
