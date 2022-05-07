using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.QrScanning;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Employee.Operations.Sales
{
    public class NewSalePageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        
        public ICommand ReadBarCodeCommand { get; private set; }

        public NewSalePageViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;
            
            ReadBarCodeCommand = new Command(async () => await OnReadBarCodeCommand());
        }

        private async Task OnReadBarCodeCommand()
        {
            var scanner = DependencyService.Get<IQrScanningService>();
            var barCode = await scanner.ScanAsync();
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}