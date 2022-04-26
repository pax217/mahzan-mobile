using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.UnitSale;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.UnitSale;
using Mahzan.Mobile.Services.UnitsSale;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using ZXing.OneD.RSS;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.UnitsSale
{
    public class AdminUnitSalePageViewModel: BindableBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitSaleService _unitSaleService;
        
        public Guid UnitSaleId { get; set; }
        
        private string _abbreviation;
        public string Abbreviation
        {
            get { return _abbreviation; }
            set
            {
                _abbreviation = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Abbreviation)));
            }
        }
        
        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        public ICommand DeleteUnitSaleCommand { get; set; }
        
        public ICommand SaveUnitSaleCommand { get; set; }

        public AdminUnitSalePageViewModel(
            INavigationService navigationService, 
            IUnitSaleService unitSaleService)
        {
            _navigationService = navigationService;
            _unitSaleService = unitSaleService;
            
            SaveUnitSaleCommand = new Command(async () => await OnSaveUnitSaleCommand());
            
            DeleteUnitSaleCommand = new Command(async () => await OnDeleteUnitSaleCommand());

        }

        private async Task OnSaveUnitSaleCommand()
        {
            if (UnitSaleId==Guid.Empty)
            {
                await CreateUnitSale();
            }
            else
            {
                await UpdateUnitSale();
            }
        }
        
        private async Task CreateUnitSale()
        {
            var httpResponseMessage = await _unitSaleService.Create(new CreateUnitSaleCommand()
            {
                Abbreviation = Abbreviation,
                Description = Description
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("CreateUnitSale", errorApi.Message, "Ok");
                
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Creación Unidad de Venta", 
                    $"Se ha creado correctamente la unidad de venta {Description}", 
                    "Ok");
            
                await _navigationService.GoBackAsync();   
            }
        }
        
        private async Task UpdateUnitSale()
        {
            var httpResponseMessage = await _unitSaleService.Update(new UpdateUnitSaleCommand
            {
                UnitSaleId = UnitSaleId,
                Abbreviation = Abbreviation,
                Description = Description,
            });
                
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("UpdateUnitSale", errorApi.Message, "Ok");
            }
                
            await App.Current.MainPage.DisplayAlert(
                "Actualización Uniad de Venta", 
                $"Se ha actualizado correctamenta la unidad de venta {Description}", 
                "Ok");

            await _navigationService.GoBackAsync();
        }

        private async Task OnDeleteUnitSaleCommand()
        {
            await DeleteUnitSale();
        }

        private async Task DeleteUnitSale()
        {
            var answer = await Application
                .Current
                .MainPage
                .DisplayAlert("Atención!",
                    "¿Estas seguro de borrar la unidad de venta?", "Si", "No");
            
            if (answer)
            {
                var httpResponseMessage = await _unitSaleService.Delete(UnitSaleId.ToString());
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert(
                        "DeleteUnitSale", 
                        errorApi.Message, 
                        "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Elimina impuesto", 
                        $"Se ha elimindo correctamente la unidad de venta {Description}", 
                        "Ok");

                    await _navigationService.GoBackAsync();
                }
            }
        }
        
        public async Task GetUnitSale(Guid unitSaleId)
        {
            var httpResponseMessageStores = await _unitSaleService.Get(new GetUnitsSaleCommand()
            {
                UnitSaleId = unitSaleId
            });
           
            var respuesta = await httpResponseMessageStores.Content.ReadAsStringAsync();
          
            if (httpResponseMessageStores.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetUnitSale", errorApi.Message, "Ok");
            }
           
            var getUnitsSaleResponse = JsonConvert.DeserializeObject<GetUnitsSaleResponse>(respuesta);

            if (getUnitsSaleResponse != null)
            {
                Abbreviation = getUnitsSaleResponse.Data.FirstOrDefault()?.Abbreviation;
                Description = getUnitsSaleResponse.Data.FirstOrDefault()?.Description;
            }
        }
        
        public async void OnNavigatedFrom(INavigationParameters parameters)
        {
 
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            UnitSaleId = parameters.GetValue<Guid>("unitSaleId");
            if (UnitSaleId!=Guid.Empty)
            {
                await GetUnitSale(UnitSaleId);
            }
        }
    }
}