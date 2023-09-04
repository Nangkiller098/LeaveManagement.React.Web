using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeaveManagement.Application.Contracts;
using LeaveManagement.Domain.Model;
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
            }
            return Ok(apiUserDto);
        }
    }
}