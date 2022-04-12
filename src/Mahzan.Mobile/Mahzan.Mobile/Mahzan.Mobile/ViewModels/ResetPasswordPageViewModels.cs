using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.User;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.User;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels
{
    public class ResetPasswordPageViewModels: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IUserService _userService;
        
        private string _email;
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public ICommand ReturnLogInCommand { get; set; }
        
        public ICommand ResetPasswordCommand { get; set; }

        public ResetPasswordPageViewModels(
            INavigationService navigationService,
            IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
            
            ReturnLogInCommand = new Command(async () => await OnReturnLogInCommand());
            ResetPasswordCommand = new Command(async () => await OnResetPassword());
        }

        
        private async Task OnResetPassword()
        {
            var httpResponseMessage = await _userService.ResetPassword(new ResetPasswordCommand
            {
                Email = Email
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "OnResetPassword", errorApi.Message, "ok");
            }
            
            await Application.Current.MainPage.DisplayAlert(
                "Recuperacion de Contraseña", 
                "Se ha enviado tu nueva contraseña al correo electronico", 
                "ok");
        }
        private async Task OnReturnLogInCommand()
        {
            await _navigationService.NavigateAsync("LoginPage");
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