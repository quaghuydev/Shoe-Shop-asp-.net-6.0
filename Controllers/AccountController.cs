using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ShoeShop.Dto;
using ShoeShop.Interfaces;
using ShoeShop.Models;
using ShoeShop.Models.ViewModel;

namespace ShoeShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IPhotoService photoService;
        private readonly IEmailService emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IPhotoService photoService, IEmailService emailService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.photoService = photoService;
            this.emailService = emailService;
        }
        public IActionResult Login()
        {
            LoginDto loginDto = new LoginDto();
            return View(loginDto);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }
            var user = await userManager.FindByEmailAsync(login.Email);
            if (user != null)
            {
                var passwordCheck = await userManager.CheckPasswordAsync(user, login.Password);
                if (passwordCheck)
                {
                    var result = await signInManager.PasswordSignInAsync(user, login.Password, false, false);
                    if (result.Succeeded)
                    {
                        await signInManager.SignInAsync(user, false);
                        return RedirectToAction("Index", "Product");
                    }
                }
                TempData["Error"] = "Wrong credential. Please try again";
                return View(login);
            }
            TempData["Error"] = "Wrong credential. Please try again";
            return View(login);
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto register)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(register.Email);
                if (user != null)
                {
                    TempData["Error"] = "Email đã tổn tại";
                    return View(register);
                }
                var photoResult = await photoService.AddPhoto(register.avatar);
                var newUser = new User
                {
                    Email = register.Email,
                    UserName = register.Fullname,
                    PhoneNumber = register.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    AvatarUrl = photoResult.Url.ToString(),
                    EmailConfirmed = true,
                    UserRoles = UserRoles.User,
                    CreateDate = DateTime.Now
                };
                var newUserResponse = await userManager.CreateAsync(newUser, register.Password);
                if (newUserResponse.Succeeded)
                {
                    await userManager.AddToRoleAsync(newUser, UserRoles.User);
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["Error"] = "username đã tồn tại ";
                    return View(register);
                }


            }
            else
            {
                TempData["Error"] = "Register is failed";
                return View(register);
            }
        }
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ViewData["Error"] = "Email không tồn tại";
                    // User with this email doesn't exist
                    return View();
                }

                // Generate verification code. Could also be more complex like a GUID
                var verificationCode = new Random().Next(100000, 999999).ToString();

                // Save this verification code in TempData
                TempData["VerificationCode"] = verificationCode;

                var messageBody = $"Your verification code is {verificationCode}";

                // Send email
                await emailService.SendEmailAsync(model.Email, user.UserName, "Reset Your Password", messageBody);

                // Redirect to ResetPassword view
                return RedirectToAction(nameof(ResetPassword), new { email = model.Email });
            }

            // If we got this far, something failed, re-display form
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(string email)
        {
            var model = new ResetPasswordViewModel { Email = email };
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ViewData["Error"] = "Email không tồn tại";
                // Don't reveal that the user does not exist
                return View();
            }

            if (!model.VerificationCode.Equals(TempData["VerificationCode"]))
            {
                ModelState.AddModelError("", "Verification Không đúng.");
                return View(model);
            }

            // Reset password
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var result = await userManager.ResetPasswordAsync(user, token, model.Password);
            if (result.Succeeded)
            {
                await userManager.AddPasswordAsync(user, model.Password);
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }


}

