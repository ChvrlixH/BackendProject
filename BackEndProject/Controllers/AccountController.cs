﻿using BackEndProject.Utils;
using BackEndProject.Utils.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BackEndProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();

            if (!ModelState.IsValid) { return View(); }

            AppUser newUser = new()
            {
                Fullname = registerVM.Fullname,
                Email = registerVM.Email,
                UserName = registerVM.Username,
                IsActive = true,
            };

            var identityResult = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _userManager.AddToRoleAsync(newUser, RoleType.Member.ToString());

            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (User.Identity.IsAuthenticated)
                return BadRequest();

            if (!ModelState.IsValid) { return View(); }

            var user = await _userManager.FindByNameAsync(loginVM.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Username or password invalid");
                return View();
            }

            if(!user.IsActive)
            {
                ModelState.AddModelError("", "User is blocked");
                return View();
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, true);
            if (signInResult.IsLockedOut)
            {
                ModelState.AddModelError("", "Your account is blocked temporary");
                return View();
            }
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Username or password invalid");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction(nameof(Login));
            }

            return BadRequest();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (!ModelState.IsValid) { return View(); }

            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if (user is null) { ModelState.AddModelError("Email", $"User not found by email: {forgotPasswordVM.Email}"); return View(); }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            string url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token }, HttpContext.Request.Scheme);

            EmailHelper emailHelper = new EmailHelper();

            string body = await GetEmailTemplateAsync(url);

            MailRequestVM mailRequestVM = new()
            {
                ToEmail = user.Email,
                Subject = "Reset your password",
                Body = body
            };

            await emailHelper.SendEmailAsync(mailRequestVM);

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPasswordVM)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordVM.UserId) || string.IsNullOrWhiteSpace(resetPasswordVM.Token)) { return BadRequest(); }

            var user = await _userManager.FindByIdAsync(resetPasswordVM.UserId);
            if (user is null) { return NotFound(); }

            ViewBag.UserName = user.UserName;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ChangePasswordVM changePasswordVM, string? userId, string? token)
        {
            if (!ModelState.IsValid) { return View(); }

            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) { return BadRequest(); }

            var user = await _userManager.FindByIdAsync(userId);
            if (user is null) { return NotFound(); }

            var identityResult = await _userManager.ResetPasswordAsync(user, token, changePasswordVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (var error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> CreateRoles()
        {

            foreach (var roleType in Enum.GetValues(typeof(RoleType)))
            {
                if (!await _roleManager.RoleExistsAsync(roleType.ToString()))
                    await _roleManager.CreateAsync(new IdentityRole { Name = roleType.ToString() });
            }
            //await _rolemanager.createasync(new identityrole { name = roletype.admin.tostring() });
            //await _rolemanager.createasync(new identityrole { name = roletype.moderator.tostring() });
            //await _rolemanager.createasync(new identityrole { name = roletype.member.tostring() });

            return Json("Ok");
        }

        public async Task<string> GetEmailTemplateAsync(string url)
        {
            string folder = @"admin\templates\images\";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, folder + "\\", "template.html");

            using StreamReader streamReader = new StreamReader(path);
            string result = await streamReader.ReadToEndAsync();
            result = result.Replace("[reset_password_url]", url);
            streamReader.Close();
            return result;
        }

    }
}
