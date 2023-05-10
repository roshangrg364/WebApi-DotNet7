using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using System.Diagnostics;
using Villa_MVC_Core_Module.Dto;
using Villa_MVC_Core_Module.Service;
using Villa_Web_App.Extensions;
using Villa_Web_App.Models;

namespace Villa_Web_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly VillaServiceInterface _villaService;
        private readonly IToastNotification _toastNotification;

        public HomeController(VillaServiceInterface villaService,
            IToastNotification toastNotification)
        {
            _villaService = villaService;
            _toastNotification = toastNotification;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _villaService.GetAllVillas(this.GetToken());
                if (response == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                if (response.IsSuccess)
                {
                    var model = new List<VillaResponseDto>();
                    if (response.Data != null)
                    {
                        model = JsonConvert.DeserializeObject<List<VillaResponseDto>>(response.Data.ToString());

                    }
                    return View(model);
                }
                var error = string.Join(",", response.Errors);
                _toastNotification.AddInfoToastMessage(error);

                return RedirectToAction("Error", "Home");

            }
            catch (Exception ex)
            {

                return RedirectToAction("Error", "Home");

            }

        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}