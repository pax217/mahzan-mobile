using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Commands.User;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.Services.SHA1;
using Mahzan.Mobile.SqLite._Base;
using Newtonsoft.Json;

namespace Mahzan.Mobile.Services.User
{
    public class UserService : BaseService,IUserService
    {
        private readonly ISHA1 _sha1;
        
        public UserService(
            IRepository<SqLite.Entities.User> userRepository,
            ISHA1 sha1
        ) : base(userRepository)
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

        public async Task<HttpResponseMessage> Create(CreateUserCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/User/SignUp");
            try
            {
                HttpClient httpClient = new HttpClient();

                command.PasswordEncrypted = await EncryptPassword(command.Password);

                string jsonData = JsonConvert.SerializeObject(command);
                StringContent stringContent = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                httpResponseMessage = await httpClient.PostAsync(uriBuilder.ToString(), stringContent);
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