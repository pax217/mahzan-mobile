using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.Bluetooth;
using Java.Util;
using Mahzan.Mobile.Droid;
using Mahzan.Mobile.Services.BlueTooth;
using System.Text;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(AndroidBlueToothService))]
namespace Mahzan.Mobile.Droid
{
    public class AndroidBlueToothService: IBlueToothService
    {
        /// <summary>
        /// We have to use local device Bluetooth adapter.
        /// BondedDevices returns BluetoothDevice collection anyway I need to take just device name.
        /// </summary>
        /// <returns></returns>
        public IList<string> GetDeviceList()
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                var btdevice = bluetoothAdapter?.BondedDevices.Select(i => i.Name).ToList();
                return btdevice;
            }
        }

        /// <summary>
        /// We have to use local device Bluetooth adapter.
        /// We need to find Bluetooth Device with selected device name.
        /// Now, we use BluetoothSocket class with most common UUID
        /// Try to connect BluetoothSocket then convert your text-message to bytearray
        /// Last step write your bytearray by way of bluetoothSocket
        /// </summary>
        /// <param name="deviceName">Selected deviceName</param>
        /// <param name="text">My printed text-message</param>
        /// <returns></returns>
        public async Task Print(string deviceName, byte[] buffer)
        {
            using (BluetoothAdapter bluetoothAdapter = BluetoothAdapter.DefaultAdapter)
            {
                BluetoothDevice device = (from bd in bluetoothAdapter?.BondedDevices
                                          where bd?.Name == deviceName
                                          select bd).FirstOrDefault();
                try
                {
                    using (BluetoothSocket bluetoothSocket = device?.
                        CreateRfcommSocketToServiceRecord(
                        UUID.FromString("00001101-0000-1000-8000-00805f9b34fb")))
                    {
                        bluetoothSocket?.Connect();
                        //byte[] buffer = Encoding.UTF8.GetBytes(text);
                        bluetoothSocket?.OutputStream.Write(buffer, 0, buffer.Length);
                        bluetoothSocket.Close();
                    }
                }
                catch (Exception exp)
                {
                    throw exp;
                }
            }
        }
    }
}