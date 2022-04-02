using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Models.User;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.SqLite._Base;
using Newtonsoft.Json;

namespace Mahzan.Mobile.Services.User
{
    public class UserService:BaseService,IUserService
    {
        public UserService(IRepository<SqLite.Entities.User> userRepository) : base(userRepository)
        {
        }

        public async Task<HttpResponseMessage> LogIn(string userName, string passwordEncrypted)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/User/LogIn");
            
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["userName"] = userName;
                query["passwordEncrypted"] = passwordEncrypted;
                uriBuilder.Query = query.ToString();
                
                HttpClient httpClient = new HttpClient();
                httpResponseMessage = await httpClient.GetAsync(uriBuilder.ToString());
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return httpResponseMessage;
        }
    }
}