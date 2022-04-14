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
    public partial class ResetPasswordPage : ContentPage
    {
        private readonly INavigationService _navigationService;

        private readonly IUserService _userService;
        
        private ResetPasswordPageViewModels viewModel;
        public ResetPasswordPage(
            INavigationService navigationService,
            IUserService userService)
        {
            _navigationService = navigationService;
            _userService = userService;
            
            InitializeComponent();

            
            viewModel = new ResetPasswordPageViewModels(
                _navigationService, 
                _userService,
                emailValidator);

            BindingContext = viewModel;
        }
    }
}