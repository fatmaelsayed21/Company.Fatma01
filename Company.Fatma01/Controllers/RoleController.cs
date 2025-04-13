using Company.DAL.Models;
using Company.PL.Dtos;
using Company.PL.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;

namespace Company.PL.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager , UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<RoleToReturnDto> roles;
            if (string.IsNullOrEmpty(SearchInput))
            {
                   roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {
                    Id = U.Id,
                    Name = U.Name

                });
            }
            else
            {
                roles = _roleManager.Roles.Select(U => new RoleToReturnDto()
                {

                    Id = U.Id,
                    Name = U.Name


                }).Where(R => R.Name.ToLower().Contains(SearchInput.ToLower()));

            }
            return View(roles);
        }



        [HttpGet]
        public async Task<IActionResult> Create()
        {
            
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(RoleToReturnDto model)
        {
            if (ModelState.IsValid)    //server side validation
            {
               var role = await  _roleManager.FindByNameAsync(model.Name);
                if (role is null)
                {
                    role = new IdentityRole()
                    {
                        Name = model.Name

                    };
                   var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            return View(model);

        }




        [HttpGet]
        public async Task<IActionResult> Details(string? id, string ViewName = "Details")
        {

            if (id == null) return BadRequest("Invalid Id"); //400
            var role = await _roleManager.FindByIdAsync(id); //because get takes int id but details take nullable int id
            if (role is null) return NotFound(new { statusCode = 404, message = $" Role with id {id} is not found" });

            var dto = new RoleToReturnDto()
            {
                Id = role.Id,
                Name = role.Name,
               
            };

            return View(ViewName, dto);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {


            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string id, RoleToReturnDto model
            )
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Id");

                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Operation!!");

                var roleResult =await _roleManager.FindByNameAsync(model.Name);

                if(roleResult is null)
                {
                    role.Name = model.Name;

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
                ModelState.AddModelError("","Invalid Operation");
            }

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {


            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string id, RoleToReturnDto model)
        {

            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Invalid Id");

                var role = await _roleManager.FindByIdAsync(id);
                if (role is null) return BadRequest("Invalid Operation!!");


                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                
                ModelState.AddModelError("", "Invalid Operation");


            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUser(string roleId)
        {
          var role= await _roleManager.FindByIdAsync(roleId);
            if (role is null) 
                return NotFound();


            ViewData["RoleId"]=roleId;

            var usersInRole = new List<UserInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName

                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userInRole.IsSelected = true;
                }
                else
                {
                    userInRole.IsSelected = false;
                }
                usersInRole.Add(userInRole);


            }


            return View(usersInRole);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUser(string? roleId , List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();
            if(ModelState.IsValid)
            {

                foreach (var user in users)
                {
                    var AppUser = await _userManager.FindByIdAsync(user.UserId);

                    if (AppUser is not null)
                    {


                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(AppUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(AppUser, role.Name);

                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(AppUser, role.Name))

                        {
                            await _userManager.RemoveFromRoleAsync(AppUser, role.Name);

                        }


                    }

                }
                return RedirectToAction(nameof(Edit), new { id = role.Id });

            }

            return View(users);
        }

    }
}
