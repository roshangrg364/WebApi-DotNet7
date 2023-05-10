using CoreModule.Src;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using VillaApi.ApiResponseModel;

namespace VillaApi.Controllers
{
    [Route("api/v{version:apiVersion}/AccountApi")]
    [ApiController]
    [ApiVersionNeutral]
    public class AccountApiController : ControllerBase
    {
        private readonly UserServiceInterface _userService;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        public AccountApiController(
            UserServiceInterface userService, IConfiguration configuration,
            UserManager<User> userManager)
        {
            _userService = userService;
            _configuration = configuration;
            _userManager = userManager;
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
                var tokenHandler = new JwtSecurityTokenHandler();
                var secretKey = _configuration.GetValue<string>("JwtConfig:Key");
                var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new Claim(ClaimTypes.Role,(await _userManager.GetRolesAsync(user)).FirstOrDefault())
                    };
                var signingCredential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
                var tokenDescriptor = new SecurityTokenDescriptor()
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = signingCredential

                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var returnModel = new LoginResponseDto()
                {
                    Token = tokenHandler.WriteToken(token),
                    User = new UserResponseDto() { 
                        Id = user.Id,
                        Name = user.FullName,
                        UserName = user.UserName,
                        Role =(await _userManager.GetRolesAsync(user).ConfigureAwait(true)).FirstOrDefault()
                    }
                    
                };

                return Ok(new ResponseModel { Status = HttpStatusCode.OK, IsSuccess = true, Data = returnModel });
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
