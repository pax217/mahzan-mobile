using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Store;
using Mahzan.Mobile.Models.Store;

namespace Mahzan.Mobile.Services.Store
{
    public interface IStoreService
    {
        Task<HttpResponseMessage> Get(GetStoreCommand command);

        Task<HttpResponseMessage> Create(CreateStoreCommand command);
    }
}