using CoreModule.Src;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VillaApi.ApiResponseModel;

namespace VillaApi.Controllers
{

    [Route("api/v{version:apiVersion}/villaApi")]
    [ApiController]
    [ApiVersion("1.0")]
    public class VillaApiController : ControllerBase
    {
        private readonly VillaServiceInterface _villaService;
        public VillaApiController(VillaServiceInterface villaService)
        {
            _villaService = villaService;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetVilla()
        {
            try
            {
                var Villas = await _villaService.GetAllVillas();
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = Villas });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });
            }


        }

        [Authorize]
        [ResponseCache(CacheProfileName = "Default30")]
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var villa = await _villaService.GetById(id).ConfigureAwait(true);
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = villa });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] VillaCreateDto dto)
        {
            try
            {
                var villa = await _villaService.Create(dto).ConfigureAwait(true);
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = villa });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }

        [Authorize]
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] VillaUpdateDto dto)
        {
            try
            {
                dto.Id = id;
                var villa = await _villaService.Update(dto).ConfigureAwait(true);
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = villa });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }
        [Authorize]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _villaService.Delete(id).ConfigureAwait(true);
                return Ok(new ResponseModel { Status = HttpStatusCode.NoContent, IsSuccess = true });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }
    }
}
