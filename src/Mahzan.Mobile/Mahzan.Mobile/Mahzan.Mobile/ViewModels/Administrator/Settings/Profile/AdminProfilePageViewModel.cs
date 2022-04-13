using System.ComponentModel;
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

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Profile
{
    public class AdminProfilePageViewModel:BindableBase,INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IUserService _userService;
        
        private string _newPassword;
        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                _newPassword = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(NewPassword))); 
            }
        }

        public ICommand ChangePasswordCommand { get; set; }

        public AdminProfilePageViewModel(
            INavigationService navigationService,
            IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;


            ChangePasswordCommand = new Command(async () => await OnChangePasswordCommand());
        }

        private async Task OnChangePasswordCommand()
        {
            var httpResponseMessage  = await _userService.ChangePassword(new ChangePasswordCommand
            {
                NewPasswordDecrypted = NewPassword
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "OnSignUpCommand", errorApi.Message, "ok");
            }
            
            await Application.Current.MainPage.DisplayAlert(
                "Cambio de Contraseña", 
                "Se ha cambiado correctamente la contraseña", 
                "ok");
            
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