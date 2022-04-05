using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Services.BlueTooth;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace Mahzan.Mobile.ViewModels.Settings.Printer
{
    public class SelectPrinterPageViewModel : BindableBase
    {
        private readonly IBlueToothService _blueToothService;

        private readonly IPageDialogService _dialogService;

        private readonly IRepository<BluetoothDevice> _bluetoothDeviceRepository;

        private IList<string> _deviceList;
        public IList<string> DeviceList
        {
            get
            {
                if (_deviceList == null)
                    _deviceList = new ObservableCollection<string>();
                return _deviceList;
            }
            set
            {
                _deviceList = value;

            }
        }

        private string _printMessage;
        public string PrintMessage
        {
            get
            {
                return _printMessage;
            }
            set
            {
                _printMessage = value;
            }
        }

        private string _selectedDevice;
        public string SelectedDevice
        {
            get
            {
                return _selectedDevice;
            }
            set
            {
                _selectedDevice = value;

            }
        }



        //Commands
        public ICommand PrintCommand { get; set; }

        public SelectPrinterPageViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            IRepository<BluetoothDevice> bluetoothDeviceRespository)
        {
            //Services
            _blueToothService = Xamarin.Forms.DependencyService.Get<IBlueToothService>();
            _dialogService = pageDialogService;
            _bluetoothDeviceRepository = bluetoothDeviceRespository;

            //Comands
            PrintCommand = new Command(async () => await OnPrintCommand());

            Task.Run(() => BindDeviceList());
        }

        private async Task OnPrintCommand()
        {
            await _bluetoothDeviceRepository.DeleteAll();

            //Inserta en SQL configuración
            await _bluetoothDeviceRepository
                  .Insert(new BluetoothDevice
                         {
                            DeviceName = SelectedDevice
                         });

            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("--------------------------------\n");
            stringBuilder.Append("              Mahzan            \n");
            stringBuilder.Append("--------------------------------\n");
            stringBuilder.Append("******Impresion de prueba*******\n");
            stringBuilder.Append("--------------------------------\n");
            stringBuilder.Append("   " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt" + "   \n"));
            stringBuilder.Append("--------------------------------\n");

            if (SelectedDevice==null)
            {
                await _dialogService
                      .DisplayAlertAsync("Dispositivos Bluetooth",
                                         "Debes seleccionar un dispositivo",
                                         "Ok");
                return;
            }
            else 
            {
                await _blueToothService.Print(SelectedDevice, stringBuilder.ToString());
            }


        }

        /// <summary>
        /// Get Bluetooth device list with DependencyService
        /// </summary>
        async Task BindDeviceList()
        {
            //Busca si el dispositivo fue previamnete configurato

            List<BluetoothDevice> bluetoothDevice = await _bluetoothDeviceRepository
                                                    .Get();

            if (bluetoothDevice.Any())
            {
                SelectedDevice = bluetoothDevice.FirstOrDefault().DeviceName;
            }
            

            var list = _blueToothService.GetDeviceList();
            DeviceList.Clear();
            foreach (var item in list)
                DeviceList.Add(item);
        }


    }
}