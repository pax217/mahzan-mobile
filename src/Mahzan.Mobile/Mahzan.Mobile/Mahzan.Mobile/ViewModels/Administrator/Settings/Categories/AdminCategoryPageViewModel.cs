using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Category;
using Mahzan.Mobile.Commands.Department;
using Mahzan.Mobile.Models.Category;
using Mahzan.Mobile.Models.Department;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.Category;
using Mahzan.Mobile.Services.Department;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Categories
{
    public class AdminCategoryPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IDepartmentService _departmentService;

        private readonly ICategoryService _categoryService;

        //Stores
        private ObservableCollection<Department> _departments;
        public ObservableCollection<Department> Departments
        {
            get => _departments;
            set => SetProperty(ref _departments, value);
        }
        private Department _selectedDepartment;
        public Department SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                if (_selectedDepartment != value)
                {
                    _selectedDepartment = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedDepartment))); // Notify that there was a change on this property
            }
        }
        
        public Guid CategoryId { get; set; }
        
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

        public ICommand DeleteCategoryCommand { get; set; }
        public ICommand SaveCategoryCommand { get; set; }

        public AdminCategoryPageViewModel(
            INavigationService navigationService,
            IDepartmentService departmentService,
            ICategoryService categoryService)
        {
            _navigationService = navigationService;
            _departmentService = departmentService;
            _categoryService = categoryService;

            Task.Run(GetDepartments);
            
            SaveCategoryCommand = new Command(async () => await OnSaveCategoryCommand());
            
            DeleteCategoryCommand = new Command(async () => await OnDeleteCategoryCommand());

        }

        private async Task OnSaveCategoryCommand()
        {
            if (CategoryId==Guid.Empty)
            {
                await CreateCategory();
            }
            else
            {
                await UpdateCategory();
            }
        }

        private async Task CreateCategory()
        {
            var httpResponseMessage = await _categoryService.Create(new CreateCategoryCommand
            {
                Name = Name,
                DepartmentId = _selectedDepartment.DepartmentId
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
                    "Creación de Categoría", 
                    $"Se ha creado correctamente la categoría {Name}", 
                    "Ok");
            
                await _navigationService.GoBackAsync();   
            }
        }

        private async Task UpdateCategory()
        {
            var httpResponseMessage = await _categoryService.Update(new UpdateCategoryCommand
            {
                CategoryId = CategoryId,
                Name = Name,
                DepartmentId = _selectedDepartment.DepartmentId,
            });
                
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("UpdateCategory", errorApi.Message, "Ok");
            }
                
            await App.Current.MainPage.DisplayAlert(
                "Actualización de Categoría", 
                $"Se ha actualizado correctamenta la categoría {Name}", 
                "Ok");

            await _navigationService.GoBackAsync();
        }
        
        private async Task DeleteCategory()
        {
            var answer = await Application
                .Current
                .MainPage
                .DisplayAlert("Atención!",
                    "¿Estas seguro de borrar la categoría?", "Si", "No");
            
            if (answer)
            {
                var httpResponseMessage = await _categoryService.Delete(CategoryId.ToString());
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert(
                        "DeleteDepartment", 
                        errorApi.Message, 
                        "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Elimina categoría", 
                        $"Se ha elimindo correctamente la categoría {Name}", 
                        "Ok");

                    await _navigationService.GoBackAsync();
                }
            }
        }

        private async Task OnDeleteCategoryCommand()
        {
            await DeleteCategory();
        }

        private async Task GetDepartments()
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
                Departments = new ObservableCollection<Department>(getDepartmentsResponse.Data);
        }
        
        public async Task GetCategory(Guid categoryId)
        {
            var httpResponseMessageStores = await _categoryService.Get(new GetCategoriesCommand
            {
                CategoryId = categoryId
            });
           
            var respuesta = await httpResponseMessageStores.Content.ReadAsStringAsync();
          
            if (httpResponseMessageStores.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetCategory", errorApi.Message, "Ok");
            }
           
            var getCategoriesResponse = JsonConvert.DeserializeObject<GetCategoriesResponse>(respuesta);

            if (getCategoriesResponse != null)
            {
                SelectedDepartment = Departments.FirstOrDefault(c => 
                    c.DepartmentId == getCategoriesResponse.Data.FirstOrDefault().DepartmentId);
                Name = getCategoriesResponse.Data.FirstOrDefault()?.Name;
            }
        }

        public  void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            CategoryId = parameters.GetValue<Guid>("categoryId");
            if (CategoryId!=Guid.Empty)
            {
                await GetCategory(CategoryId);
            }
        }
    }
}