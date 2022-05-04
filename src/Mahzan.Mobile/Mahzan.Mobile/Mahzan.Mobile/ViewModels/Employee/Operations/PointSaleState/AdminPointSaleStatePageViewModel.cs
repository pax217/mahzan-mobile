using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Models.PointSaleState;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.PointSaleState;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Employee.Operations.PointSaleState
{
    public class AdminPointSaleStatePageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        private readonly IPointSaleStateService _pointSaleStateService;
        private readonly IRepository<User> _userRepository; 
        
        private int _tenCents;
        public int TenCents
        {
            get => _tenCents;
            set => SetProperty(ref _tenCents, value);
        }
        
        private int _twentyCents;
        public int TwentyCents
        {
            get => _twentyCents;
            set => SetProperty(ref _twentyCents, value);
        }
        
        private int _fiftyCents;
        public int FiftyCents
        {
            get => _fiftyCents;
            set => SetProperty(ref _fiftyCents, value);
        }
        
        private int _one;
        public int One
        {
            get => _one;
            set => SetProperty(ref _one, value);
        }
        
        private int _two;
        public int Two
        {
            get => _two;
            set => SetProperty(ref _two, value);
        }
        
        private int _five;
        public int Five
        {
            get => _five;
            set => SetProperty(ref _five, value);
        }
        
        private int _ten;
        public int Ten
        {
            get => _ten;
            set => SetProperty(ref _ten, value);
        }
        
        private int _twenty;
        public int Twenty
        {
            get => _twenty;
            set => SetProperty(ref _twenty, value);
        }
        
        private int _fifty;
        public int Fifty
        {
            get => _fifty;
            set => SetProperty(ref _fifty, value);
        }
        
        private int _hundred;
        public int Hundred
        {
            get => _hundred;
            set => SetProperty(ref _hundred, value);
        }
        
        private int _twoHundred;
        public int TwoHundred
        {
            get => _twoHundred;
            set => SetProperty(ref _twoHundred, value);
        }
        
        private int _fiveHundred;
        public int FiveHundred
        {
            get => _fiveHundred;
            set => SetProperty(ref _fiveHundred, value);
        }
        
        private int _oneThousand;
        public int OneThousand
        {
            get => _oneThousand;
            set => SetProperty(ref _oneThousand, value);
        }

        public ICommand OpenPointSalestateCommand { get; set; }
        public AdminPointSaleStatePageViewModel(
            INavigationService navigationService, 
            IPointSaleStateService pointSaleStateService, IRepository<User> userRepository)
        {
            _navigationService = navigationService;
            _pointSaleStateService = pointSaleStateService;
            _userRepository = userRepository;

            OpenPointSalestateCommand = new Command(async () => await OnOpenPointSaleStateCommand());
        }

        private async Task OnOpenPointSaleStateCommand()
        {
            await OpenPointSaleState();
        }

        private async Task OpenPointSaleState()
        {
            var users = await _userRepository.Get();
            var user = users.FirstOrDefault();
            
            var httpResponseMessage = await _pointSaleStateService.Create(new CreatePointSaleStateCommand
            {
                PointSaleId = user.PointSaleId,
                Coins = new Coins
                {
                    TenCents = TenCents,
                    TwentyCents = TwentyCents,
                    FiftyCents = FiftyCents,
                    One = One,
                    Two = Two,
                    Five = Five,
                    Ten = Ten,
                },
                Bills = new Bills
                {
                    Twenty = Twenty,
                    Fifty = Fifty,
                    Hundred = Hundred,
                    TwoHundred = TwoHundred,
                    FiveHundred = FiveHundred,
                    OneThousand = OneThousand
                }
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("OpenPointSaleState", errorApi.Message, "Ok");
                return;
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Abrir Punto de Venta", 
                    $"Se ha abierto correctamente el punto de venta", 
                    "Ok");
                
            }
        }


        public async void OnNavigatedFrom(INavigationParameters parameters)
        {
   
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}