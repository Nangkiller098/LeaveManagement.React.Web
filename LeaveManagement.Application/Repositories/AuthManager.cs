

using System.Text;
using AutoMapper;
using LeaveManagement.Application.Contracts;
using LeaveManagement.Domain.Model;
using LeaveManagement.Domain.Model.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace LeaveManagement.Application.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthManager(IMapper mapper,UserManager<ApiUser> userManager,IConfiguration configuration )
        {
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
            
        }

        public async Task<bool> Login(LoginDto loginDto)
        {
            bool isValidUser=false;
            try
            {
                var _user = await _userManager.FindByEmailAsync(loginDto.Email);
                if(_user is null)
                {
                    return default;
                }
               isValidUser=await _userManager.CheckPasswordAsync(_user,loginDto.Password);
               if(!isValidUser)
               {
                return default;
               }
               isValidUser=true;
                
            }
            catch (Exception)
            {
                  
            }
            return isValidUser;
        }

        public async Task<IEnumerable<IdentityError>> Registers(ApiUserDto userDto)
        {
            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName=userDto.Email;
            var result = await _userManager.CreateAsync(user,userDto.Password);
            if(result.Succeeded){
                await _userManager.AddToRoleAsync(user,"User");
            }
            return result.Errors;
        }
        // private async Task<string>GenerateToken(ApiUser user)
        // {
        //     var securitykey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        //     var credentails= new SigningCredentials(securitykey,Security)
        // }
    }
}