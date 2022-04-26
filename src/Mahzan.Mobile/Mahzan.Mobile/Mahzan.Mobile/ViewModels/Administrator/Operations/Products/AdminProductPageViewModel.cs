using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using ImTools;
using Mahzan.Mobile.Commands.Category;
using Mahzan.Mobile.Commands.Department;
using Mahzan.Mobile.Commands.Product;
using Mahzan.Mobile.Commands.SubCategory;
using Mahzan.Mobile.Commands.Tax;
using Mahzan.Mobile.Commands.UnitSale;
using Mahzan.Mobile.Models.Category;
using Mahzan.Mobile.Models.Department;
using Mahzan.Mobile.Models.Product;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.SubCategory;
using Mahzan.Mobile.Models.Tax;
using Mahzan.Mobile.Models.UnitSale;
using Mahzan.Mobile.QrScanning;
using Mahzan.Mobile.Services.Category;
using Mahzan.Mobile.Services.Department;
using Mahzan.Mobile.Services.Product;
using Mahzan.Mobile.Services.SubCategory;
using Mahzan.Mobile.Services.Tax;
using Mahzan.Mobile.Services.UnitsSale;
using Mahzan.Mobile.Utils.Images;
using Newtonsoft.Json;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Toasts;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Operations.Products
{
    public class AdminProductPageViewModel : BindableBase, INavigationAware
    {
        #region Attributes

        private readonly INavigationService _navigationService;
        private readonly IPageDialogService _pageDialogService;
        private readonly IProductsService _productsService;
        private readonly IDepartmentService _departmentService;
        private readonly ICategoryService _categoryService;
        private readonly ISubCategoryService _subCategoryService;
        private readonly ITaxService _taxService;
        private readonly IUnitSaleService _unitSaleService;

        /*private readonly IProductCategoriesService _productCategoriesService;

        private readonly IProductUnitsService _productUnitsService;

        private readonly IProductsService _productsService;

        private readonly ITaxesService _taxesService;*/

        #endregion region 

        #region Properties

        private Guid ProductId { get; set; }

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
        
        //Units Sale
        private ObservableCollection<UnitSale> _unitsSale;
        public ObservableCollection<UnitSale> UnitsSale
        {
            get => _unitsSale;
            set => SetProperty(ref _unitsSale, value);
        }
        private UnitSale _selectedUnitSale;
        public UnitSale SelectedUnitSale
        {
            get => _selectedUnitSale;
            set
            {
                if (_selectedUnitSale != value)
                {
                    _selectedUnitSale = value;
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
        
        //Taxes
        private ObservableCollection<Tax> _taxes;
        public ObservableCollection<Tax> Taxes
        {
            get => _taxes;
            set => SetProperty(ref _taxes, value);
        }
        

        //Path Image Product

        private string PathImageProduct { get; set; }
        
        
        #endregion


        #region Commands

        public ICommand OpenCameraCommand { get; set; }
        public ICommand OpenBarCodeCommand { get; set; }
        public ICommand SaveProductCommand { get; set; }
        
        public ICommand DeleteProductCommand { get; set; }
        

        #endregion

        public AdminProductPageViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService,
            IProductsService productsService,
            IDepartmentService departmentService,
            ICategoryService categoryService,
            ISubCategoryService subCategoryService,
            ITaxService taxService,
            IUnitSaleService unitSaleService) 

        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            //Service
            _productsService = productsService;
            _departmentService = departmentService;
            _categoryService = categoryService;
            _subCategoryService = subCategoryService;
            _taxService = taxService;
            _unitSaleService = unitSaleService;

            //List
            Task.Run(GetTaxes);

            //Pickers
            Task.Run(GetUnitsSale);
            Task.Run(GetProductUnits);
            Task.Run(GetDepartments);

            //Commands
            OpenCameraCommand = new Command(async () => await OnOpenCameraCommand());
            OpenBarCodeCommand = new Command(async () => await OnOpenBarCodeCommand());
            SaveProductCommand = new Command(async () => await OnSaveProductCommand());
            DeleteProductCommand = new Command(async () => await OnDeleteProductCommand());
            
            Initialize();
        }

        private async Task OnDeleteProductCommand()
        {
            var answer = await Application
                .Current
                .MainPage
                .DisplayAlert("Atención!",
                    "¿Estas seguro de borrar el producto?", "Si", "No");
            
            if (answer)
            {
                var httpResponseMessage = await _productsService.Delete(ProductId.ToString());
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert(
                        "OnDeleteProductCommand", 
                        errorApi.Message, 
                        "Ok");
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Elimina producto", 
                        $"Se ha elimindo correctamente el Producto {Description}", 
                        "Ok");

                    await _navigationService.GoBackAsync();
                }
            }
        }

        private async Task GetUnitsSale()
        {
            var httpResponseMessage = await _unitSaleService.Get(new GetUnitsSaleCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetUnitsSale", errorApi.Message, "ok");
                
            }
            var getUnitsSaleResponse = JsonConvert.DeserializeObject<GetUnitsSaleResponse>(respuesta);

            if (getUnitsSaleResponse != null)
                UnitsSale = new ObservableCollection<UnitSale>(getUnitsSaleResponse.Data);
        }

        private async Task GetTaxes()
        {
            var httpResponseMessage = await _taxService.Get(new GetTaxCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();

            if (httpResponseMessage.StatusCode!= HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "GetTaxes", errorApi.Message, "ok");
                
            }
            var getTaxesResponse = JsonConvert.DeserializeObject<GetTaxesResponse>(respuesta);

            if (getTaxesResponse != null)
                Taxes = new ObservableCollection<Tax>(getTaxesResponse.Data);
        }

        #region Private Methods
        private async Task OnSaveProductCommand()
        {
            if (ProductId==Guid.Empty)
            {
                await CreateProduct();
            }
            else
            {
                
            }

        }

        private async Task CreateProduct()
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
                    FollowInventory = false,
                    UnitSaleId = _selectedUnitSale.UnitSaleId.ToString(),
                    TaxesIds = Taxes
                        .Where(t => t.Active == true)
                        .Select(x => x.TaxId).ToList()
                },
                Organization = new CreateProductOrganizationCommand
                {
                    DepartmentId = _selectedDepartment.DepartmentId.ToString(),
                    CategoryId = _selectedCategory.CategoryId.ToString(),
                    SubCategoryId = _selectedSubCategory.SubCategoryId.ToString(),
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
            ProductId = parameters.GetValue<Guid>("productId");

            if (ProductId !=Guid.Empty)
            {
                await GetProduct(ProductId);
            }
        }

        public async Task GetProduct(Guid productsId) 
        {
            var httpResponseMessage= await _productsService.Get(new GetProductsCommand()
            {
                ProductId = productsId.ToString()
            });
          
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetProduct", errorApi.Message, "Ok");
            }
          
            var getProductsResponse = JsonConvert.DeserializeObject<GetProductsResponse>(respuesta);
            if (getProductsResponse != null)
            {
                var product = getProductsResponse.Data.FirstOrDefault();
                if (product != null)
                {
                    var bytes = Convert.FromBase64String(product.Image);
                    ProductImageSource = ImageSource.FromStream(() => new MemoryStream(bytes));

                    AlternativeKey = product.AlternativeKey;
                    BarCode = product.BarCorde;
                    Description = product.Description;
                    Price = product.Price;
                    Cost = product.Cost;
                }
            }
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