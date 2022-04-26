using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Tax;
using Mahzan.Mobile.Models.Department;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.Tax;
using Mahzan.Mobile.Services.Tax;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Taxes
{
    public class ListTaxesPageViewModel:BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly ITaxService _taxService;
        
        //Taxes
        private ObservableCollection<Tax> _listViewTaxes { get; set; }
        public ObservableCollection<Tax> ListViewTaxes
        {
            get => _listViewTaxes;
            set
            {
                _listViewTaxes = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewTaxes)));

            }
        }
        
        private Tax _selectedTax { get; set; }

        public Tax SelectedTax
        {
            get
            {
                return _selectedTax;
            }
            set
            {
                if (_selectedTax != value)
                {
                    _selectedTax = value;
                    HandleTax();
                }
            }
        }
        
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }

        public ICommand AddTaxCommand { get; set; }
        
        public ICommand RefreshCommand { get; set; }
        
        public ListTaxesPageViewModel(
            INavigationService navigationService, 
            ITaxService taxService)
        {
            _navigationService = navigationService;
            _taxService = taxService;

            Task.Run(GetTaxes);
            
            RefreshCommand = new Command(async () => await OnRefreshCommand());
            AddTaxCommand = new Command(async () => await OnAddTaxCommand());

        }
        
        private async Task OnRefreshCommand()
        {
            IsRefreshing = true;

            await GetTaxes();
            
            IsRefreshing = false;
        }
        
        private async Task OnAddTaxCommand()
        {
            await _navigationService.NavigateAsync("AdminTaxPage");
        }
        
        private void HandleTax()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("taxId", SelectedTax.TaxId);
            _navigationService.NavigateAsync("AdminTaxPage", navigationParams);
        }

        private async Task GetTaxes()
        {
            var httpResponseMessage = await _taxService.Get(new GetTaxCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetTaxes", errorApi.Message, "ok");

                return;  
            }
            var getTaxesResponse = JsonConvert.DeserializeObject<GetTaxesResponse>(respuesta);

            if (getTaxesResponse != null)
                ListViewTaxes = new ObservableCollection<Tax>(getTaxesResponse.Data);
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}