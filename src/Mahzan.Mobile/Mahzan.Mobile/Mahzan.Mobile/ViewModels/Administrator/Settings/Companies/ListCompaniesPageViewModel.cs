using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Company;
using Mahzan.Mobile.Models.Company;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.Company;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Companies
{
    public class ListCompaniesPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly ICompanyService _companyService;
        
        private ObservableCollection<Company> _listViewCompanies { get; set; }
        public ObservableCollection<Company> ListViewCompanies
        {
            get => _listViewCompanies;
            set
            {
                _listViewCompanies = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewCompanies)));

            }
        }

        private Company _selectedCompany { get; set; }

        public Company SelectedCompany
        {
            get
            {
                return _selectedCompany;
            }
            set
            {
                if (_selectedCompany != value)
                {
                    _selectedCompany = value;
                    HandleSelectedCompany();
                }
            }
        }
        
        public ICommand AddCompanyCommand { get; set; }

        public ListCompaniesPageViewModel(
            INavigationService navigationService,
            ICompanyService companyService)
        {
            _navigationService = navigationService;
            _companyService = companyService;

            Task.Run(() => GetCompanies());

            AddCompanyCommand = new Command(async () => await OnAddCompanyCommand());
        }

        private async Task GetCompanies()
        {
          var httpResponseMessage=  await _companyService.Get(new GetCompaniesCommand());
          
          var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

          if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
          {
              var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
              await Application.Current.MainPage.DisplayAlert(
                  "Inicio de Sesión", errorApi.Message, "ok");

              return;  
          }
          var getCompaniesResponse = JsonConvert.DeserializeObject<GetCompaniesResponse>(respuesta);

          if (getCompaniesResponse != null)
              ListViewCompanies = new ObservableCollection<Company>(getCompaniesResponse.Data);
        }
        
        private void HandleSelectedCompany()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("companyId", SelectedCompany.CompanyId);
            _navigationService.NavigateAsync("AdminCompanyPage", navigationParams);
        }

        private async Task OnAddCompanyCommand()
        {
            await _navigationService.NavigateAsync("AdminCompanyPage");
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}