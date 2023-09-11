using LeaveManagement.Application.Contracts;
using LeaveManagement.Domain.Dto.Users;
using LeaveManagement.Domain.Model.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Web.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAuthManager authManager,ILogger<AccountController> logger)
        {
            _logger      = logger;
            _authManager = authManager;         
        }
        //POST: api/Account/register
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto apiUserDto)
        {
            _logger.LogInformation($"Registration Attempt for",apiUserDto.Email);
            try
            {
                var errors = await _authManager.Registers(apiUserDto);
                if(errors.Any())
                {
                    foreach(var error in errors)    
                    {
                        ModelState.AddModelError(error.Code,error.Description);
                    }            
                    return BadRequest(ModelState);
                }
                return Ok(apiUserDto); 
                }
            catch (Exception ex)
            {
                _logger.LogError(ex,$"Something Went Wrong in the {nameof(Register)} - User registration attemtp for {apiUserDto.Email}");
                return Problem($"Something went Wrong in the {nameof(Register)}. Please Contact Support.",statusCode :500);
            }

        }
        //Post: api/Account/login
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] LoginDto loginDto)
        {

                var authResponseDto = await _authManager.Login(loginDto);
                if(authResponseDto == null)
                {
                    return Unauthorized();
                }
                return Ok(authResponseDto);

        }
        //Post: api/Account/refreshToken
        [HttpPost]
        [Route("refreshToken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            var authResponseDto = await _authManager.VerifyRefreshToken(request);
            if(authResponseDto == null)
            {
                return Unauthorized();
            }
            return Ok(authResponseDto);
        }

    }
}