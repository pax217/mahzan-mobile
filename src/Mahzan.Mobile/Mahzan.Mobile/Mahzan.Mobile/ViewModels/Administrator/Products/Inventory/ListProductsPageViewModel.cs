using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Products.Inventory
{
    public class ListProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        //private readonly IProductsService _productsService;

        private ObservableCollection<ListViewProducts> _listViewProducts { get; set; }
        public ObservableCollection<ListViewProducts> ListViewProducts
        {
            get => _listViewProducts;
            set
            {
                _listViewProducts = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewProducts)));
            }
        }

        public ICommand AddProductCommand { get; private set; }
        public ICommand ScannCommand { get; private set; }

        private ListViewProducts _selectedProduct { get; set; }

        public ListViewProducts SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    HandleSelectedProduct();
                }
            }
        }

        private void HandleSelectedProduct()
        {
            var navigationParams = new NavigationParameters();
            navigationParams.Add("productsId", SelectedProduct.ProductsId);
            _navigationService.NavigateAsync("AddProductPage", navigationParams);
        }

        public ListProductsPageViewModel(
            INavigationService navigationService/*,
            IProductsService productsService*/)
            :base(navigationService)
        {
            //
            _navigationService = navigationService;

            //_productsService = productsService;

            //Services
            Task.Run(() => GetProducts());

            //Comands
            AddProductCommand = new Command(async () => await OnAddProductCommand());
            ScannCommand = new Command(async () => await OnScannCommand());
            //Events

            //
        }

        private async Task OnAddProductCommand()
        {
            await _navigationService.NavigateAsync("AddProductPage");
        }

        private async Task GetProducts()
        {
           /* GetProductsResult getProductsResult = await _productsService
                                                        .Get(new GetProductsFilter
                                                        {
                                                        });

            if (getProductsResult.IsValid)
            {
                ListViewProducts = new ObservableCollection<ListViewProducts>();

                foreach (var product in getProductsResult.Products)
                {
                    ImageSource imageSource = null;

                    if (product.ProductsPhotos != null)
                    {
                        var bytes = Convert.FromBase64String(product.ProductsPhotos.Base64);
                        imageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                    }

                    ListViewProducts.Add(new ListViewProducts
                    {
                        ProductsId = product.ProductsId,
                        Description = product.Description,
                        ImageSource = imageSource,
                        Price = product.Price
                    });

                }
            }
            */
        }

        private async Task OnScannCommand()
        {

            /*var scanner = DependencyService.Get<IQrScanningService>();
            var result = await scanner.ScanAsync();

            GetProductsResult getProductsResult = await _productsService
                                                        .Get(new GetProductsFilter
                                                        {
                                                            Barcode = result
                                                        });

            if (getProductsResult.IsValid)
            {
                ListViewProducts.Clear();
                ListViewProducts = new ObservableCollection<ListViewProducts>();

                foreach (var product in getProductsResult.Products)
                {
                    ImageSource imageSource = null;

                    if (product.ProductsPhotos != null)
                    {
                        var bytes = Convert.FromBase64String(product.ProductsPhotos.Base64);
                        imageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                    }

                    ListViewProducts.Add(new ListViewProducts
                    {
                        ProductsId = product.ProductsId,
                        Description = product.Description,
                        ImageSource = imageSource,
                        Price = product.Price
                    });

                }
            }
            else
            {
                ListViewProducts.Clear();
                await Application.Current.MainPage.DisplayAlert("Producto No Encontrado",
                                                                string.Format("El producto con código de barras {0} no ha sido encontrado.", result),
                                                                "ok");
            }*/
        }
    }

    public class ListViewProducts
    {
        public Guid ProductsId { get; set; }
        public string Description { get; set; }
        public ImageSource ImageSource { get; set; }
        public decimal Price { get; set; }
    }
}