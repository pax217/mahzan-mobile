using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Behaviors;
using Mahzan.Mobile.Commands.User;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.User;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels
{
    public class ResetPasswordPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IUserService _userService;
        
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Email)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(PassedValidation)));
            }
        }

        public ICommand ReturnLogInCommand { get; set; }
        
        public ICommand ResetPasswordCommand { get; set; }
        
        private EmailValidatorBehavior EmailValidatorBehavior;
        
        public bool PassedValidation
        {
            get
            {
                return EmailValidatorBehavior.IsValid != null;
            }
        }

        public ResetPasswordPageViewModel(
            INavigationService navigationService,
            IUserService userService,
            EmailValidatorBehavior emailValidatorBehavior)
        {
            _navigationService = navigationService;
            _userService = userService;
            EmailValidatorBehavior = emailValidatorBehavior;
            
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
            await _navigationService.GoBackAsync();
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