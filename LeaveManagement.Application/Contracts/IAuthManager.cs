using LeaveManagement.Domain.Model.Users;
using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.Application.Contracts
{
    public interface IAuthManager
    {
        Task<IEnumerable<IdentityError>>Registers(ApiUserDto userDto);
        Task<bool> Login(LoginDto loginDto);

    }

}