using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.User;
using Mahzan.Mobile.Services.User;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Mahzan.Mobile.Views;
using Mahzan.Mobile.Views.Administrator;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels
{
public class LoginPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IUserService _userService;
        
        private readonly IRepository<User> _userRepository;

        public string UserName { get; set; }

        public string Password { get; set; }

        public ICommand LoginCommand { get; set; }
        
        public ICommand SignUpCommand { get; set; }
        
        public ICommand ForgotPasswordCommand { get; set; }
        

        public LoginPageViewModel(
            INavigationService navigationService,
            IUserService userService,
            IRepository<User> userRepository)
        {

            //Services
            _userService = userService;
            
            //Repositories
            _userRepository = userRepository;

            //Navigation
            _navigationService = navigationService;

            //Commands
            LoginCommand = new Command(async () => await OnLogIn());
            SignUpCommand = new Command(async () => await OnSignUp());
            ForgotPasswordCommand = new Command(async () => await OnForgotPasswordCommand());
        }


        public async Task OnLogIn()
        {
            var httpResponseMessage= await _userService.LogIn("mahzan", "Mahzan22%&");
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "Inicio de Sesión", errorApi.Message, "ok");

                return;
            }
            
            var logInResponse = JsonConvert.DeserializeObject<LogInResponse>(respuesta);

            if (logInResponse != null)
            {
                await SaveOnSqlite(logInResponse);

                switch (@logInResponse.Role)
                {
                    case "Administrator":
                        await _navigationService.NavigateAsync(nameof(MainPage) + "/" + nameof(NavigationPage) + "/" + nameof(AdministratorDashboardPage));
                        break;
                    case "Cashier":
                        break;
                }
            }
        }

        public async Task OnSignUp()
        {
            await _navigationService.NavigateAsync("SignUpPage");
        }

        public async Task OnForgotPasswordCommand()
        {
            await _navigationService.NavigateAsync("ResetPasswordPage");
        }

        private async Task SaveOnSqlite(LogInResponse logInResponse)
        {
            await _userRepository.DeleteAll();
            await _userRepository.Insert(new User()
            {
                Token = logInResponse.Token,
                Role = logInResponse.Role,
                UserName = logInResponse.UserName
            }); 
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}