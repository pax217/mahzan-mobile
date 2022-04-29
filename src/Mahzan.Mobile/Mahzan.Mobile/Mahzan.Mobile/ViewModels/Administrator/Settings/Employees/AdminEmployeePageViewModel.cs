using System;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Employee;
using Mahzan.Mobile.Models.Employee;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.Employee;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Employees
{
    public class AdminEmployeePageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IEmployeeService _employeeService;
        
        public Guid EmployeeId { get; set; }
        
        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Code)));
            }
        }
        
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(FirstName)));
            }
        }
        
        private string _secondName;
        public string SecondName
        {
            get { return _secondName; }
            set
            {
                _secondName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SecondName)));
            }
        }
        
        private string _lastName;
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(LastName)));
            }
        }
        
        private string _sureName;
        public string SureName
        {
            get { return _sureName; }
            set
            {
                _sureName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SureName)));
            }
        }
        
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Email)));
            }
        }
        
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Phone)));
            }
        }
        
        public ICommand SaveEmployeeCommand { get; set; }
        
        public ICommand DeleteEmployeeCommand { get; set; }
        public AdminEmployeePageViewModel(
            INavigationService navigationService, 
            IEmployeeService employeeService)
        {
            _navigationService = navigationService;
            _employeeService = employeeService;

            SaveEmployeeCommand = new Command(async () => await OnSaveEmployeeCommand());

        }

        private async Task OnSaveEmployeeCommand()
        {
            if (EmployeeId==Guid.Empty)
            {
                await CreateEmployee();
            }
        }
        
        private async Task CreateEmployee()
        {
            var httpResponseMessage = await _employeeService.Create(new CreateEmployeeCommand
            {
                Code = Code,
                FirstName = FirstName,
                SecondName = SecondName,
                LastName = LastName,
                SureName = SureName,
                Email = Email,
                Phone = Phone
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("CreateEmployee", errorApi.Message, "Ok");
                
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Creación de Empleado", 
                    $"Se ha creado correctamente el Empleado {FirstName} {LastName}", 
                    "Ok");
            
                await _navigationService.GoBackAsync();   
            }
        }
        
        public async Task GetEmployee(Guid employeeId)
        {
            var httpResponseMessageStores = await _employeeService.Get(new GetEmployeeCommand
            {
                EmployeeId = employeeId
            });
           
            var respuesta = await httpResponseMessageStores.Content.ReadAsStringAsync();
          
            if (httpResponseMessageStores.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetUnitSale", errorApi.Message, "Ok");
            }
           
            var getEmployeesResponse = JsonConvert.DeserializeObject<GetEmployeesResponse>(respuesta);

            if (getEmployeesResponse != null)
            {
                Code = getEmployeesResponse.Data.FirstOrDefault()?.Code;
                FirstName = getEmployeesResponse.Data.FirstOrDefault()?.FirstName;
                SecondName = getEmployeesResponse.Data.FirstOrDefault()?.SecondName;
                LastName = getEmployeesResponse.Data.FirstOrDefault()?.LastName;
                Email= getEmployeesResponse.Data.FirstOrDefault()?.Email;
                Phone= getEmployeesResponse.Data.FirstOrDefault()?.Phone;
            }
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            EmployeeId = parameters.GetValue<Guid>("employeeId");
            if (EmployeeId!=Guid.Empty)
            {
                await GetEmployee(EmployeeId);
            }
        }
    }
}