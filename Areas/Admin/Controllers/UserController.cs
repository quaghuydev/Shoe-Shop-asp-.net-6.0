using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Dto;
using ShoeShop.Interfaces;
using ShoeShop.Models;
using ShoeShop.Models.ViewModel;

namespace ShoeShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ("admin"))]
    public class UserController : Controller
    {
        private readonly IUserRepository repository;
        private readonly IPhotoService photoService;
        private readonly UserManager<User> userManager;

        public UserController(IUserRepository repository, IPhotoService photoService, UserManager<User> userManager)
        {
            this.repository = repository;
            this.photoService = photoService;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<User> users = await repository.getUsers();
            return View(users);
        }
        public async Task<IActionResult> Create()
        {
            UserDto userDto = new UserDto();
            return View(userDto);
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(userDto.Email);
                if (user != null)
                {
                    TempData["Error"] = "Email đã tổn tại";
                    return View(userDto);
                }
                var photoResult = await photoService.AddPhoto(userDto.avatar);
                var newUser = new User
                {
                    Email = userDto.Email,
                    UserName = userDto.Username,
                    PhoneNumber = userDto.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    AvatarUrl = photoResult.Url.ToString(),
                    EmailConfirmed = true,
                    UserRoles = userDto.Role,
                    CreateDate = DateTime.Now
                };
                var newUserResponse = await userManager.CreateAsync(newUser, userDto.Password);
                if (newUserResponse.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, userDto.Role == "admin" ? UserRoles.Admin : UserRoles.User);
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "username đã tồn tại ";
                    return View(userDto);
                }


            }
            else
            {
                TempData["Error"] = "Tạo thất bại. Kiểm tra lại mật khẩu or username";
                return View(userDto);
            }
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (!ModelState.IsValid)
            {
                return View("Error");
            }
            var user = await repository.getUserById(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }
            var userDto = new UserViewModel
            {
                Id = id,
                Username = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,

            };
            return View(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, UserViewModel userDto)
        {
            if (ModelState.IsValid)
            {

                var user = await repository.getUserByIdNoTracking(id);
                if (user != null)
                {

                    user.UserName = userDto.Username;
                    user.Email = userDto.Email;
                    user.PhoneNumber = userDto.PhoneNumber;
                    if (userDto.avatar != null)
                    {
                        var photoResult = await photoService.AddPhoto(userDto.avatar);
                        user.AvatarUrl = photoResult.Url.ToString();
                    }
                    repository.Update(user);
                    return RedirectToAction("Index");

                }
            }
            return View();
        }

        public async Task<IActionResult> UpdateStatus(string id)
        {
            var user = await repository.getUserByIdNoTracking(id);
            if (user.UserStatus == Models.Enums.UserStatus.Active)
            {
                user.UserStatus = Models.Enums.UserStatus.Lock;
            }
            else
            {
                user.UserStatus = Models.Enums.UserStatus.Active;
            }
            repository.Update(user);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            if (await repository.Delete(id)) return Ok();
            return BadRequest();
        }
    }
}
