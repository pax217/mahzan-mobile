using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mahzan.Mobile.SqLite._Base;

namespace Mahzan.Mobile.Services._Base
{
    public class BaseService
    {
        //public static string UrlApi = "https://localhost:7236";

        public static string UrlApi = "https://mahzan-api-development-ryuer.ondigitalocean.app";
        public static string Token { get; set; }

        private readonly IRepository<SqLite.Entities.User> _userRepository;
        public BaseService(
            IRepository<SqLite.Entities.User> userRepository)
        {
            _userRepository = userRepository;
            
            Task.Run(GetToken);
        }
        
        private async Task GetToken() 
        {
            List<SqLite.Entities.User> aspNetUsers;
            aspNetUsers = await _userRepository.Get();

            Token = aspNetUsers.FirstOrDefault()?.Token;
        }
    }
}