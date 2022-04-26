using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.UnitSale;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.Tax;
using Mahzan.Mobile.Models.UnitSale;
using Mahzan.Mobile.Services.UnitsSale;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.UnitsSale
{
    public class ListUnitsSalePageViewModel: BindableBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;
        private readonly IUnitSaleService _unitSaleService;
        
        //Units Sale
        private ObservableCollection<UnitSale> _listViewUnitsSale { get; set; }
        public ObservableCollection<UnitSale> ListViewUnitsSale
        {
            get => _listViewUnitsSale;
            set
            {
                _listViewUnitsSale = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewUnitsSale)));

            }
        }
        
        private UnitSale _selectedUnitsSale { get; set; }

        public UnitSale SelectedUnitsSale
        {
            get
            {
                return _selectedUnitsSale;
            }
            set
            {
                if (_selectedUnitsSale != value)
                {
                    _selectedUnitsSale = value;
                    HandleUnitSale();
                }
            }
        }
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        
        public ICommand AddUnitSaleCommand { get; set; }
        
        public ICommand RefreshCommand { get; set; }

        public ListUnitsSalePageViewModel(
            INavigationService navigationService, 
            IUnitSaleService unitSaleService)
        {
            _navigationService = navigationService;
            _unitSaleService = unitSaleService;

            Task.Run(GetUnitsSale);
            
            RefreshCommand = new Command(async () => await OnRefreshCommand());
            AddUnitSaleCommand = new Command(async () => await OnAddUnitSaleCommand());
        }

        private async Task OnAddUnitSaleCommand()
        {
            await _navigationService.NavigateAsync("AdminUnitSalePage");
        }

        private async Task OnRefreshCommand()
        {
            IsRefreshing = true;

            await GetUnitsSale();
            
            IsRefreshing = false;
        }

        private void HandleUnitSale()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("unitSaleId", SelectedUnitsSale.UnitSaleId);
            _navigationService.NavigateAsync("AdminUnitSalePage", navigationParams);
        }

        private async Task GetUnitsSale()
        {
            var httpResponseMessage = await _unitSaleService.Get(new GetUnitsSaleCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetTaxes", errorApi.Message, "ok");

                return;  
            }
            var getUnitsSaleResponse = JsonConvert.DeserializeObject<GetUnitsSaleResponse>(respuesta);

            if (getUnitsSaleResponse != null)
                ListViewUnitsSale = new ObservableCollection<UnitSale>(getUnitsSaleResponse.Data);
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}