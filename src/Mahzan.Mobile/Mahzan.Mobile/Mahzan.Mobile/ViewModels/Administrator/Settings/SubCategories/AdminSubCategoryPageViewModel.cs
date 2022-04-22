using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Category;
using Mahzan.Mobile.Commands.SubCategory;
using Mahzan.Mobile.Models.Category;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.SubCategory;
using Mahzan.Mobile.Services.Category;
using Mahzan.Mobile.Services.SubCategory;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.SubCategories
{
    public class AdminSubCategoryPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        
        private readonly ICategoryService _categoryService;

        private readonly ISubCategoryService _subCategoryService;
        
        //Categories
        private ObservableCollection<Category> _categories;
        public ObservableCollection<Category> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
        private Category _selectedCategory;
        public Category SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedCategory))); // Notify that there was a change on this property
            }
        }
        public Guid SubCategoryId { get; set; }
        
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
        
        public ICommand DeleteSubCategoryCommand { get; set; }
        public ICommand SaveSubCategoryCommand { get; set; }
        
        public AdminSubCategoryPageViewModel(
            INavigationService navigationService,
            ICategoryService categoryService,
            ISubCategoryService subCategoryService)
        {
            _navigationService = navigationService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            
            Task.Run(GetCategories);
            
            SaveSubCategoryCommand = new Command(async () => await OnSaveSubCategoryCommand());
            
            DeleteSubCategoryCommand = new Command(async () => await OnDeleteSubCategoryCommand());

        }
        
        private async Task OnSaveSubCategoryCommand()
        {
            if (SubCategoryId==Guid.Empty)
            {
                await CreateSubCategory();
            }
            else
            {
                await UpdateSubCategory();
            }
        }
        
        private async Task OnDeleteSubCategoryCommand()
        {
            await DeleteCategory();
        }
        
        private async Task CreateSubCategory()
        {
            var httpResponseMessage = await _subCategoryService.Create(new CreateSubCategoryCommand
            {
                Name = Name,
                CategoryId = _selectedCategory.CategoryId
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("CreateSubCategory", errorApi.Message, "Ok");
                
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Creación de Sub Categoría", 
                    $"Se ha creado correctamente la sub categoría {Name}", 
                    "Ok");
            
                await _navigationService.GoBackAsync();   
            }
        }
        
        private async Task UpdateSubCategory()
        {
            var httpResponseMessage = await _subCategoryService.Update(new UpdateSubCategoryCommand
            {
                SubCategoryId = SubCategoryId,
                Name = Name,
                CategoryId = _selectedCategory.CategoryId,
            });
                
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("UpdateCategory", errorApi.Message, "Ok");
            }
                
            await App.Current.MainPage.DisplayAlert(
                "Actualización de Sub Categoría", 
                $"Se ha actualizado correctamenta la sub categoría {Name}", 
                "Ok");

            await _navigationService.GoBackAsync();
        }
        
        private async Task DeleteCategory()
        {
            var answer = await Application
                .Current
                .MainPage
                .DisplayAlert("Atención!",
                    "¿Estas seguro de borrar la sub categoría?", "Si", "No");
            
            if (answer)
            {
                var httpResponseMessage = await _subCategoryService.Delete(SubCategoryId.ToString());
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert(
                        "DeleteCategory", 
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
        
        private async Task GetCategories()
        {
            var httpResponseMessage = await _categoryService.Get(new GetCategoriesCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetCategories", errorApi.Message, "ok");

                return;  
            }
            var getCategoriesResponse = JsonConvert.DeserializeObject<GetCategoriesResponse>(respuesta);

            if (getCategoriesResponse != null)
                Categories = new ObservableCollection<Category>(getCategoriesResponse.Data);
        }
        
        public async Task GetSubCategory(Guid subCategoryId)
        {
            var httpResponseMessageStores = await _subCategoryService.Get(new GetSubCategoriesCommand()
            {
                SubCategoryId = subCategoryId
            });
           
            var respuesta = await httpResponseMessageStores.Content.ReadAsStringAsync();
          
            if (httpResponseMessageStores.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetCategory", errorApi.Message, "Ok");
            }
           
            var getSubCategoriesResponse = JsonConvert.DeserializeObject<GetSubCategoriesResponse>(respuesta);

            if (getSubCategoriesResponse != null)
            {
                SelectedCategory = Categories.FirstOrDefault(c => 
                    c.CategoryId == getSubCategoriesResponse.Data.FirstOrDefault().CategoryId);
                Name = getSubCategoriesResponse.Data.FirstOrDefault()?.Name;
            }
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            SubCategoryId = parameters.GetValue<Guid>("subCategoryId");
            if (SubCategoryId!=Guid.Empty)
            {
                await GetSubCategory(SubCategoryId);
            }
        }
    }
}