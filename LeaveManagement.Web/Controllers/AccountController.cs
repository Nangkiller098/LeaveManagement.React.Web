using LeaveManagement.Application.Contracts;
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
            var authResponse = await _authManager.Login(loginDto);
            if(authResponse==null)
            {
                return Unauthorized();
            }
            return Ok(authResponse);
        }
    }
}