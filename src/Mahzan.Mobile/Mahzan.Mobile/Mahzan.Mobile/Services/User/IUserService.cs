using System.Threading.Tasks;
using Mahzan.Mobile.Models.User;
using System.Net.Http;
using Mahzan.Mobile.Commands.Company;
using Mahzan.Mobile.Commands.User;

namespace Mahzan.Mobile.Services.User
{
    public interface IUserService
    {
        Task<HttpResponseMessage> LogIn(string userName,string password);

        Task<HttpResponseMessage> Create(CreateUserCommand command);

        Task<HttpResponseMessage> ResetPassword(ResetPasswordCommand command);

        Task<HttpResponseMessage> ChangePassword(ChangePasswordCommand command);
    }
}