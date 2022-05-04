using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
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


    }
}