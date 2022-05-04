using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Commands.PointSaleState;
using Mahzan.Mobile.Models.PointSaleState;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.SqLite._Base;
using Newtonsoft.Json;
using ZXing.Aztec.Internal;

namespace Mahzan.Mobile.Services.PointSaleState
{
    public class PointSaleStateService : BaseService,IPointSaleStateService
    {
        public PointSaleStateService(
            IRepository<SqLite.Entities.User> userRepository) 
            : base(userRepository)
        {
        }
        
        public async Task<HttpResponseMessage> Create(CreatePointSaleStateCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/PointSaleState/Open");
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

        public async Task<HttpResponseMessage> Get(GetPointSaleStateCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/PointSaleSTATE/Get");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["pageNumber"] = "1";
                query["pageSize"] = "10";

                if (command.PointSaleId!=null)
                {
                    query["PointSaleId"] = command.PointSaleId.ToString();
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
    }
}