using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using System.Security.Claims;
using Villa_MVC_Core_Module.Dto;
using Villa_MVC_Core_Module.Service;
using Villa_Web_App.Models;

namespace Villa_Web_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountServiceInterface _accountService;
        private readonly IToastNotification _toastNotification;
        public AccountController(AccountServiceInterface accountService,
            IToastNotification toastNotification)
        {
            _accountService = accountService;
            _toastNotification = toastNotification;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var response = await _accountService.Login(new LoginDto
                {
                    Username = model.Username,
                    Password = model.Password
                });
                if (response == null)
                {
                    _toastNotification.AddErrorToastMessage("Error while login.");
                    return View(model);
                }

                if (!response.IsSuccess)
                {
                    _toastNotification.AddErrorToastMessage(string.Join("</br>", response.Errors.ToList()));
                    return View(model);
                }
                var responseModel = JsonConvert.DeserializeObject<LoginResponseModel>(response.Data.ToString());
                var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, responseModel.User.Id.ToString()),
            new Claim(ClaimTypes.Role, responseModel.User.Role),
            new Claim("Token", responseModel.Token)
        };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
                    IsPersistent = model.RememberMe
                };

                await HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               principal,
               authProperties);
                _toastNotification.AddSuccessToastMessage("logged in successfully");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View(model);
            }

        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel model)

        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }
                var response = await _accountService.Create(new UserCreateDto
                {
                    UserName = model.UserName,
                    Password = model.Password,
                    Name = model.Name
                });
                if (response == null)
                {
                    _toastNotification.AddErrorToastMessage("error while registering user");
                    return View(model);
                }

                if (!response.IsSuccess)
                {
                    _toastNotification.AddErrorToastMessage(string.Join("</br>", response.Errors.ToList()));
                    return View(model);
                }
                return RedirectToAction(nameof(Login));
            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View(model);

            }

        }


        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(
        CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
