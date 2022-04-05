using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mahzan.Mobile.Services.BlueTooth
{
    public interface IBlueToothService
    {
        IList<string> GetDeviceList();
        Task Print(string deviceName, string text);
    }
}