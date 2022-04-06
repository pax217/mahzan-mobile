using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ImTools;
using Mahzan.Mobile.Commands.Product;
using Mahzan.Mobile.Services._Base;
using Newtonsoft.Json;

namespace Mahzan.Mobile.Services.Product
{
    public class ProductsService :BaseService,IProductsService
    {
        public async Task<HttpResponseMessage> Create(CreateProductCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/Product/Create");
            try
            {
                HttpClient httpClient = new HttpClient();

                string jsonData = JsonConvert.SerializeObject(command);
                StringContent stringContent = new StringContent(jsonData, UnicodeEncoding.UTF8, "application/json");

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