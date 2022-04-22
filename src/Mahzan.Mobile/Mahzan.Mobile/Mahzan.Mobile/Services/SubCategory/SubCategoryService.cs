using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Commands.SubCategory;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.SqLite._Base;
using Newtonsoft.Json;

namespace Mahzan.Mobile.Services.SubCategory
{
    public class SubCategoryService: BaseService, ISubCategoryService
    {
        public SubCategoryService(IRepository<SqLite.Entities.User> userRepository) : base(userRepository)
        {
        }

        public async Task<HttpResponseMessage> Get(GetSubCategoriesCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/SubCategory/Get");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["pageNumber"] = "1";
                query["pageSize"] = "10";

                if (command.SubCategoryId!=null)
                {
                    query["subCategoryId"] = command.SubCategoryId.ToString();
                }

                if (command.CategoryId!=null)
                {
                    query["categoryId"] = command.CategoryId.ToString();
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

        public async Task<HttpResponseMessage> Delete(string subCategoryId)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/SubCategory/Delete");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["subCategoryId"] = subCategoryId;

                uriBuilder.Query = query.ToString();

                HttpClient httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                httpResponseMessage = await httpClient.DeleteAsync(uriBuilder.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return httpResponseMessage;
        }

        public async Task<HttpResponseMessage> Create(CreateSubCategoryCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/SubCategory/Create");
            
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

        public async Task<HttpResponseMessage> Update(UpdateSubCategoryCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/SubCategory/Update");
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
    }
}