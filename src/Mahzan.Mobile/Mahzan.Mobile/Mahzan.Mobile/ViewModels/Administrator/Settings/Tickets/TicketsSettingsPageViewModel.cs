using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Company;
using Mahzan.Mobile.Models.Company;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.BlueTooth;
using Mahzan.Mobile.Services.Company;
using Mahzan.Mobile.Services.Printer;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Tickets
{
    public class TicketsSettingsPageViewModel:BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        
        private readonly IRepository<BluetoothDevice> _bluetoothDeviceRepository;

        private readonly ICompanyService _companyService;
        
        private readonly IBlueToothService _blueToothService;
        
        private Printer _printer;
        
        public string PrintMessage { get; set; }
        public ICommand PrintTicketCommand { get; set; }
        
        public TicketsSettingsPageViewModel(
            INavigationService navigationService,
            ICompanyService companyService)
        {
            _navigationService = navigationService;
            _blueToothService = Xamarin.Forms.DependencyService.Get<IBlueToothService>();
            _printer = new Printer(_blueToothService);
            
            _companyService = companyService;
            
            PrintMessage = "";

            PrintTicketCommand = new Command(async () => await OnPrintTicketCommand());
        }

        private async Task OnPrintTicketCommand()
        {
            var httpResponseMessage = await _companyService.Get(new GetCompaniesCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "OnPrintTicketCommand", errorApi.Message, "ok");

                return;  
            }
            var getCompaniesResponse = JsonConvert.DeserializeObject<GetCompaniesResponse>(respuesta);
            
            var logoBytes = Convert.FromBase64String(getCompaniesResponse.Data.FirstOrDefault().Logo);



            _printer.MyPrinter = "MTP-2";
            
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
            await _printer.Reset();

        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }


    }
}