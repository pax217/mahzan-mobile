using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Services.User;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels
{
public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private readonly IUserService _userService;

        public string UserName { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }

        public LoginPageViewModel(
            INavigationService navigationService,
            IUserService userService)
            : base(navigationService)
        {
            Title = "Inicio de Sesión";
            

            //Services
            _userService = userService;
            
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