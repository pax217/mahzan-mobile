using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Company;

namespace Mahzan.Mobile.Services.Company
{
    public interface ICompanyService
    {
        Task<HttpResponseMessage> Get(GetCompaniesCommand command);
        
        Task<HttpResponseMessage> Create(CreateCompanyCommand command);

        Task<HttpResponseMessage> Update(UpdateCompanyCommand command);

        Task<HttpResponseMessage> Delete(string companyId);
    }
}