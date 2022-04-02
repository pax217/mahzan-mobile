using System.Net.Http;
using System.Threading.Tasks;
using Mahzan.Mobile.Models.User;

namespace Mahzan.Mobile.Services.User
{
    public interface IUserService
    {
        Task<HttpResponseMessage> LogIn(string userName,string passwordEncrypted);
    }
}