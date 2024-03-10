using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Common;
using OnlineShoppingApp.Common;
using OnlineShoppingApp.ConfigurationClasses;
using OnlineShoppingApp.Models;
using OnlineShoppingApp.Services.Interfaces;
using OnlineShoppingApp.ViewModels;
using System.Security.Claims;

namespace OnlineShoppingApp.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<AppUser> userManager,
                                  SignInManager<AppUser> signInManager,
                                  RoleManager<AppRole> roleManager,
                                  IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        [HttpGet]

        // Redirect user to google, making url ready
        public IActionResult GoogleLogin(string returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(GoogleLoginCallback), "Account", new { returnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }

        [HttpGet]
        public async Task<IActionResult> GoogleLoginCallback(string returnUrl = null, string remoteError = null)
        {

            if (remoteError != null)
            {
                return RedirectToAction(nameof(Login));
            }

            var result = await HttpContext.AuthenticateAsync("Google");
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            // User does not exist, so create a new user
            if (user == null)
            {
                user = new Buyer { UserName = email, Email = email, FirstName = email, LastName = email };
                var createResult = await _userManager.CreateAsync(user);

                var roleResult = await _userManager.AddToRoleAsync(user, UserType.Buyer.ToString());

                if (!createResult.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }

                ////
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmationLink = Url.Action("ConfirmEmail", "Account",

                                new { userId = user.Id, token = token }, Request.Scheme);

                /// sending confirmation mail
                EmailMessage emailMessage = new EmailMessage
                {
                    To = email,
                    Body = $"<p>Thank you for registering! To activate your account, please click the link below:</p>\r\n <a href='{confirmationLink}'>Activate</a>",
                    Subject = "Confirm your email"
                };

                _emailService.SendEmail(emailMessage);
                ///


                // Add the external login information to userlogin tale
                var addLoginResult = await _userManager.AddLoginAsync(user, new UserLoginInfo("Google", result.Principal.FindFirstValue(ClaimTypes.NameIdentifier), "Google"));

                if (!addLoginResult.Succeeded)
                {
                    return RedirectToAction(nameof(Login));
                }
            }

            if (user.EmailConfirmed)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.ErrorTitle = "Registration successful";
                ViewBag.ErrorMessage = "Before you can Login, please confirm your " +
                    "email, by clicking on the confirmation link we have emailed you";
                return View("ConfirmationError");
            }

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token) // when user click on the link
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"The User ID {userId} is invalid";
                return View("NotFound"); // did not make it yet
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View(); // no view yet
            }

            ViewBag.ErrorTitle = "Email cannot be confirmed";
            return View("Error");
        }
        [HttpGet]

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpGet]

        public IActionResult Register() // normal register
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel model, string returnUrl = null)
        {
            AppUser user = null;
            if (ModelState.IsValid)
            {
                if (model.UserType == UserType.Buyer)
                {
                    user = new Buyer
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                    };
                }
                else
                {
                    user = new Seller
                    {
                        UserName = model.Username,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                    };
                }

                var createResult = await _userManager.CreateAsync(user, model.Password);
                var roleResult = await _userManager.AddToRoleAsync(user, model.UserType == UserType.Buyer ? "Buyer" : "Seller");

                return RedirectToAction("Login", "Account");

            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserViewModel UserLoginVM) //normal login
        {
            if (ModelState.IsValid)
            {
                var userExist = await _userManager.Users.SingleOrDefaultAsync(x => x.Email == UserLoginVM.Email);

                if (userExist != null)
                {
                    bool passwordCorrect = await _userManager.CheckPasswordAsync(userExist, UserLoginVM.Password);

                    if (passwordCorrect)
                    {
                        await _signInManager.SignInAsync(userExist, UserLoginVM.RememberMe);

                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Wrong UserName Or Password");
            return View(UserLoginVM);
        }

        public async Task<IActionResult> Logout()
        {
            // if google
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // if normal login
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            ViewBag.message = null;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordVM)
        {
            var user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPasswordLink = Url.Action(nameof(ResetPassword), "Account",
                                new { email = user.Email, token = token }, Request.Scheme);

                var tokenExpiration = DateTimeOffset.Now.AddHours(1);

                /// sending  mail
                EmailMessage emailMessage = new EmailMessage
                {
                    To = forgotPasswordVM.Email,
                    Body = $"<p>To reset your password, please click the link below:</p>\r\n <a href='{forgotPasswordLink}'>Reset</a>",
                    Subject = "Reset Password"
                };

                _emailService.SendEmail(emailMessage);
                ViewBag.message = "Link sent to chnge your password, check your email";
            }
            return View(forgotPasswordVM);
        }

        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordViewModel
            {
                Email = email,
                Token = token
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                var isValidToken = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, UserManager<AppUser>.ResetPasswordTokenPurpose, model.Token);

                if (isValidToken)
                {


                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);

                    if (result.Succeeded)
                    {

                        await _signInManager.SignInAsync(user, isPersistent: false);

                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid or expired reset token.");

                }
            }

            return View(model);
        }
    }

}

