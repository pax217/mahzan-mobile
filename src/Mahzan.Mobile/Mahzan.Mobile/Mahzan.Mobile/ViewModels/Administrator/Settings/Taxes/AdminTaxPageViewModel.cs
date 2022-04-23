using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Category;
using Mahzan.Mobile.Commands.Tax;
using Mahzan.Mobile.Models.Category;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.Tax;
using Mahzan.Mobile.Services.Tax;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Taxes
{
    public class AdminTaxPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly ITaxService _taxService;

        public Guid TaxId { get; set; }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));
            }
        }
        
        private decimal? _percentage;
        public decimal? Percentage
        {
            get { return _percentage; }
            set
            {
                _percentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Percentage)));
            }
        }
        
        public ICommand DeleteTaxCommand { get; set; }
        
        public ICommand SaveTaxCommand { get; set; }
        
        public AdminTaxPageViewModel(
            INavigationService navigationService, 
            ITaxService taxService)
        {
            _navigationService = navigationService;
            _taxService = taxService;
            
            SaveTaxCommand = new Command(async () => await OnSaveTaxCommand());
            
            DeleteTaxCommand = new Command(async () => await OnDeleteTaxCommand());

        }
        
        private async Task OnSaveTaxCommand()
        {
            if (TaxId==Guid.Empty)
            {
                await CreateTax();
            }
            else
            {
                await UpdateTax();
            }
        }
        
        private async Task CreateTax()
        {
            var httpResponseMessage = await _taxService.Create(new CreateTaxCommand
            {
                Name = Name,
                Percentage = Percentage
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("CreateTax", errorApi.Message, "Ok");
                
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Creación de Impuesto", 
                    $"Se ha creado correctamente el Impuesto {Name}", 
                    "Ok");
            
                await _navigationService.GoBackAsync();   
            }
        }
        
        private async Task UpdateTax()
        {
            var httpResponseMessage = await _taxService.Update(new UpdateTaxCommand
            {
                TaxId = TaxId,
                Name = Name,
                Percentage = Percentage,
            });
                
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("UpdateTax", errorApi.Message, "Ok");
            }
                
            await App.Current.MainPage.DisplayAlert(
                "Actualización de Impuesto", 
                $"Se ha actualizado correctamenta el impuesto {Name}", 
                "Ok");

            await _navigationService.GoBackAsync();
        }

        private async Task OnDeleteTaxCommand()
        {
            await DeleteTax();
        }
        
        private async Task DeleteTax()
        {
            var answer = await Application
                .Current
                .MainPage
                .DisplayAlert("Atención!",
                    "¿Estas seguro de borrar el impuesto?", "Si", "No");
            
            if (answer)
            {
                var httpResponseMessage = await _taxService.Delete(TaxId.ToString());
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert(
                        "DeleteTax", 
                        errorApi.Message, 
                        "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Elimina impuesto", 
                        $"Se ha elimindo correctamente el impuesto {Name}", 
                        "Ok");

                    await _navigationService.GoBackAsync();
                }
            }
        }
        
        public async Task GetTax(Guid taxId)
        {
            var httpResponseMessageStores = await _taxService.Get(new GetTaxCommand()
            {
                TaxId = taxId
            });
           
            var respuesta = await httpResponseMessageStores.Content.ReadAsStringAsync();
          
            if (httpResponseMessageStores.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetCategory", errorApi.Message, "Ok");
            }
           
            var getTaxesResponse = JsonConvert.DeserializeObject<GetTaxesResponse>(respuesta);

            if (getTaxesResponse != null)
            {
                Name = getTaxesResponse.Data.FirstOrDefault()?.Name;
                Percentage = getTaxesResponse.Data.FirstOrDefault()?.Percentage;
            }
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            TaxId = parameters.GetValue<Guid>("taxId");
            if (TaxId!=Guid.Empty)
            {
                await GetTax(TaxId);
            }
        }
    }
}