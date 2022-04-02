using System.Threading.Tasks;
using Mahzan.Mobile.Models.User;
using System.Net.Http;

namespace Mahzan.Mobile.Services.User
{
    public interface IUserService
    {
        Task<HttpResponseMessage> LogIn(string userName,string password);
    }
}