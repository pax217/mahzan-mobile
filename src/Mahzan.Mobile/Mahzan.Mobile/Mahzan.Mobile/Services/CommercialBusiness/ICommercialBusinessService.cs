using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.CommercialBusiness;

namespace Mahzan.Mobile.Services.CommercialBusiness
{
    public interface ICommercialBusinessService
    {
        Task<HttpResponseMessage> Get(GetComercialBusinessCommand command);
    }
}