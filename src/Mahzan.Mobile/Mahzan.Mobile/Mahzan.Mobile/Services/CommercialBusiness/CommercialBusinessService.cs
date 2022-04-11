using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Mahzan.Mobile.Commands.CommercialBusiness;
using Mahzan.Mobile.Services._Base;
using Mahzan.Mobile.SqLite._Base;
using ZXing.Aztec.Internal;

namespace Mahzan.Mobile.Services.CommercialBusiness
{
    public class CommercialBusinessService:BaseService, ICommercialBusinessService
    {
        public CommercialBusinessService(IRepository<SqLite.Entities.User> userRepository) : base(userRepository)
        {
        }
        
        public async Task<HttpResponseMessage> Get(GetComercialBusinessCommand command)
        {
            HttpResponseMessage httpResponseMessage;
            UriBuilder uriBuilder = new UriBuilder(UrlApi + "/v1/CommercialsBusiness/Get");
            try
            {
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["pageNumber"] = "1";
                query["pageSize"] = "10";

                if (command.CommercialBusinessId!=null)
                {
                    query["commercialBusinessId"] = command.CommercialBusinessId;
                }
                
                if (command.Key!=null)
                {
                    query["key"] = command.Key;
                }
                
                if (command.Description!=null)
                {
                    query["description"] = command.Description;
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