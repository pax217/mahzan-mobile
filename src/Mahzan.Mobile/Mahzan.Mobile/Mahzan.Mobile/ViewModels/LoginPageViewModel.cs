using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels
{
public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public string UserName { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }

        public LoginPageViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Inicio de Sesión";
            

            //Navigation
            _navigationService = navigationService;

            //Commands
            LoginCommand = new Command(async () => await LogIn());
        }


        public async Task LogIn()
        {


        }
    }
}