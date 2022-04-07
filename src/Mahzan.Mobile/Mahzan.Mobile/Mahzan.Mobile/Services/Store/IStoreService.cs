using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Store;

namespace Mahzan.Mobile.Services.Store
{
    public interface IStoreService
    {
        Task<HttpResponseMessage> Get(GetStoreCommand command);
    }
}