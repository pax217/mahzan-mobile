using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Category;
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
    public class ListCategoriesPageViewModel: BindableBase, INavigatedAware
    {
        private readonly INavigationService _navigationService;

        private readonly ICategoryService _categoryService;
        
        private ObservableCollection<Category> _listViewCategories { get; set; }
        public ObservableCollection<Category> ListViewCategories
        {
            get => _listViewCategories;
            set
            {
                _listViewCategories = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewCategories)));

            }
        }
        
        private Category _selectedCategory { get; set; }

        public Category SelectedCategory
        {
            get
            {
                return _selectedCategory;
            }
            set
            {
                if (_selectedCategory != value)
                {
                    _selectedCategory = value;
                    HandleCategory();
                }
            }
        }
        
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        
        public ICommand AddCategoryCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public ListCategoriesPageViewModel(
            INavigationService navigationService,
            ICategoryService categoryService)
        {
            _navigationService = navigationService;
            _categoryService = categoryService;
            
            Task.Run(GetCategories);
            
            RefreshCommand = new Command(async () => await OnRefreshCommand());
            AddCategoryCommand = new Command(async () => await OnAddCategoryCommand());

        }

        private async Task OnRefreshCommand()
        {
            IsRefreshing = true;

            await GetCategories();
            
            IsRefreshing = false;
        }
        
        private async Task OnAddCategoryCommand()
        {
            await _navigationService.NavigateAsync("AdminCategoryPage");
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
                ListViewCategories = new ObservableCollection<Category>(getCategoriesResponse.Data);   
        }

        private void HandleCategory()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("categoryId", SelectedCategory.CategoryId);
            _navigationService.NavigateAsync("AdminCategoryPage", navigationParams);
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}