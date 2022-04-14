using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Commands.Company;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.SqLite._Base;
using Newtonsoft.Json;

namespace Mahzan.Mobile.Services.Company
{
    public class CompanyService: BaseService, ICompanyService
    {
        public CompanyService(IRepository<SqLite.Entities.User> userRepository) : base(userRepository)
        {
        }

        public async Task<HttpResponseMessage> Get(GetCompaniesCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Company/Get");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["pageNumber"] = "1";
                query["pageSize"] = "10";

                if (command.CompanyId!=null)
                {
                    query["companyId"] = command.CompanyId;
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

        public async Task<HttpResponseMessage> Create(CreateCompanyCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Company/Create");
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

        public async Task<HttpResponseMessage> Update(UpdateCompanyCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Company/Update");
            try
            {
                HttpClient httpClient = new HttpClient();

                string jsonData = JsonConvert.SerializeObject(command);
                StringContent stringContent = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                httpResponseMessage = await httpClient.PutAsync(uriBuilder.ToString(), stringContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            return httpResponseMessage;
        }

        public async Task<HttpResponseMessage> Delete(string companyId)
        {
            throw new NotImplementedException();
        }
    }
}