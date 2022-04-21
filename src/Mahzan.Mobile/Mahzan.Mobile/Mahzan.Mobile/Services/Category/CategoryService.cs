using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Commands.Category;
using Mahzan.Mobile.Models.Category;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.SqLite._Base;
using Newtonsoft.Json;

namespace Mahzan.Mobile.Services.Category
{
    public class CategoryService : BaseService, ICategoryService
    {
        public CategoryService(
            IRepository<SqLite.Entities.User> userRepository) : base(userRepository)
        {
        }

        public async Task<HttpResponseMessage> Get(GetCategoriesCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Category/Get");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["pageNumber"] = "1";
                query["pageSize"] = "10";

                if (command.CategoryId!=null)
                {
                    query["categoryId"] = command.CategoryId.ToString();
                }
                
                if (command.DepartmentId!=null)
                {
                    query["departmentId"] = command.DepartmentId.ToString();
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

        public async Task<HttpResponseMessage> Create(CreateCategoryCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Category/Create");
            
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

        public async Task<HttpResponseMessage> Update(UpdateCategoryCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Category/Update");
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

        public async Task<HttpResponseMessage> Delete(string categoryId)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Category/Delete");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["categoryId"] = categoryId;

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
    }
}