using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Department;

namespace Mahzan.Mobile.Services.Department
{
    public interface IDepartmentService
    {
        Task<HttpResponseMessage> Get(GetDepartmentsCommand command);

        Task<HttpResponseMessage> Create(CreateDepartmentCommand command);

        Task<HttpResponseMessage> Delete(string departmentId);

        Task<HttpResponseMessage> Update(UpdateDepartmentCommand command);
    }
}