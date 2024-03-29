using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Commands.Employee;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.SqLite._Base;
using Newtonsoft.Json;

namespace Mahzan.Mobile.Services.Employee
{
    public class EmployeeService: BaseService, IEmployeeService 
    {
        public EmployeeService(
            IRepository<SqLite.Entities.User> userRepository) : base(userRepository)
        {
        }

        public async Task<HttpResponseMessage> Get(GetEmployeeCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Employee/Get");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["pageNumber"] = "1";
                query["pageSize"] = "10";

                if (command.EmployeeId!=null)
                {
                    query["employeeId"] = command.EmployeeId.ToString();
                }
                
                uriBuilder.Query = query.ToString();

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                httpResponseMessage = await httpClient.GetAsync(uriBuilder.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return httpResponseMessage;
        }

        public async Task<HttpResponseMessage> Create(CreateEmployeeCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Employee/Create");
            try
            {
                HttpClient httpClient = new HttpClient();

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

        public Task<HttpResponseMessage> Delete(string employeeId)
        {
            throw new System.NotImplementedException();
        }

        public Task<HttpResponseMessage> Update(UpdateEmployeeCommand command)
        {
            throw new System.NotImplementedException();
        }
    }
}