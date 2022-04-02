using System.Threading.Tasks;

namespace Mahzan.Mobile.Services.SHA1
{
    public interface ISHA1
    {
        Task<string> EncryptString(string text, string keyString);

        Task<string> DecryptString(string encryptText, string keyString);
    }
}