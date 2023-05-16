using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using System.Reflection;
using Villa_MVC_Core_Module.Dto;
using Villa_MVC_Core_Module.Service;
using Villa_Web_App.Extensions;

namespace VillaNumber_Web_App.Controllers
{
    [Authorize]
    public class VillaNumberController : Controller
    {
        private readonly VillaNumberServiceInterface _VillaNumberService;
        private readonly VillaServiceInterface _villaService;
        private readonly IToastNotification _toastNotification;

        public VillaNumberController(VillaNumberServiceInterface VillaNumberService,
            VillaServiceInterface villaService,
            IToastNotification toastNotification)
        {
            _VillaNumberService = VillaNumberService;
            _toastNotification = toastNotification;
            _villaService = villaService;
        }
        public async Task<IActionResult> Index()
        {
            try
            {
                var response = await _VillaNumberService.GetAllVillaNumbers(this.GetToken());
                if (response == null)
                {
                    _toastNotification.AddErrorToastMessage("Error while login.");
                    return RedirectToAction("Error", "Home");
                }
                if (response.IsSuccess)
                {
                    var model = new List<VillaNumberResponseDto>();
                    if (response.Data != null)
                    {
                        model = JsonConvert.DeserializeObject<List<VillaNumberResponseDto>>(response.Data.ToString());

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

        public async Task<IActionResult> Create()
        {
            ViewBag.Villas = new List<VillaResponseDto>();
            var response = await _villaService.GetAllVillas();
            if (response == null)
            {
                _toastNotification.AddErrorToastMessage("Error while login.");
                return RedirectToAction("Error", "Home");
            }
            if (response.IsSuccess)
            {
                var villas = new List<VillaResponseDto>();
                if (response.Data != null)
                {
                    ViewBag.Villas = JsonConvert.DeserializeObject<List<VillaResponseDto>>(response.Data.ToString());

                }
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VillaNumberCreateDto dto)
        {
            try
            {
                ViewBag.Villas = new List<VillaResponseDto>();
                var villasResponse = await _villaService.GetAllVillas();
                if (villasResponse == null)
                {
                    _toastNotification.AddErrorToastMessage("Error while login.");
                    return RedirectToAction("Error", "Home");
                }
                if (villasResponse.IsSuccess)
                {
                    var villas = new List<VillaResponseDto>();
                    if (villasResponse.Data != null)
                    {
                        ViewBag.Villas = JsonConvert.DeserializeObject<List<VillaResponseDto>>(villasResponse.Data.ToString());

                    }
                }
                if (!ModelState.IsValid)
                {
                    _toastNotification.AddErrorToastMessage("Invalid Model State");
                    return RedirectToAction("Index", "VillaNumber");
                }
                var response = await _VillaNumberService.Create(dto, this.GetToken());
                if (response.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage("Created successfully");
                    return RedirectToAction("Index", "VillaNumber");
                }
                _toastNotification.AddErrorToastMessage("Error..please contact to admin");
                return View(dto);

            }
            catch (Exception ex)
            {
                _toastNotification.AddErrorToastMessage(ex.Message);
                return View(dto);
            }


        }


        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Villas = new List<VillaResponseDto>();
            var villasResponse = await _villaService.GetAllVillas();
            if (villasResponse == null)
            {
                _toastNotification.AddErrorToastMessage("Error while login.");
                return RedirectToAction("Error", "Home");
            }
            if (villasResponse.IsSuccess)
            {
                var villas = new List<VillaResponseDto>();
                if (villasResponse.Data != null)
                {
                    ViewBag.Villas = JsonConvert.DeserializeObject<List<VillaResponseDto>>(villasResponse.Data.ToString());

                }
            }
            var response = await _VillaNumberService.GetById(id, this.GetToken()).ConfigureAwait(true);
            if (response.IsSuccess && response.Data != null)
            {
                var VillaNumberUpdateDto = JsonConvert.DeserializeObject<VillaNumberUpdateDto>(response.Data.ToString());
                return View(VillaNumberUpdateDto);
            }
            var error = string.Join(",", response.Errors);
            _toastNotification.AddInfoToastMessage(error);
            return RedirectToAction("Index", "VillaNumber");

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(VillaNumberUpdateDto dto)
        {
            try
            {
                ViewBag.Villas = new List<VillaResponseDto>();
                var villasResponse = await _villaService.GetAllVillas();
                if (villasResponse == null)
                {
                    _toastNotification.AddErrorToastMessage("Error while login.");
                    return RedirectToAction("Error", "Home");
                }
                if (villasResponse.IsSuccess)
                {
                    var villas = new List<VillaResponseDto>();
                    if (villasResponse.Data != null)
                    {
                        ViewBag.Villas = JsonConvert.DeserializeObject<List<VillaResponseDto>>(villasResponse.Data.ToString());

                    }
                }
                if (!ModelState.IsValid)
                {
                    _toastNotification.AddErrorToastMessage("Invalid Model State");
                    return RedirectToAction("Index", "VillaNumber");
                }
                var response = await _VillaNumberService.Update(dto, this.GetToken());
                if (response.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage("Updated successfully");
                    return RedirectToAction("Index", "VillaNumber");
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



        public async Task<IActionResult> Delete(int id)
        {
            try
            {

                var response = await _VillaNumberService.Delete(id, this.GetToken());
                if (response == null)
                {
                    _toastNotification.AddErrorToastMessage("Error while login.");
                    return RedirectToAction("Error", "Home");
                }
                if (response.IsSuccess)
                {
                    _toastNotification.AddSuccessToastMessage("Deleted successfully");
                    return RedirectToAction("Index", "VillaNumber");
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
