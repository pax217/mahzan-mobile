using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Company;

namespace Mahzan.Mobile.Services.Company
{
    public interface ICompanyService
    {
        Task<HttpResponseMessage> Get(GetCompaniesCommand command);
    }
}