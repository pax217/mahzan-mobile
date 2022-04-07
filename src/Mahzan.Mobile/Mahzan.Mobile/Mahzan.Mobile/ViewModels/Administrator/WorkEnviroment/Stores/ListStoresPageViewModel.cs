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
using Plugin.Toasts;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.WorkEnviroment.Stores
{
    public class ListStoresPageViewModel : BindableBase, INavigationAware
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
                    HandleSelectedStore();
                }
            }
        }

        private void HandleSelectedStore()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("storeId", SelectedStore.StoreId);
            _navigationService.NavigateAsync("AdminStorePage", navigationParams);
        }

        //Commands
        public ICommand AddStoreCommand { get; set; }
        public ListStoresPageViewModel(
            INavigationService navigationService,
            IStoreService storesService)
        {

            _navigationService = navigationService;
            _storeService = storesService;

            //Stores
            Task.Run(() => GetStores());

            //Commands
            AddStoreCommand = new Command(async () => await OnAddStoreCommand());
        }
        private async Task OnAddStoreCommand()
        {
            await _navigationService.NavigateAsync("AdminStorePage");
        }
        private async Task GetStores()
        {
            var httpResponseMessage = await _storeService.Get(new GetStoreCommand()
            {

            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await ShowCreateProductToast(Color.Red, "Crea producto",errorApi.Message);
                return;
            }
            
            var getStoresResponse = JsonConvert.DeserializeObject<GetStoresResponse>(respuesta);
            if (getStoresResponse != null) 
                ListViewStores = new ObservableCollection<Store>(getStoresResponse.Data);
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            await GetStores();
        }
        
        //Notifications
        public async Task ShowCreateProductToast(Color color,string title, string description)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
                
            var options = new NotificationOptions()
            {
                Title = title,
                Description = description,
                IsClickable = true, //make it false if you don't want the notification to be clickable
                AndroidOptions = new AndroidOptions()
                {
                    HexColor = color.ToHex(),
                    ForceOpenAppOnNotificationTap = true
                },
                // DelayUntil = DateTime.Now.AddSeconds(1)
            };
            
            await notificator.Notify(options);
            
            //var result = await notificator.Notify(options);

            // if(result.Action == NotificationAction.Clicked)
            // {
            //     await App.Current.MainPage.DisplayAlert("Alert", "Grab ID and tile and desc : " + result.Id + " " + options.Title, "Ok");
            // }
            //
        }
    }
}