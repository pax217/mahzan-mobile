using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Store;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.Store;
using Mahzan.Mobile.Services.Store;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;

namespace Mahzan.Mobile.ViewModels.Employee
{
    public class SelectStorePageViewModel: BindableBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IStoreService _storeService;
        private readonly IRepository<User> _userRepository; 
        
        //stores
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
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedStore)));
                HandleSelectedStore();
            }
        }

        private void HandleSelectedStore()
        {
            UpdateSelectedStore(_selectedStore.StoreId, _selectedStore.Name);

            var navigationParams = new NavigationParameters();
            navigationParams.Add("storeId", SelectedStore.StoreId);

            _navigationService.NavigateAsync("SelectPointSalePage", navigationParams);
        }

        public SelectStorePageViewModel(
            INavigationService navigationService, 
            IStoreService storeService, 
            IRepository<User> userRepository)
        {
            _navigationService = navigationService;
            _storeService = storeService;
            _userRepository = userRepository;

            Task.Run(GetStores);
        }
        
        private async Task GetStores()
        {
            var httpResponseMessage = await _storeService.Get(new GetStoreCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert(
                    "GetStores", 
                    errorApi.Message, 
                    "Ok");
            }
            
            var getStoresResponse = JsonConvert.DeserializeObject<GetStoresResponse>(respuesta);
            if (getStoresResponse != null) 
                Stores = new ObservableCollection<Store>(getStoresResponse.Data);
        }
        
        private async void UpdateSelectedStore(Guid storeId,
            string storeName)
        {
            var user = await _userRepository.Get();
            var userToUpdate = user.FirstOrDefault();

            if (userToUpdate != null)
            {
                userToUpdate.StoreId = storeId;
                userToUpdate.StoreName = storeName;
            }

            await _userRepository.Update(userToUpdate);
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
      
        }
    }
}