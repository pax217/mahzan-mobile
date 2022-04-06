using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Commands.Department;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.SqLite._Base;

namespace Mahzan.Mobile.Services.Department
{
    public class DepartmentService: BaseService, IDepartmentService
    {
        public DepartmentService(
            IRepository<SqLite.Entities.User> userRepository) 
            : base(userRepository)
        {
        }
        public async Task<HttpResponseMessage> Get(GetDepartmentsCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Department/Get");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["pageNumber"] = "1";
                query["pageSize"] = "10";
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


    }
}