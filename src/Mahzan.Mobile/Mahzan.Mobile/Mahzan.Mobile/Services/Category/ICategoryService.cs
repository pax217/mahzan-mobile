using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Category;
using Mahzan.Mobile.Models.Category;

namespace Mahzan.Mobile.Services.Category
{
    public interface ICategoryService
    {
        Task<HttpResponseMessage> Get(GetCategoriesCommand command);

        Task<HttpResponseMessage> Create(CreateCategoryCommand command);
        
        Task<HttpResponseMessage> Update(UpdateCategoryCommand command);
        
        Task<HttpResponseMessage> Delete(string categoryId);
    }
}