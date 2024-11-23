using CourseBusinessWebsite.Payloads.RequestData.TokenRequest;
using CourseBusinessWebsite.Payloads.RequestData.UserRequest;
using CourseBusinessWebsite.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CourseBusinessWebsite.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _iAuthService;
        public AuthController(IAuthService iAuthService)
        {
            _iAuthService = iAuthService;
        }
        [HttpPost("/api/auth/Register")]
        public async Task<IActionResult> Register([FromBody] RequestRegister request)
        {
            var result = await _iAuthService.Register(request);
            if (result.Status == 404)
            {
                return NotFound(result);
            }
            else if (result.Status == 400)
            {
                return BadRequest(result);
            }
            return Ok(request);
        }
        [HttpPost("/api/auth/Login")]
        public async Task<IActionResult> Login([FromBody] RequestLogin request)
        {
            var result = await _iAuthService.Login(request);
            if (result.Status == 404)
            {
                return NotFound(result);
            }
            else if (result.Status == 400)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("/api/auth/ConfirmCreateNewAccount")]
        public async Task<IActionResult> ConfirmCreateNewAccount([FromBody] RequestConfirmCreateAccount request)
        {
            return Ok(await _iAuthService.ConfirmCreateNewAccount(request));
        }
        [HttpPost("/api/auth/RenewAccessToken")]
        public IActionResult RenewAccessToken([FromBody] RequestRenewToken request)
        {
            return Ok(_iAuthService.RenewAccessToken(request));
        }
        [HttpPut("/api/auth/ChangePassword")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ChangePassword(RequestChangePassword request)
        {
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await _iAuthService.ChangePassword(id, request));
        }
        [HttpPut("/api/auth/UpdateUserInformation")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateUserInformation(RequestUpdateUserInformation request)
        {
            int id = int.Parse(HttpContext.User.FindFirst("Id").Value);
            return Ok(await _iAuthService.UpdateUserInformation(id, request));
        }
        [HttpPut("/api/auth/ChangeDecentralization")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeDecentralization(RequestChangeDecentralization request)
        {
            return Ok(await _iAuthService.ChangeDecentralization(request));
        }
        [HttpPut("/api/auth/ForgotPassword")]
        public async Task<IActionResult> ForgotPassword(RequestForgotPassword request)
        {
            return Ok(await _iAuthService.ForgotPassword(request));
        }
        [HttpPut("/api/auth/ConfirmCreateNewPassword")]
        public async Task<IActionResult> ConfirmCreateNewPassword(RequestConfirmCreateNewPassword request)
        {
            return Ok(await _iAuthService.ConfirmCreateNewPassword(request));
        }

        [HttpGet("/api/auth/GetAllUsers")]
        public async Task<IActionResult> GetAllUsers([FromQuery] RequestUserInput input, int pageSize = 10, int pageNumber = 1)
        {
            return Ok(await _iAuthService.GetAllUsers(input, pageSize, pageNumber));
        }
        [HttpPost("/api/auth/GetUserByName")]
        public async Task<IActionResult> GetUserByName(string name, RequestPaginationUserInput input)
        {
            return Ok(await _iAuthService.GetUserByName(name, input.PageSize, input.PageNumber));
        }
    }
}
