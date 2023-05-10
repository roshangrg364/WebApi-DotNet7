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
    [ApiVersion("1.0")]
    [ApiController]
    public class VillaNumberApiController : ControllerBase
    {

        private readonly VillaNumberServiceInterface _villaNumberService;
        public VillaNumberApiController(VillaNumberServiceInterface villaService)
        {
            _villaNumberService = villaService;
        }
        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVillaNumber()
        {
            try
            {
                var villaNumbers = await _villaNumberService.GetAllVillas();
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = villaNumbers });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });
            }


        }


        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var villa = await _villaNumberService.GetById(id).ConfigureAwait(true);
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = villa });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] VillaNumberCreateDto dto)
        {
            try
            {
                var villaNumber = await _villaNumberService.Create(dto).ConfigureAwait(true);
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = villaNumber });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }


        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] VillaNumberUpdateDto dto)
        {
            try
            {
               
                dto.Id = id;
                var villaNumber = await _villaNumberService.Update(dto).ConfigureAwait(true);
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = villaNumber });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _villaNumberService.Delete(id).ConfigureAwait(true);
                return Ok(new ResponseModel { Status = HttpStatusCode.NoContent, IsSuccess = true });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }
    }
}
