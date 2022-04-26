using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.UnitSale;

namespace Mahzan.Mobile.Services.UnitsSale
{
    public interface IUnitSaleService
    {
        Task<HttpResponseMessage> Get(GetUnitsSaleCommand command);
        Task<HttpResponseMessage> Delete(string unitSaleId);
        Task<HttpResponseMessage> Create(CreateUnitSaleCommand command);

        Task<HttpResponseMessage> Update(UpdateUnitSaleCommand command);
    }
}