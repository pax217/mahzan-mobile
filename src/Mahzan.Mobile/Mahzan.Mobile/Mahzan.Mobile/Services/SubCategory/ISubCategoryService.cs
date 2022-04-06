using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.SubCategory;

namespace Mahzan.Mobile.Services.SubCategory
{
    public interface ISubCategoryService
    {
        Task<HttpResponseMessage> Get(GetSubCategoriesCommand command);
    }
}