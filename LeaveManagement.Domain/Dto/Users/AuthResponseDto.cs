using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement.Domain.Dto.Users
{
    public class AuthResponseDto
    {
        public string UserId {get;set;}
        public string Token {get;set;}
    }
}