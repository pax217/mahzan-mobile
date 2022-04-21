using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Department;
using Mahzan.Mobile.Commands.Store;
using Mahzan.Mobile.Models.Company;
using Mahzan.Mobile.Models.Department;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.Store;
using Mahzan.Mobile.Services.Department;
using Mahzan.Mobile.Services.Store;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Departaments
{
    public class AdminDepartmentPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IStoreService _storeService;

        private readonly IDepartmentService _departmentService;
        
        //Stores
        private ObservableCollection<Store> _stores;
        public ObservableCollection<Store> Stores
        {
            get => _stores;
            set => SetProperty(ref _stores, value);
        }
        private Store _selectedStore;
        public Store SelectedStore
        {
            get => _selectedStore;
            set
            {
                if (_selectedStore != value)
                {
                    _selectedStore = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedStore))); // Notify that there was a change on this property
            }
        }
        
        public Guid DepartmentId { get; set; }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        public ICommand SaveDepartmentCommand { get; set; }
        
        public ICommand DeleteDepartmentCommand { get; set; }
        
        public AdminDepartmentPageViewModel(
            IStoreService storeService,
            IDepartmentService departmentService,
            INavigationService navigationService)
        {

            _storeService = storeService;
            _navigationService = navigationService;
            _departmentService = departmentService;
            
            Task.Run(GetStores);
            
            SaveDepartmentCommand = new Command(async () => await OnSaveDepartmentCommand());
            
            DeleteDepartmentCommand = new Command(async () => await OnDeleteDepartmentCommand());
            
        }

        private async Task OnDeleteDepartmentCommand()
        {
            if (DepartmentId==Guid.Empty)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Elimina el departamento", 
                    $"Debes seleccionar un departamento", 
                    "Ok");
            }
            else
            {
                await DeleteDepartment();
            }   
        }

        private async Task DeleteDepartment()
        {
            var answer = await Application
                .Current
                .MainPage
                .DisplayAlert("Atención!",
                    "¿Estas seguro de borrar el departamento?", "Si", "No");
            
            if (answer)
            {
                var httpResponseMessage = await _departmentService.Delete(DepartmentId.ToString());
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert("DeleteCompany", errorApi.Message, "Ok");
                   
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Elimina Departamento", 
                        $"Se ha elimindo correctamente el departamento {Name}", 
                        "Ok");

                    await _navigationService.GoBackAsync();
                }
            }
        }

        private async Task OnSaveDepartmentCommand()
        {
            if (DepartmentId==Guid.Empty)
            {
                await CreateDepartment();
            }
            else
            {
                await UpdateDepartment();
            }
        }

        private async Task UpdateDepartment()
        {
            var httpResponseMessage = await _departmentService.Update(new UpdateDepartmentCommand
            {
                
                DepartmentId = DepartmentId,
                Name = Name,
                StoreId = _selectedStore.StoreId,
            });
                
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("UpdateDepartment", errorApi.Message, "Ok");
            }
                
            await App.Current.MainPage.DisplayAlert(
                "Actualización de Departamento", 
                $"Se ha actualizado correctamenta el departamento {Name}", 
                "Ok");

            await _navigationService.GoBackAsync();
        }

        private async Task CreateDepartment()
        {
            var httpResponseMessage = await _departmentService.Create(new CreateDepartmentCommand
            {
                Name = Name,
                StoreId = _selectedStore.StoreId
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("CreateCompany", errorApi.Message, "Ok");
                
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Creación de Departamento", 
                    $"Se ha creado correctamente el departamento {Name}", 
                    "Ok");
            
                await _navigationService.GoBackAsync();   
            }
        }

        private async Task GetStores()
        {
            var httpResponseMessage= await _storeService.Get(new GetStoreCommand());
          
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetCompanies", errorApi.Message, "Ok");
            }
          
            var getStoresResponse = JsonConvert.DeserializeObject<GetStoresResponse>(respuesta);
            if (getStoresResponse != null)
                Stores = new ObservableCollection<Store>(getStoresResponse.Data);
        }
        
        public async Task GetDepartment(Guid departmentId)
        {
            var httpResponseMessageStores = await _departmentService.Get(new GetDepartmentsCommand()
            {
                DepartmentId = departmentId
            });
           
            var respuesta = await httpResponseMessageStores.Content.ReadAsStringAsync();
          
            if (httpResponseMessageStores.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetStore", errorApi.Message, "Ok");
            }
           
            var getDepartmantsResponse = JsonConvert.DeserializeObject<GetDepartmantsResponse>(respuesta);

            if (getDepartmantsResponse != null)
            {
                SelectedStore = Stores.FirstOrDefault(c => 
                    c.StoreId == getDepartmantsResponse.Data.FirstOrDefault().StoreId);
                Name = getDepartmantsResponse.Data.FirstOrDefault()?.Name;
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            DepartmentId = parameters.GetValue<Guid>("departmentId");
            if (DepartmentId!=Guid.Empty)
            {
                await GetDepartment(DepartmentId);
            }
        }
    }
}