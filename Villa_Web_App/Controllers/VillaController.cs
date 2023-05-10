using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using System.Reflection;
using Villa_MVC_Core_Module.Dto;
using Villa_MVC_Core_Module.Service;
using Villa_Web_App.Extensions;

namespace Villa_Web_App.Controllers
{
   
    public class VillaController : Controller
    {
        private readonly VillaServiceInterface _villaService;
        private readonly IToastNotification _toastNotification;

        public VillaController(VillaServiceInterface villaService,
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
                    _toastNotification.AddErrorToastMessage("Error while login.");
                    return RedirectToAction("Error", "Home");
                }
                if (response.IsSuccess )
                {
                    var model = new List<VillaResponseDto>();
                    if(response.Data !=null)
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
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VillaCreateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _toastNotification.AddErrorToastMessage("Invalid Model State");
                    return RedirectToAction("Index", "Villa");
                }
                var response = await _villaService.Create(dto, this.GetToken());
                if (response == null)
                {
                    _toastNotification.AddErrorToastMessage("Error while login.");
                    return RedirectToAction("Error", "Home");
                }
                if (response.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage("Created successfully");
                    return RedirectToAction("Index", "Villa");
                }
                _toastNotification.AddErrorToastMessage("Error..please contact to admin");
                return RedirectToAction("Error", "Home");

            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return RedirectToAction("Error", "Home");

            }


        }

        [Authorize]
        public async Task<IActionResult> Update(int id)
        {
            var response =await _villaService.GetById(id, this.GetToken()).ConfigureAwait(true);
            if (response == null)
            {
                _toastNotification.AddErrorToastMessage("Error while login.");
                return RedirectToAction("Error", "Home");
            }
            if (response.IsSuccess && response.Data !=null)
            {
                var villaUpdateDto  = JsonConvert.DeserializeObject<VillaUpdateDto>(response.Data.ToString());
                return View(villaUpdateDto);
            }
            var error = string.Join(",", response.Errors);
            _toastNotification.AddInfoToastMessage(error);
            return RedirectToAction("Index", "Villa");

        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VillaUpdateDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _toastNotification.AddErrorToastMessage("Invalid Model State");
                    return RedirectToAction("Index", "Villa");
                }
                var response = await _villaService.Update(dto, this.GetToken());
                if (response == null)
                {
                    _toastNotification.AddErrorToastMessage("Error while login.");
                    return RedirectToAction("Error", "Home");
                }
                if (response.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage("Updated successfully");
                    return RedirectToAction("Index", "Villa");
                }
                var error = string.Join(",", response.Errors);
                _toastNotification.AddInfoToastMessage(error);

                return RedirectToAction("Error", "Home");

            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return RedirectToAction("Error", "Home");

            }


        }


        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
              
                var response = await _villaService.Delete(id, this.GetToken());
                if (response == null)
                {
                    _toastNotification.AddErrorToastMessage("Error while login.");
                    return RedirectToAction("Error", "Home");
                }
                if (response.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage("Deleted successfully");
                    return RedirectToAction("Index", "Villa");
                }
                var error = string.Join(",", response.Errors);
                _toastNotification.AddInfoToastMessage(error);

                return RedirectToAction("Error", "Home");

            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return RedirectToAction("Error", "Home");

            }


        }


    }
}
