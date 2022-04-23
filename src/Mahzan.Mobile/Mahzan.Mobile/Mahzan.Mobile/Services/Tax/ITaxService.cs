using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Tax;

namespace Mahzan.Mobile.Services.Tax
{
    public interface ITaxService
    {
        Task<HttpResponseMessage> Create(CreateTaxCommand command);
        
        Task<HttpResponseMessage> Get(GetTaxCommand command);
        
        Task<HttpResponseMessage> Delete(string taxId);
        
        Task<HttpResponseMessage> Update(UpdateTaxCommand command);
    }
    
}