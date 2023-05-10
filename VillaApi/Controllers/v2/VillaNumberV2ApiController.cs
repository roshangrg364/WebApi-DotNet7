using CoreModule.Src;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VillaApi.ApiResponseModel;

namespace VillaApi.Controllers
{
    [Authorize]
    [Route("api/v{version:apiVersion}/VillaNumberApi")]
    [ApiController]
    [ApiVersion("2.0")]
    public class VillaNumberV2ApiController : ControllerBase
    {

        private readonly VillaNumberServiceInterface _villaNumberService;
        public VillaNumberV2ApiController(VillaNumberServiceInterface villaService)
        {
            _villaNumberService = villaService;
        }
        [MapToApiVersion("2.0")]
        [HttpGet]
        public async Task<IActionResult> GetVillaNumberV2()
        {
            try
            {
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = new List<string>() { "a", "b" } });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });
            }


        }
      
    }
}
