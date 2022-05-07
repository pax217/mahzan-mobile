using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Employee.Operations.Sales
{
    public class ListSalesPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        public ICommand AddSaleCommand { get; set; }
        
        public ListSalesPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            
            AddSaleCommand = new Command(async () => await OnAddSaleCommand());
        }

        private async Task OnAddSaleCommand()
        {
           await _navigationService.NavigateAsync("NewSalePage");
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}