

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
        private const string _loginProvider ="LeavemanagementWeb";
        private const string _refreshToken ="RefreshToken";
        public AuthManager(IMapper mapper,UserManager<ApiUser> userManager,IConfiguration configuration )
        {
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
            
        }
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
                _user = await _userManager.FindByEmailAsync(loginDto.Email);
                bool isvalidUser = await _userManager.CheckPasswordAsync(_user,loginDto.Password);
                if(_user is null || isvalidUser==false)
                {
                    return null;
                }
                var token= await GenerateToken();
                return new AuthResponseDto
                {
                    Token = token,
                    UserId= _user.Id,
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



        //generate token when login or register users
        private async Task<string>GenerateToken()
        {
            //get super key 
            var securitykey= new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            //check sign in credentials and hash it
            var credentails= new SigningCredentials(securitykey,SecurityAlgorithms.HmacSha256);
            //get role of user
            var role = await _userManager.GetRolesAsync(_user);
            //claims role from user
            var roleClamins=role.Select(x => new Claim(ClaimTypes.Role,x)).ToList();
            //claims users 
            var userClaims = await _userManager.GetClaimsAsync(_user);
            

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub,_user.Email),
                new(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email,_user.Email),
                new("uid",_user.Id),
            }.Union(userClaims).Union(roleClamins);

            //assign data to JwtSecurityToken
            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience:_configuration["JwtSettings:Audience"],
                claims:claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentails
                
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user,_loginProvider,_refreshToken);
            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user,_loginProvider,_refreshToken);
            var restule = await _userManager.SetAuthenticationTokenAsync(_user,_loginProvider,_refreshToken,newRefreshToken);
            return newRefreshToken;
        }
        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {
            var JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var tokenContent = JwtSecurityTokenHandler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q=>q.Type==JwtRegisteredClaimNames.Email)?.Value;
            _user = await _userManager.FindByEmailAsync(username);
            if(_user ==null)
            {
                return null;
            }
            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider,_refreshToken,request.RefreshToken);
        }
    }
}