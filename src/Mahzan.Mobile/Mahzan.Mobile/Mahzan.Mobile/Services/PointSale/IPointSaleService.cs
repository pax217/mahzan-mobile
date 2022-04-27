using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.PointSale;
using Mahzan.Mobile.Models.PointSale;

namespace Mahzan.Mobile.Services.PointSale
{
    public interface IPointSaleService
    {
        Task<HttpResponseMessage> Get(GetPointSaleCommand command);

        Task<HttpResponseMessage> Create(CreatePointSaleCommand command);
        
        Task<HttpResponseMessage> Delete(string storeId);
        
        Task<HttpResponseMessage> Update(UpdatePointSaleCommand command);
    }
}