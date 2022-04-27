using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using Mahzan.Mobile.Commands.Employee;
using Mahzan.Mobile.Models.Department;
using Mahzan.Mobile.Models.Employee;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.Employee;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Employees
{
    public class ListEmployeesPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IEmployeeService _employeeService;
        
        private ObservableCollection<Employee> _listViewEmployees { get; set; }
        public ObservableCollection<Employee> ListViewEmployees
        {
            get => _listViewEmployees;
            set
            {
                _listViewEmployees = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewEmployees)));

            }
        }
        
        private Employee _selectedEmployee { get; set; }

        public Employee SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                if (_selectedEmployee != value)
                {
                    _selectedEmployee = value;
                    HandleSelectedEmployee();
                }
            }
        }

        public ListEmployeesPageViewModel(INavigationService navigationService, IEmployeeService employeeService)
        {
            _navigationService = navigationService;
            _employeeService = employeeService;

            Task.Run(GetEmployees);
        }
        
        private void HandleSelectedEmployee()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("employeeId", SelectedEmployee.EmployeeId);
            _navigationService.NavigateAsync("AdminEmployeePage", navigationParams);
        }

        private async Task GetEmployees()
        {
            var httpResponseMessage = await _employeeService.Get(new GetEmployeeCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetEmployees", errorApi.Message, "ok");

                return;  
            }
            var getEmployeesResponse = JsonConvert.DeserializeObject<GetEmployeesResponse>(respuesta);

            if (getEmployeesResponse != null)
                ListViewEmployees = new ObservableCollection<Employee>(getEmployeesResponse.Data);
  
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {
   
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}