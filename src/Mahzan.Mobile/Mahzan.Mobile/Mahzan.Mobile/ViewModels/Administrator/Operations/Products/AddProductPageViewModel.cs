using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Category;
using Mahzan.Mobile.Commands.Department;
using Mahzan.Mobile.Commands.Product;
using Mahzan.Mobile.Commands.SubCategory;
using Mahzan.Mobile.Models.Category;
using Mahzan.Mobile.Models.Department;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.SubCategory;
using Mahzan.Mobile.QrScanning;
using Mahzan.Mobile.Services.Category;
using Mahzan.Mobile.Services.Department;
using Mahzan.Mobile.Services.Product;
using Mahzan.Mobile.Services.SubCategory;
using Mahzan.Mobile.Utils.Images;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toasts;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Operations.Products
{
    public class AddProductPageViewModel : ViewModelBase
    {
        #region Attributes

        private readonly INavigationService _navigationService;

        private readonly IPageDialogService _pageDialogService;

        private readonly IProductsService _productsService;

        private readonly IDepartmentService _departmentService;

        private readonly ICategoryService _categoryService;

        private readonly ISubCategoryService _subCategoryService;

        /*private readonly IProductCategoriesService _productCategoriesService;

        private readonly IProductUnitsService _productUnitsService;

        private readonly IProductsService _productsService;

        private readonly ITaxesService _taxesService;*/

        #endregion region 

        #region Properties

        private Guid? ProductsId { get; set; }

        //FollowInventory
        private bool _switchFollowInventory;
        public bool SwitchFollowInventory
        {
            get => _switchFollowInventory;
            set => SetProperty(ref _switchFollowInventory, value);
        }


        //Image
        private ImageSource _productImageSource;
        public ImageSource ProductImageSource
        {
            get => _productImageSource;
            set => SetProperty(ref _productImageSource, value);
        }

        //BarCode
        private string _barCode;
        public string BarCode
        {
            get => _barCode;
            set => SetProperty(ref _barCode, value);
        }

        //SKU
        private string _alternativeKey;
        public string AlternativeKey
        {
            get => _alternativeKey;
            set => SetProperty(ref _alternativeKey, value);
        }


        //Description

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        //Price
        private decimal? _price;
        public decimal? Price
        {
            get => _price;
            set => SetProperty(ref _price, value);
        }

        //Cost

        private decimal? _cost;
        public decimal? Cost
        {
            get => _cost;
            set => SetProperty(ref _cost, value);
        }
        
        // Departments
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
                    Task.Run(() => GetCategories());
                }
            }
        }

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
                    Task.Run(() => GetSubCategories());
                }
            }
        }
        
        //Sub Categories
        private ObservableCollection<SubCategory> _subCategories;
        public ObservableCollection<SubCategory> SubCategories
        {
            get => _subCategories;
            set => SetProperty(ref _subCategories, value);
        }
        private SubCategory _selectedSubCategory;
        public SubCategory SelectedSubCategory
        {
            get => _selectedSubCategory;
            set
            {
                if (_selectedSubCategory != value)
                {
                    _selectedSubCategory = value;
                }
            }
        }
        
        //Product Units
        private ObservableCollection<Category> _productUnits;
        public ObservableCollection<Category> ProductUnits
        {
            get => _productUnits;
            set => SetProperty(ref _productUnits, value);
        }
        //
        // //Taxes
        // private ObservableCollection<API.Entities.Taxes> _taxes;
        // public ObservableCollection<API.Entities.Taxes> Taxes
        // {
        //     get => _taxes;
        //     set => SetProperty(ref _taxes, value);
        // }


        //Selected ProductUnitCategory
        // private ProductCategories _selectedProductCategory;
        // public ProductCategories SelectedProductCategory
        // {
        //     get => _selectedProductCategory;
        //     set
        //     {
        //         if (_selectedProductCategory != value)
        //         {
        //             _selectedProductCategory = value;
        //             HandleSelectedProductCategory();
        //         }
        //     }
        // }

        //Selected ProductUnit
        // private ProductUnits _selectedProductUnit;
        // public ProductUnits SelectedProductUnit
        // {
        //     get => _selectedProductUnit;
        //     set
        //     {
        //         if (_selectedProductUnit != value)
        //         {
        //             _selectedProductUnit = value;
        //             HandleSelectedProductUnit();
        //         }
        //     }
        // }

        //Path Image Product

        private string PathImageProduct { get; set; }
        
        
        #endregion


        #region Commands

        public ICommand OpenCameraCommand { get; set; }
        public ICommand OpenBarCodeCommand { get; set; }
        public ICommand CreateProductCommand { get; set; }

        public ICommand ShowSnackCommand { get; set; }

        #endregion

        public AddProductPageViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            IProductsService productsService,
            IDepartmentService departmentService,
            ICategoryService categoryService,
            ISubCategoryService subCategoryService
            /*,
            IProductCategoriesService productCategoriesService,
            IProductUnitsService productUnitsService,
            IProductsService productsService, 
            ITaxesService taxesService*/):
            base(navigationService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            //Service
            _productsService = productsService;
            _departmentService = departmentService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            
            /*_productCategoriesService = productCategoriesService;
            _productUnitsService = productUnitsService;
            _productsService = productsService;
            _taxesService = taxesService;*/

            //Pickers
            Task.Run(() => GetDepartments());

            Task.Run(() => GetProductUnits());

            //List
            Task.Run(() => GetTaxes());

            //Commands
            OpenCameraCommand = new Command(async () => await OnOpenCameraCommand());
            OpenBarCodeCommand = new Command(async () => await OnOpenBarCodeCommand());
            CreateProductCommand = new Command(async () => await OnCreateProductCommand());

            //Initialize
            Initialize();

        }

        private async Task GetTaxes()
        {
            // GetTaxesResult result = await _taxesService.GetWhere(new GetTaxesFilter { 
            //
            // });
            //
            // if (result.IsValid)
            // {
            //     Taxes = new ObservableCollection<API.Entities.Taxes>(result.Taxes);
            // }
        }

        #region Private Methods
        private async Task OnCreateProductCommand()
        {
            
            var httpResponseMessage =await _productsService.Create(new CreateProductCommand
            {
                Photo = new CreateProductPhotoCommand
                {
                    Title = Path.GetFileNameWithoutExtension(PathImageProduct).Replace("_", ""),
                    MIMEType = ImagesUtil.GetMIMEType(Path.GetExtension(PathImageProduct)),
                    Base64 = ImagesUtil.ConvertImageBase64(PathImageProduct)
                },
                Detail = new CreateProductDetailCommand
                {
                    BarCode = BarCode,
                    AlternativeKey = AlternativeKey,
                    Description = Description,
                    Price = Price,
                    Cost = Cost,
                    FollowInventory = SwitchFollowInventory,
                    ProductSaleUnitId = "a6013276-7df0-44ed-9685-81ac3489af81"
                },
                Organization = new CreateProductOrganizationCommand
                {
                    DepartmentId = "6bbec42b-9471-4b58-9188-10e80c8b6d48",
                    CategoryId = "d241c704-6fe3-4019-83a9-8af13deaed2d",
                    SubCategoryId = "4ec243d1-4550-4936-9169-621cf720b5b9",
                }
            });
            

            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);

                await ShowCreateProductToast(Color.Red, "Crea producto",errorApi.Message);
                return;
            }

            await ShowCreateProductToast(
                Color.Green, 
                "Crea producto",
                string.Format("Se ha creado correctamente el producto {0}",Description));
            
        }
        

        private async Task GetProductUnits()
        {
            // GetProductUnitsResult getProductUnitsResult;
            // getProductUnitsResult = await _productUnitsService
            //                         .Get(new GetProductUnitsFilter
            //                         {
            //
            //                         });
            //
            // if (getProductUnitsResult.IsValid)
            // {
            //     ProductUnits = new ObservableCollection<ProductUnits>(getProductUnitsResult.ProductUnits);
            //
            // }
            // else
            // {
            //     await Application
            //           .Current
            //           .MainPage
            //           .DisplayAlert(getProductUnitsResult.Title,
            //                         getProductUnitsResult.Message,
            //                         "ok");
            // }

        }

        private void Initialize()
        {
            var noAvailableImage = new Image { Aspect = Aspect.AspectFit };
            noAvailableImage.Source = ImageSource.FromFile("image_no_available.png");

            ProductImageSource = noAvailableImage.Source;
        }

        private async Task OnOpenBarCodeCommand()
        {
            var scanner = Xamarin.Forms.DependencyService.Get<IQrScanningService>();
            var result = await scanner.ScanAsync();
            
            if (result != null)
            {
                BarCode = result;
            }
        }

        private async Task OnOpenCameraCommand()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application
                      .Current
                      .MainPage
                      .DisplayAlert("No Camera", "No Camera available", "Ok");
            }

            var file = await CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    SaveToAlbum = false
                }
            );

            if (file == null)
            {
                return;
            }
            else
            {
                PathImageProduct = file.AlbumPath;
            }

            ProductImageSource = ImageSource.FromStream(() => {
                var stream = file.GetStream();
                file.Dispose();
                return stream;
            }
            );
        }

        private async Task GetDepartments()
        {
            var httpResponseMessage = await _departmentService.Get(new GetDepartmentsCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "Inicio de Sesión", errorApi.Message, "ok");

                return;
            }
            
            var getDepartmentsResponse = JsonConvert.DeserializeObject<GetDepartmantsResponse>(respuesta);
            if (getDepartmentsResponse != null)
                Departments = new ObservableCollection<Department>(getDepartmentsResponse.Data);
        }

        private async Task GetCategories()
        {
           var httpResponseMessage= await _categoryService.Get(new GetCategoriesCommand
            {
                DepartmentId = _selectedDepartment.DepartmentId
            });
           
           var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
           if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
           {
               var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
               await Application.Current.MainPage.DisplayAlert(
                   "Inicio de Sesión", errorApi.Message, "ok");
               
           }
           
           var getCategoriesResponse = JsonConvert.DeserializeObject<GetCategoriesResponse>(respuesta);
           if (getCategoriesResponse != null)
               Categories = new ObservableCollection<Category>(getCategoriesResponse.Data);

        }
        
        private async Task GetSubCategories()
        {
            var httpResponseMessage = await _subCategoryService.Get(new GetSubCategoriesCommand
            {
                CategoryId = _selectedCategory.CategoryId
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "Inicio de Sesión", errorApi.Message, "ok");
               
            }
            
            var getSubCategoriesResponse = JsonConvert.DeserializeObject<GetSubCategoriesResponse>(respuesta);
            if (getSubCategoriesResponse != null)
                SubCategories = new ObservableCollection<SubCategory>(getSubCategoriesResponse.Data);
        }
        
        
        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            ProductsId = parameters.GetValue<Guid>("productsId");

            if (ProductsId !=Guid.Empty)
            {
                await GetProduct(ProductsId.Value);
            }
        }

        public async Task GetProduct(Guid productsId) 
        {
            // GetProductsResult result = await _productsService
            //                                  .Get(new GetProductsFilter
            //                                 {
            //                                    ProductsId = productsId
            //                                  });
            //
            // if (result.IsValid)
            // {
            //     API.Entities.Products product = result.Products.FirstOrDefault();
            //
            //     var bytes = Convert.FromBase64String(product.ProductsPhotos.Base64);
            //     ProductImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
            //
            //     SKU = product.SKU;
            //     BarCode = product.Barcode;
            //     Description = product.Description;
            //     SelectedProductCategory = ProductCategories
            //                               .SingleOrDefault(x => x.ProductCategoriesId == product.ProductCategoriesId);
            //     SelectedProductUnit = ProductUnits
            //                           .SingleOrDefault(x => x.ProductUnitsId == product.ProductUnitsId);
            //
            //     Price = product.Price;
            //     Cost = product.Cost;
            // }
        }
        
        
        //Notifications
        public async Task ShowCreateProductToast(Color color,string title, string description)
        {
            var notificator = DependencyService.Get<IToastNotificator>();
                
            var options = new NotificationOptions()
            {
                Title = title,
                Description = description,
                IsClickable = true, //make it false if you don't want the notification to be clickable
                AndroidOptions = new AndroidOptions()
                {
                    HexColor = color.ToHex(),
                    ForceOpenAppOnNotificationTap = true
                },
                // DelayUntil = DateTime.Now.AddSeconds(1)
            };
            
            await notificator.Notify(options);
            
            //var result = await notificator.Notify(options);

            // if(result.Action == NotificationAction.Clicked)
            // {
            //     await App.Current.MainPage.DisplayAlert("Alert", "Grab ID and tile and desc : " + result.Id + " " + options.Title, "Ok");
            // }
            //
        }

        #endregion
    }
}