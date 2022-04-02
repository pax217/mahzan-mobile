using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Models.User;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.Services.SHA1;

namespace Mahzan.Mobile.Services.User
{
    public class UserService : BaseService,IUserService
    {
        private readonly ISHA1 _sha1;
        
        public UserService(ISHA1 sha1)
        {
            _sha1 = sha1;
        }

        public async Task<HttpResponseMessage> LogIn(string userName, string password)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/User/LogIn");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["userName"] = userName;
                query["passwordEncrypted"] = await EncryptPassword(password);
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

        private async Task<string> EncryptPassword(string password)
        {
            return await _sha1.EncryptString(password,"E546C8DF278XZ5931069B522E695D4A2");
        }
    }
}