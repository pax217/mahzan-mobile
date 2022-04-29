using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.PointSale;
using Mahzan.Mobile.Models.PointSale;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.PointSale;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Mahzan.Mobile.Views;
using Mahzan.Mobile.Views.Administrator;
using Mahzan.Mobile.Views.Employee;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Employee
{
    public class SelectPointSalePageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPointSaleService _pointSaleService;
        private readonly IRepository<User> _userRepository; 
        
        //Points SAle
        private ObservableCollection<PointSale> _pointsSale;
        public ObservableCollection<PointSale> PointsSale
        {
            get => _pointsSale;
            set => SetProperty(ref _pointsSale, value);
        }
        private PointSale _selectedPointsSale;
        public PointSale SelectedPointsSale
        {
            get => _selectedPointsSale;
            set
            {
                if (_selectedPointsSale != value)
                {
                    _selectedPointsSale = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedPointsSale)));
                HandleSelectedPointSale();
            }
        }
        
        public SelectPointSalePageViewModel(
            INavigationService navigationService, 
            IPointSaleService pointSaleService,
            IRepository<User> userRepository)
        {
            _navigationService = navigationService;
            _pointSaleService = pointSaleService;
            _userRepository = userRepository;
        }
        
        private void HandleSelectedPointSale()
        {
            Task.Run(()=> UpdateSelectedPointSale(_selectedPointsSale.StoreId, _selectedPointsSale.Name));
            _navigationService.NavigateAsync(nameof(MainPage) + "/" + nameof(NavigationPage) + "/" + nameof(EmployeeDashboardPage));
        }
        
        private async Task UpdateSelectedPointSale(Guid pointSaleId,
            string pointSaleName)
        {
            var user = await _userRepository.Get();
            var userToUpdate = user.FirstOrDefault();
        
            if (userToUpdate != null)
            {
                userToUpdate.PointSaleId = pointSaleId;
                userToUpdate.PointSaleName = pointSaleName;
            }
        
            await _userRepository.Update(userToUpdate);
        }
        
        private async Task GetPointsSale(Guid storeId)
        {
            var httpResponseMessage = await _pointSaleService.Get(new GetPointSaleCommand
            {
                StoreId = storeId
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetPointsSale", errorApi.Message, "ok");

                return;  
            }
            var getPointsSaleResponse = JsonConvert.DeserializeObject<GetPointsSaleResponse>(respuesta);

            if (getPointsSaleResponse != null)
                PointsSale = new ObservableCollection<PointSale>(getPointsSaleResponse.Data);
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            var storeId = parameters.GetValue<Guid>("storeId");
            if (storeId!=Guid.Empty)
            {
                await GetPointsSale(storeId);
            }
        }
    }
}