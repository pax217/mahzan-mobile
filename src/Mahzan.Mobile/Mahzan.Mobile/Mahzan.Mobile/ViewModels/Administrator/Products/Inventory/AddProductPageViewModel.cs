using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Product;
using Mahzan.Mobile.Utils.Images;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Products.Inventory
{
    public class AddProductPageViewModel : BindableBase, INavigationAware
    {
        #region Attributes

        private readonly INavigationService _navigationService;

        private readonly IPageDialogService _pageDialogService;

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
        private string _sku;
        public string SKU
        {
            get => _sku;
            set => SetProperty(ref _sku, value);
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

        // //Categories
        // private ObservableCollection<ProductCategories> _productCategories;
        // public ObservableCollection<ProductCategories> ProductCategories
        // {
        //     get => _productCategories;
        //     set => SetProperty(ref _productCategories, value);
        // }
        //
        // //Product Units
        // private ObservableCollection<ProductUnits> _productUnits;
        // public ObservableCollection<ProductUnits> ProductUnits
        // {
        //     get => _productUnits;
        //     set => SetProperty(ref _productUnits, value);
        // }
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


        #endregion

        public AddProductPageViewModel(
            INavigationService navigationService,
            IPageDialogService pageDialogService/*,
            IProductCategoriesService productCategoriesService,
            IProductUnitsService productUnitsService,
            IProductsService productsService, 
            ITaxesService taxesService*/)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;

            //Service
            /*_productCategoriesService = productCategoriesService;
            _productUnitsService = productUnitsService;
            _productsService = productsService;
            _taxesService = taxesService;*/

            //Pickers
            Task.Run(() => GetProductCategories());
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
            CreateProductCommand command = new CreateProductCommand
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
                    AlternativeKey = String.Empty,
                    Description = Description,
                    Price = Price,
                    Cost = Cost,
                    FollowInventory = SwitchFollowInventory
                },
                Organization = new CreateProductOrganizationCommand
                {
                    
                }
            };

            // CreateProductCommand command = new CreateProductCommand
            // {
            //     CreateProductDetailCommand = new CreateProductDetailCommand
            //     {
            //         ProductCategoriesId = _selectedProductCategory.ProductCategoriesId,
            //         ProductUnitsId = _selectedProductUnit.ProductUnitsId,
            //         SKU = SKU,
            //         Barcode = BarCode,
            //         Description = Description,
            //         Price = Price.Value,
            //         Cost = Cost,
            //         FollowInventory = SwitchFollowInventory
            //     },
            //     CreateProductPhotoCommand = new CreateProductPhotoCommand
            //     {
            //         Title = Path.GetFileNameWithoutExtension(PathImageProduct).Replace("_", ""),
            //         MIMEType = ImagesUtil.GetMIMEType(Path.GetExtension(PathImageProduct)),
            //         Base64 = ImagesUtil.ConvertImageBase64(PathImageProduct)
            //     },
            // };
            //
            // //Taxes
            // if (Taxes.Where(x=> x.Active).ToList().Count>0)
            // {
            //     command.CreateProductTaxesCommand = new List<CreateProductTaxesCommand>(); 
            //
            //     foreach (var tax in Taxes.Where(x => x.Active))
            //     {
            //         command
            //             .CreateProductTaxesCommand
            //             .Add(new CreateProductTaxesCommand
            //             {
            //             TaxRate = tax.TaxRatePercentage,
            //             TaxesId = tax.TaxesId
            //             });
            //     }
            // }
            //
            // if (ProductsId == Guid.Empty)
            // {
            //     //Insert
            //     CreateProductResult createProductResult;
            //     createProductResult = await _productsService
            //                                .CreateProduct(command);
            //
            //     if (createProductResult.IsValid)
            //     {
            //         //Si el producto se sigue en el inventario
            //         if (SwitchFollowInventory)
            //         {
            //             var navigationParams = new NavigationParameters();
            //             navigationParams.Add("productsId", createProductResult.ProductsId);
            //             await _navigationService.NavigateAsync("AddProductInventoryPage", navigationParams);
            //
            //         }
            //         else 
            //         {
            //             await _pageDialogService
            //                 .DisplayAlertAsync(
            //                 createProductResult.Title,
            //                 createProductResult.Message,
            //                 "Ok");
            //         }
            //     }
            //     else
            //     {
            //         await _pageDialogService
            //             .DisplayAlertAsync(
            //             createProductResult.Title,
            //             createProductResult.Message,
            //             "Ok");
            //     }
            // }
            // else 
            // {
            //     var navigationParams = new NavigationParameters();
            //     navigationParams.Add("productsId", ProductsId.Value);
            //
            //     await _navigationService.NavigateAsync("AddProductInventoryPage", navigationParams);
            // }




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
            // var scanner = Xamarin.Forms.DependencyService.Get<IQrScanningService>();
            // var result = await scanner.ScanAsync();
            //
            // if (result != null)
            // {
            //     BarCode = result;
            // }
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

        private async Task GetProductCategories()
        {
            // GetProductCategoriesResult getProductCategoriesResult;
            // getProductCategoriesResult = await _productCategoriesService
            //                                    .Get(new GetProductCategoriesFilter
            //                                    {
            //
            //                                    });
            //
            // if (getProductCategoriesResult.IsValid)
            // {
            //     ProductCategories = new ObservableCollection<ProductCategories>(getProductCategoriesResult.ProductCategories);
            //
            // }
            // else
            // {
            //     await Application
            //           .Current
            //           .MainPage
            //           .DisplayAlert(getProductCategoriesResult.Title,
            //                         getProductCategoriesResult.Message,
            //                         "ok");
            // }
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

        #endregion
    }
}