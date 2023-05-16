using CoreModule.Src;
using CoreModule.Src.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using VillaApi.ApiResponseModel;

namespace VillaApi.Controllers
{
    [AllowAnonymous]
    [Route("api/v{version:apiVersion}/AccountApi")]
    [ApiController]
    [ApiVersionNeutral]
    public class AccountApiController : ControllerBase
    {
        private readonly UserServiceInterface _userService;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly TokenServiceInterface _tokenService;
        public AccountApiController(
            UserServiceInterface userService, IConfiguration configuration,
            UserManager<User> userManager,
            TokenServiceInterface tokenService)
        {
            _userService = userService;
            _configuration = configuration;
            _userManager = userManager;
            _tokenService = tokenService;
        }
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            try
            {

                var user = await _userService.GetByuserName(dto.Username);
                if (user == null) throw new CustomException("Username/password do not match");
                var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, dto.Password);
                if (!isPasswordCorrect) throw new CustomException("UserName/Password do not match");


                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,(await _userManager.GetRolesAsync(user)).FirstOrDefault())
                    };

                var jwtToken = _tokenService.GenerateAccessToken(claims);
                var refreshToken = await _userService.UpdateRefreshToken(user.Id);


                var returnModel = new LoginResponseDto()
                {
                    Token = jwtToken,
                    RefreshToken = refreshToken,
                    User = new UserResponseDto()
                    {
                        Id = user.Id,
                        Name = user.FullName,
                        UserName = user.UserName,
                        Role = (await _userManager.GetRolesAsync(user).ConfigureAwait(true)).FirstOrDefault()
                    }

                };

                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = returnModel });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }


        [HttpPost("Refresh-Token")]
        public async Task<IActionResult> RefreshToken(TokenDto tokenModel)
        {
            try
            {
                if (tokenModel is null)
                {
                    return BadRequest("Invalid client request");
                }

                string? accessToken = tokenModel.AccessToken;
                string? refreshToken = tokenModel.RefreshToken;

                var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    throw new CustomException("Invalid token");
                }
                string userId = principal.Claims.FirstOrDefault(a=>a.Type == ClaimTypes.NameIdentifier).Value;
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null || user.RefreshToken != refreshToken)
                {
                    throw new CustomException("Invalid Access Token");
                }

                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList());
                var newRefreshToken = await _userService.UpdateRefreshToken(userId);

                var returnModel = new TokenDto()
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                };
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data=returnModel });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }

        }


        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Register([FromBody] UserCreateDto dto)
        {
            try
            {
                var userResponse = await _userService.Create(dto);
                if (userResponse == null) throw new CustomException("Error while creating user");
                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = userResponse });
            }
            catch (Exception ex)
            {

                return BadRequest(new ResponseModel { Status = HttpStatusCode.BadRequest, IsSuccess = false, Errors = new List<string> { ex.Message } });

            }
        }
    }
}
