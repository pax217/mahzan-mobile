using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Services.BlueTooth;
using Mahzan.Mobile.Services.Printer;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Printers
{
    public class SelectPrinterPageViewModel : BindableBase
    {
        private readonly IBlueToothService _blueToothService;

        private readonly IPageDialogService _dialogService;

        private readonly IRepository<BluetoothDevice> _bluetoothDeviceRepository;
        
        private Printer _printer;

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
            _printer = new Printer(_blueToothService);

            //Comands
            PrintCommand = new Command(async () => await OnPrintCommand());

            Task.Run(() => BindDeviceList());
        }

        private async Task OnPrintCommand()
        {

            if (SelectedDevice==null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Impresora", "Debes seleccionar un dispositivo bluethood", "ok");
                
                return;
            }
            else 
            {
                await _bluetoothDeviceRepository.DeleteAll();

                //Inserta en SQL configuración
                await _bluetoothDeviceRepository
                    .Insert(new BluetoothDevice
                    {
                        DeviceName = SelectedDevice
                    });

                _printer.MyPrinter = SelectedDevice;
                PrintMessage = "Mahzan";
                
                await _printer.Reset();
                await _printer.SetAlignCenter();
                await _printer.WriteLine($"Normal: {PrintMessage}");
                await _printer.LineFeed();
                await _printer.BoldOn();
                await _printer.WriteLine($"Bold: {PrintMessage}");
                await _printer.BoldOff();
                await _printer.LineFeed();
                await _printer.WriteLine_Big($"Grande: \n{PrintMessage}");
                await _printer.LineFeed();
                await _printer.SetUnderLine($"Subrayado: {PrintMessage}");
                await _printer.LineFeed();
                await _printer.SetAlignRight();
                await _printer.WriteLine_Bold("Negrito en la Derecha:");
                await _printer.BoldOn();
                await _printer.WriteLine_Bigger($"G1: {PrintMessage}",1);
                await _printer.BoldOff();
                await _printer.WriteLine_Bold("Underline...");
                await _printer.SetUnderLineOn();
                await _printer.WriteLine_Bigger($"G2: {PrintMessage}", 2);
                await _printer.WriteLine_Bigger($"G3: {PrintMessage}", 3);
                await _printer.SetUnderLineOff();
                await _printer.LineFeed(2);
                await _printer.SetAlignCenter();
                await _printer.WriteLine_Bold("Texto Reverse...");
                await _printer.SetReverseOn();
                await _printer.WriteLine_Bigger($"Reverse: {PrintMessage}", 1);
                await _printer.SetReverseOff();
                await _printer.WriteLine_Bigger($"Not Reverse: {PrintMessage}", 1);
                await _printer.LineFeed(3);
                await _printer.WriteLine( DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss tt"));
                await _printer.Reset();
            }


        }

        /// <summary>
        /// Get Bluetooth device list with DependencyService
        /// </summary>
        async Task BindDeviceList()
        {
            //Busca si el dispositivo fue previamnete configurado

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