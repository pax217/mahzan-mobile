using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mahzan.Mobile.Services.CommercialBusiness;
using Mahzan.Mobile.Services.Company;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Companies;
using Prism.Navigation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Mahzan.Mobile.Views.Administrator.Settings.Companies
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminCompanyPage : TabbedPage
    {
        private readonly INavigationService _navigationService;

        private readonly ICommercialBusinessService _commercialBusinessService;

        private readonly ICompanyService _companyService;
        
        private readonly AdminCompanyPageViewModel _adminCompanyPageViewModel;
        public AdminCompanyPage(
            INavigationService navigationService,
            ICommercialBusinessService commercialBusinessService,
            ICompanyService companyService)
        {
            InitializeComponent();
            
            _navigationService = navigationService;
            _commercialBusinessService = commercialBusinessService;
            _companyService = companyService;

            _adminCompanyPageViewModel = new AdminCompanyPageViewModel(
                _navigationService,
                _commercialBusinessService,
                _companyService,
                CompanyNameValidator,
                RfcValidator
            );
            
            BindingContext = _adminCompanyPageViewModel;
        }
    }
}