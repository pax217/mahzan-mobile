using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Models.PointSaleState;

namespace Mahzan.Mobile.Services.PointSaleState
{
    public interface IPointSaleStateService
    {
        Task<HttpResponseMessage> Create(CreatePointSaleStateCommand command);
    }
}