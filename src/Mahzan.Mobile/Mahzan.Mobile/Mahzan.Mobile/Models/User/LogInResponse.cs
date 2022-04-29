using System;
using System.Security.Principal;

namespace Mahzan.Mobile.Models.User
{
    public class LogInResponse
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
        
        public string Role { get; set; }
        
        public string UserName { get; set; }
    }
}