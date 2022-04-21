using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Department;
using Mahzan.Mobile.Models.Company;
using Mahzan.Mobile.Models.Department;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.Department;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Departaments
{
    public class ListDepartmentsPageViewModel: BindableBase, INavigationAware
    {
        private readonly IDepartmentService _departmentService;

        private readonly INavigationService _navigationService;
        
        private ObservableCollection<Department> _listViewDepartments { get; set; }
        public ObservableCollection<Department> ListViewDepartments
        {
            get => _listViewDepartments;
            set
            {
                _listViewDepartments = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewDepartments)));

            }
        }
        
        private Department _selectedDepartment { get; set; }

        public Department SelectedDepartment
        {
            get
            {
                return _selectedDepartment;
            }
            set
            {
                if (_selectedDepartment != value)
                {
                    _selectedDepartment = value;
                    HandleDepartmentStore();
                }
            }
        }

        private void HandleDepartmentStore()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("departmentId", SelectedDepartment.DepartmentId);
            _navigationService.NavigateAsync("AdminDepartmentPage", navigationParams);
        }
        
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        
        public ICommand AddDepartmentCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public ListDepartmentsPageViewModel(
            IDepartmentService departmentService,
            INavigationService navigationService)
        {
            _departmentService = departmentService;
            _navigationService = navigationService;

            Task.Run(GetDepartments);
            
            RefreshCommand = new Command(async () => await OnRefreshCommand());
            AddDepartmentCommand = new Command(async () => await OnAddDepartmentCommand());

        }

        private async Task OnAddDepartmentCommand()
        {
            await _navigationService.NavigateAsync("AdminDepartmentPage");
        }

        private async Task OnRefreshCommand()
        {
            IsRefreshing = true;

            await GetDepartments();
            
            IsRefreshing = false;
        }

        public async Task GetDepartments()
        {
            var httpResponseMessage = await _departmentService.Get(new GetDepartmentsCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetDepartments", errorApi.Message, "ok");

                return;  
            }
            var getDepartmentsResponse = JsonConvert.DeserializeObject<GetDepartmantsResponse>(respuesta);

            if (getDepartmentsResponse != null)
                ListViewDepartments = new ObservableCollection<Department>(getDepartmentsResponse.Data);
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}