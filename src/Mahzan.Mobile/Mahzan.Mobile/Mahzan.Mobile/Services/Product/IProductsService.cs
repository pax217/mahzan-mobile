using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Product;

namespace Mahzan.Mobile.Services.Product
{
    public interface IProductsService
    {
        Task<HttpResponseMessage> Create(CreateProductCommand command);

        Task<HttpResponseMessage> Get(GetProductsCommand command);

        Task<HttpResponseMessage> Delete(string productId);
    }
}