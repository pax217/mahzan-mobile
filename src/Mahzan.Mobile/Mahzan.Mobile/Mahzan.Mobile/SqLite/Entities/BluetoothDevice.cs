using SQLite;

namespace Mahzan.Mobile.SqLite.Entities
{
    public class BluetoothDevice
    {
        [PrimaryKey]
        public string DeviceName { get; set; }
    }
}