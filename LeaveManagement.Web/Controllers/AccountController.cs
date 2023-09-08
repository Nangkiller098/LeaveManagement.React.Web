using LeaveManagement.Application.Contracts;
using LeaveManagement.Domain.Dto.Users;
using LeaveManagement.Domain.Model.Users;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Web.Controllers
{
    
    public class AccountController : BaseApiController
    {
        private readonly IAuthManager _authManager;

        public AccountController(IAuthManager authManager)
        {
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