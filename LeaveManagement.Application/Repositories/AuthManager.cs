

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using LeaveManagement.Application.Contracts;
using LeaveManagement.Domain.Dto.Users;
using LeaveManagement.Domain.Model;
using LeaveManagement.Domain.Model.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


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

        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
               bool isValidUser= await _userManager.CheckPasswordAsync(user,loginDto.Password);
               if(user == null || isValidUser==false)
               {
                return null;
               }
               var token = await GenerateToken(user);
               return new AuthResponseDto
               {
                Token=token,
                UserId=user.Id,
               };

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
        private async Task<string>GenerateToken(ApiUser user)
        {
            var securitykey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])); //get key jwt
            var credentails= new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256); //check credentials
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(x=> new Claim(ClaimTypes.Role,x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Email), //person who hold the key
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), //prevent playback operations or playback attack
                new Claim(JwtRegisteredClaimNames.Email,user.Email), //prevent playback operations or playback attack
                new Claim("uid",user.Id), //prevent playback operations or playback attack

            }.Union(userClaims).Union(roleClaims);;
            var token = new JwtSecurityToken(
                issuer:_configuration["JwtSettings:Issuer"],
                audience:_configuration["JwtSettings:audience"],
                claims:claims,
                expires:DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationsInMinus"])),
                signingCredentials:credentails
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}