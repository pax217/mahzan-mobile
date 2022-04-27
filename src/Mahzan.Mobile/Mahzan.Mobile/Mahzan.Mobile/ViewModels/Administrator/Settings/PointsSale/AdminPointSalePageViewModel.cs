using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Department;
using Mahzan.Mobile.Commands.PointSale;
using Mahzan.Mobile.Commands.Store;
using Mahzan.Mobile.Models.Department;
using Mahzan.Mobile.Models.PointSale;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.Store;
using Mahzan.Mobile.Services.PointSale;
using Mahzan.Mobile.Services.Store;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.PointsSale
{
    public class AdminPointSalePageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPointSaleService _pointSaleService;
        private readonly IStoreService _storeService;
        
        //Departments
        private ObservableCollection<Store> _stores;
        public ObservableCollection<Store> Stores
        {
            get => _stores;
            set => SetProperty(ref _stores, value);
        }
        private Store _selectedStore;
        public Store SelectedStore
        {
            get => _selectedStore;
            set
            {
                if (_selectedStore != value)
                {
                    _selectedStore = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedStore))); // Notify that there was a change on this property
            }
        }
        
        public Guid PointSaleId { get; set; }
        
        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Code)));
            }
        }
        
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
        
        public ICommand DeletePointSaleCommand { get; set; }
        public ICommand SavePointSaleCommand { get; set; }

        public AdminPointSalePageViewModel(
            INavigationService navigationService, 
            IPointSaleService pointSaleService, 
            IStoreService storeService)
        {
            _navigationService = navigationService;
            _pointSaleService = pointSaleService;
            _storeService = storeService;

            Task.Run(GetStores);
            
            SavePointSaleCommand = new Command(async () => await OnSavePointSaleCommand());
            DeletePointSaleCommand = new Command(async () => await OnDeletePointSaleCommand());
        }

        private async Task OnDeletePointSaleCommand()
        {
            await DeleteCategory();
        }
        
        private async Task DeleteCategory()
        {
            var answer = await Application
                .Current
                .MainPage
                .DisplayAlert("Atención!",
                    "¿Estas seguro de borrar la categoría?", "Si", "No");
            
            if (answer)
            {
                var httpResponseMessage = await _pointSaleService.Delete(PointSaleId.ToString());
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert(
                        "DeleteCategory", 
                        errorApi.Message, 
                        "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Elimina Punto de Venta", 
                        $"Se ha elimindo correctamente el Punto de Venta {Name}", 
                        "Ok");

                    await _navigationService.GoBackAsync();
                }
            }
        }

        private async Task OnSavePointSaleCommand()
        {
            if (PointSaleId==Guid.Empty)
            {
                await CreatePointSale();
            }
            else
            {
                await UpdatePointSale();
            }
        }
        private async Task GetStores()
        {
            var httpResponseMessage = await _storeService.Get(new GetStoreCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetStores", errorApi.Message, "ok");

                return;  
            }
            var getStoresResponse = JsonConvert.DeserializeObject<GetStoresResponse>(respuesta);

            if (getStoresResponse != null)
                Stores = new ObservableCollection<Store>(getStoresResponse.Data);
        }

        public async Task GetPointSale(Guid pointSaleId)
        {
            var httpResponseMessageStores = await _pointSaleService.Get(new GetPointSaleCommand
            {
                PointSale = pointSaleId
            });
           
            var respuesta = await httpResponseMessageStores.Content.ReadAsStringAsync();
          
            if (httpResponseMessageStores.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert("GetPointSale", errorApi.Message, "Ok");
                return;
            }
           
            var getPointsSaleResponse = JsonConvert.DeserializeObject<GetPointsSaleResponse>(respuesta);

            if (getPointsSaleResponse != null)
            {
                SelectedStore = Stores.FirstOrDefault(c => 
                    c.StoreId == getPointsSaleResponse.Data.FirstOrDefault().StoreId);
                Code = getPointsSaleResponse.Data.FirstOrDefault()?.Code;
                Name = getPointsSaleResponse.Data.FirstOrDefault()?.Name;
            }
        }
        
        private async Task UpdatePointSale()
        {
            var httpResponseMessage = await _pointSaleService.Update(new UpdatePointSaleCommand
            {
                PointSaleId= PointSaleId,
                Code = Code,
                Name = Name,
                StoreId = _selectedStore.StoreId,
            });
                
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("UpdatePointSale", errorApi.Message, "Ok");
            }
                
            await App.Current.MainPage.DisplayAlert(
                "Actualización Punto de Venta", 
                $"Se ha actualizado correctamenta el punto de Venta {Name}", 
                "Ok");

            await _navigationService.GoBackAsync();
        }
        
        private async Task CreatePointSale()
        {
            var httpResponseMessage = await _pointSaleService.Create(new CreatePointSaleCommand
            {
                Name = Name,
                Code = Code,
                StoreId = _selectedStore.StoreId
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("CreatePointSale", errorApi.Message, "Ok");
                
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Creación Punto de Venta", 
                    $"Se ha creado correctamente el punto de venta {Name}", 
                    "Ok");
            
                await _navigationService.GoBackAsync();   
            }
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            PointSaleId = parameters.GetValue<Guid>("pointSaleId");
            if (PointSaleId!=Guid.Empty)
            {
                await GetPointSale(PointSaleId);
            }
        }
    }
}