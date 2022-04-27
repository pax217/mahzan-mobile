using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.PointSale;
using Mahzan.Mobile.Models.Department;
using Mahzan.Mobile.Models.PointSale;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.PointSale;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.PointsSale
{
    public class ListPointsSalePageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService; 
        private readonly IPointSaleService _pointSaleService;
        
        private ObservableCollection<PointSale> _listViewPointsSale { get; set; }
        public ObservableCollection<PointSale> ListViewPointsSale
        {
            get => _listViewPointsSale;
            set
            {
                _listViewPointsSale = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewPointsSale)));

            }
        }
        
        private PointSale _selectedPointSale { get; set; }

        public PointSale SelectedPointSale
        {
            get
            {
                return _selectedPointSale;
            }
            set
            {
                if (_selectedPointSale != value)
                {
                    _selectedPointSale = value;
                    HandleSelectedPointSale();
                }
            }
        }
        
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        
        public ICommand AddPointSaleCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public ListPointsSalePageViewModel(
            IPointSaleService pointSaleService, 
            INavigationService navigationService)
        {
            _pointSaleService = pointSaleService;
            _navigationService = navigationService;

            Task.Run(GetPointsSale);
            
            RefreshCommand = new Command(async () => await OnRefreshCommand());
            AddPointSaleCommand = new Command(async () => await OnAddPointSaleCommand());

        }

        private async Task OnAddPointSaleCommand()
        {
            await _navigationService.NavigateAsync("AdminPointSalePage");
        }

        private async Task OnRefreshCommand()
        {
            IsRefreshing = true;

            await GetPointsSale();
            
            IsRefreshing = false;
        }
        
        private void HandleSelectedPointSale()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("pointSaleId", SelectedPointSale.PointSaleId);
            _navigationService.NavigateAsync("AdminPointSalePage", navigationParams);
        }

        private async Task GetPointsSale()
        {
            var httpResponseMessage = await _pointSaleService.Get(new GetPointSaleCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetDepartments", errorApi.Message, "ok");

                return;  
            }
            var getPointsSaleResponse = JsonConvert.DeserializeObject<GetPointsSaleResponse>(respuesta);

            if (getPointsSaleResponse != null)
                ListViewPointsSale = new ObservableCollection<PointSale>(getPointsSaleResponse.Data);
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}