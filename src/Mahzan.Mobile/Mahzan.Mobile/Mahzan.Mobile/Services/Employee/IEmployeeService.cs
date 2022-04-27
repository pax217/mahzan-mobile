using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Employee;

namespace Mahzan.Mobile.Services.Employee
{
    public interface IEmployeeService
    {
        Task<HttpResponseMessage> Get(GetEmployeeCommand command);

        Task<HttpResponseMessage> Create(CreateEmployeeCommand command);
        
        Task<HttpResponseMessage> Delete(string employeeId);
        
        Task<HttpResponseMessage> Update(UpdateEmployeeCommand command);
    }
}