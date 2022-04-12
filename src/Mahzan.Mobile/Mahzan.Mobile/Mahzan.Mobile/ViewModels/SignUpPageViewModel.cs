using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Company;
using Mahzan.Mobile.Commands.User;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.SHA1;
using Mahzan.Mobile.Services.User;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels
{
    public class SignUpPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IUserService _userService;

        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(CompanyName))); // Notify that there was a change on this property
            }
        }
        
        private string _conatctName;
        public string ContactName
        {
            get { return _conatctName; }
            set
            {
                _conatctName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ContactName))); // Notify that there was a change on this property
            }
        }
        
        private string _whatsappPhone;
        public string WhatsappPhone
        {
            get { return _whatsappPhone; }
            set
            {
                _whatsappPhone = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(WhatsappPhone))); // Notify that there was a change on this property
            }
        }
        
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Email))); // Notify that there was a change on this property
            }
        }
        
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(UserName))); // Notify that there was a change on this property
            }
        }
        
        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Password))); // Notify that there was a change on this property
            }
        }
        
        public ICommand ReturnLogInCommand { get; set; }
        
        public ICommand SignUpCommand { get; set; }
        
        
        public SignUpPageViewModel(
            INavigationService navigationService,
            IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;


            ReturnLogInCommand = new Command(async () => await OnReturnLogInCommand());
            SignUpCommand = new Command(async () => await OnSignUpCommand());
            
        }


        private async Task OnReturnLogInCommand()
        {
            await _navigationService.NavigateAsync("LoginPage");
        }

        private async Task OnSignUpCommand()
        {
            var httpResponseMessage = await _userService.Create(new CreateUserCommand
            {
                CompanyName = CompanyName,
                ContactName = ContactName,
                WhatsappPhone = WhatsappPhone,
                Email = Email,
                UserName = UserName,
                Password = Password
            });
          
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "OnSignUpCommand", errorApi.Message, "ok");
            }
            
            await Application.Current.MainPage.DisplayAlert(
                "Registo de Usuario", 
                "Se ha creado correctamente el usuario, confirma tu email para poder iniciar sesión", 
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