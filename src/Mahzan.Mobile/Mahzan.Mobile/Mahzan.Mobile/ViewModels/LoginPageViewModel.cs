using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.User;
using Mahzan.Mobile.Services.SHA1;
using Mahzan.Mobile.Services.User;
using Newtonsoft.Json;
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
            var httpResponseMessage= await _userService.LogIn(UserName, Password);
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode != HttpStatusCode.NoContent)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "Inicio de Sesión", errorApi.Message, "ok");

                return;
            }
            
            await NavigationService.NavigateAsync("SelectStorePage");
        }
    }
}