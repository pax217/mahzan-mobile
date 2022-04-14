using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mahzan.Mobile.Services.User;
using Mahzan.Mobile.ViewModels;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mahzan.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private readonly INavigationService _navigationService;

        private readonly IUserService _userService;
        
        private readonly SignUpPageViewModel _signUpPageViewModel;
        public SignUpPage(
            INavigationService navigationService,
            IUserService userService)
        {
            InitializeComponent();
            
            _navigationService = navigationService;
            _userService = userService;
            
            _signUpPageViewModel = new SignUpPageViewModel(
                _navigationService,
                _userService,
                CompanyNameValidator,
                ContactNameValidator,
                EmailValidator,
                UserValidator,
                PasswordValidator);
            
            BindingContext = _signUpPageViewModel;
        }
    }
}