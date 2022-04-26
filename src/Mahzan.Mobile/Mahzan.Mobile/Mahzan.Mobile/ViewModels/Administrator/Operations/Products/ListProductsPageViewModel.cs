using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Product;
using Mahzan.Mobile.Models.Product;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.QrScanning;
using Mahzan.Mobile.Services.Product;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Operations.Products
{
    public class ListProductsPageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly IProductsService _productsService;

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
            navigationParams.Add("productId", SelectedProduct.ProductId);
            _navigationService.NavigateAsync("AdminProductPage", navigationParams);
        }

        public ListProductsPageViewModel(
            INavigationService navigationService,
            IProductsService productsService)
        {

            _navigationService = navigationService;
            _productsService = productsService;

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
            await _navigationService.NavigateAsync("AdminProductPage");
        }

        private async Task GetProducts()
        {

            var httpResponseMessage = await _productsService.Get(new GetProductsCommand());
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await Application.Current.MainPage.DisplayAlert(
                    "Inicio de Sesión", errorApi.Message, "ok");

                return;
            }
            
            var getProductsResponse = JsonConvert.DeserializeObject<GetProductsResponse>(respuesta);

            if (getProductsResponse!=null)
            {
                await FillProducts(getProductsResponse);
            }
            
        }

        private async Task OnScannCommand()
        {
            var scanner = DependencyService.Get<IQrScanningService>();
            var barCode = await scanner.ScanAsync();
        
            var httpResponseMessage = await _productsService.Get(new GetProductsCommand
            {
                BarCode = barCode
            });
            
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                ListViewProducts.Clear();
                await Application.Current.MainPage.DisplayAlert("Producto No Encontrado",
                    string.Format("El producto con código de barras {0} no ha sido encontrado.", barCode),
                    "ok");
                return;
            }
            
            var getProductsResponse = JsonConvert.DeserializeObject<GetProductsResponse>(respuesta);
            if (getProductsResponse != null)
            {
                ListViewProducts.Clear();
                ListViewProducts = new ObservableCollection<ListViewProducts>();

                await FillProducts(getProductsResponse);
            }

        }

        private async Task FillProducts(GetProductsResponse getProductsResponse)
        {
            if (getProductsResponse!= null)
            {
                ListViewProducts = new ObservableCollection<ListViewProducts>();

                foreach (var product in getProductsResponse.Data)
                {
                    ImageSource imageSource = null;
 
                    if (product.Image != null)
                    {
                        var bytes = Convert.FromBase64String(product.Image);
                        imageSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                    }
 
                    ListViewProducts.Add(new ListViewProducts
                    {
                        ProductId = product.ProductId,
                        Description = product.Description,
                        ImageSource = imageSource,
                        Price = product.Price
                    });
                }
            }
        }

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }

    public class ListViewProducts
    {
        public Guid ProductId { get; set; }
        public string Description { get; set; }
        public ImageSource ImageSource { get; set; }
        
        public decimal Cost { get; set; }
        public decimal Price { get; set; }
    }
}