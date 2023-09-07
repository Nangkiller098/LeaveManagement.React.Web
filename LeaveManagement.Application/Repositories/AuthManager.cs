

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
        private ApiUser _user;
        private const string _loginProvider ="LeaveManagementReact";
        private const string _refreshToken="RefreshToken";
        public AuthManager(IMapper mapper,UserManager<ApiUser> userManager,IConfiguration configuration )
        {
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
            
        }



        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
                _user = await _userManager.FindByEmailAsync(loginDto.Email);
               bool isValidUser= await _userManager.CheckPasswordAsync(_user,loginDto.Password);
               if(_user == null || isValidUser==false)
               {
                return null;
               }
               var token = await GenerateToken();
               return new AuthResponseDto
               {
                Token=token,
                UserId=_user.Id,
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



        private async Task<string>GenerateToken()
        {
            var securitykey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"])); //get key jwt
            var credentails= new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256); //check credentials
            var roles = await _userManager.GetRolesAsync(_user);
            var roleClaims = roles.Select(x=> new Claim(ClaimTypes.Role,x)).ToList();
            var userClaims = await _userManager.GetClaimsAsync(_user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,_user.Email), //person who hold the key
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()), //prevent playback operations or playback attack
                new Claim(JwtRegisteredClaimNames.Email,_user.Email), //prevent playback operations or playback attack
                new Claim("uid",_user.Id), //prevent playback operations or playback attack

            }.Union(userClaims).Union(roleClaims);;
            var token = new JwtSecurityToken(
                issuer:_configuration["JwtSettings:Issuer"],
                audience:_configuration["JwtSettings:Audience"],
                claims:claims,
                expires:DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationsInMinutes"])),
                signingCredentials:credentails
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> CreateRefreshToken()
        {
         await _userManager.RemoveAuthenticationTokenAsync(_user,_loginProvider,_refreshToken);
         var newFreshToken= await _userManager.GenerateUserTokenAsync(_user,_loginProvider,_refreshToken);
         var result = await _userManager.SetAuthenticationTokenAsync(_user,_loginProvider,_refreshToken,newFreshToken);
            return newFreshToken;
        }
        public Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            throw new NotImplementedException();
        }
    }
}