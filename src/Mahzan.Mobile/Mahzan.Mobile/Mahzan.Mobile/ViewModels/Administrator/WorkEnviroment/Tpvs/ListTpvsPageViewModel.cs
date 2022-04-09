using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Store;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.Store;
using Mahzan.Mobile.Services.Store;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.WorkEnviroment.Tpvs
{
    public class ListTpvsPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        
        private readonly IStoreService _storeService;
        
        private ObservableCollection<Store> _listViewStores { get; set; }
        public ObservableCollection<Store> ListViewStores
        {
            get => _listViewStores;
            set
            {
                _listViewStores = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewStores)));

            }
        }
        
        private Store _selectedStore { get; set; }
        public Store SelectedStore
        {
            get
            {
                return _selectedStore;
            }
            set
            {
                if (_selectedStore != value)
                {
                    _selectedStore = value;
                    GetTpvs();
                }
            }
        }
        
        public ICommand AddPointOfSaleCommand { get; set; }
        
        public ListTpvsPageViewModel(
            INavigationService navigationService,
            IStoreService service)
        {
            _navigationService = navigationService;
            _storeService = service;
            
            Task.Run(() => GetStores());
            
            //Commands
            AddPointOfSaleCommand = new Command(async () => await OnAddPointOfSaleCommand());
        }

        private async Task GetStores()
        {
            var httpResponseMessage = await _storeService.Get(new GetStoreCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                App.Current.MainPage.DisplayAlert("GetStore", errorApi.Message, "Ok");
                return;
            }
            
            var getStoresResponse = JsonConvert.DeserializeObject<GetStoresResponse>(respuesta);
            if (getStoresResponse != null) 
                ListViewStores = new ObservableCollection<Store>(getStoresResponse.Data);
        }

        private async Task GetTpvs()
        {
        }
        
        private async Task OnAddPointOfSaleCommand()
        {
            await _navigationService.NavigateAsync("AdminTpvsPage");
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