using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.SubCategory;
using Mahzan.Mobile.Models.Category;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.SubCategory;
using Mahzan.Mobile.Services.SubCategory;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.SubCategories
{
    public class ListSubCategoriesPageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly ISubCategoryService _subCategoryService;
        
        private ObservableCollection<SubCategory> _listViewSubCategories { get; set; }
        public ObservableCollection<SubCategory> ListViewSubCategories
        {
            get => _listViewSubCategories;
            set
            {
                _listViewSubCategories = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewSubCategories)));
            }
        }
        
        private SubCategory _selectedSubCategory { get; set; }

        public SubCategory SelectedSubCategory
        {
            get
            {
                return _selectedSubCategory;
            }
            set
            {
                if (_selectedSubCategory != value)
                {
                    _selectedSubCategory = value;
                    HandleSubCategory();
                }
            }
        }
        
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
        }
        public ICommand AddSubCategoryCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        
        public ListSubCategoriesPageViewModel(
            INavigationService navigationService,
            ISubCategoryService subCategoryService)
        {
            _navigationService = navigationService;
            _subCategoryService = subCategoryService;
            
            Task.Run(GetSubCategories);
            
            AddSubCategoryCommand = new Command(async () => await OnAddSubCategoryCommand());
            RefreshCommand = new Command(async () => await OnRefreshCommand());

        }
        private async Task OnRefreshCommand()
        {
            IsRefreshing = true;

            await GetSubCategories();
            
            IsRefreshing = false;
        }
        private async Task GetSubCategories()
        {
            var httpResponseMessage = await _subCategoryService.Get(new GetSubCategoriesCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetCategories", errorApi.Message, "ok");

                return;  
            }
            var getSubCategoriesResponse = JsonConvert.DeserializeObject<GetSubCategoriesResponse>(respuesta);

            if (getSubCategoriesResponse != null)
                ListViewSubCategories = new ObservableCollection<SubCategory>(getSubCategoriesResponse.Data);   
        }
        
        private void HandleSubCategory()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("subCategoryId", SelectedSubCategory.SubCategoryId);
            _navigationService.NavigateAsync("AdminSubCategoryPage", navigationParams);
        }
        
        private async Task OnAddSubCategoryCommand()
        {
            await _navigationService.NavigateAsync("AdminSubCategoryPage");
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}