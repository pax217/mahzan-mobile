using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Product;
using Mahzan.Mobile.Models.Product;
using Mahzan.Mobile.Models.Ticket;
using Mahzan.Mobile.QrScanning;
using Mahzan.Mobile.Services.Product;
using Mahzan.Mobile.ViewModels.Administrator.Operations.Products;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Employee.Operations.Sales
{
    public class NewSalePageViewModel: BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;
        
        private readonly IProductsService _productsService;
        
        private List<TicketDetail> ListTicketDetail = new List<TicketDetail>();
        
        private ObservableCollection<ListViewTicketDetail> _listViewTicketDetail { get; set; }

        public ObservableCollection<ListViewTicketDetail> ListViewTicketDetail
        {
            get => _listViewTicketDetail;
            set
            {
                _listViewTicketDetail = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ListViewTicketDetail)));
            }
        }

        
        public ICommand ReadBarCodeCommand { get; private set; }

        public NewSalePageViewModel(
            INavigationService navigationService, 
            IProductsService productsService)
        {
            _navigationService = navigationService;
            _productsService = productsService;

            ReadBarCodeCommand = new Command(async () => await OnReadBarCodeCommand());
            
            ListViewTicketDetail = new ObservableCollection<ListViewTicketDetail>();
        }

        private async Task OnReadBarCodeCommand()
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
                await Application.Current.MainPage.DisplayAlert("Producto No Encontrado",
                    string.Format("El producto con código de barras {0} no ha sido encontrado.", barCode),
                    "ok");
                return;
            }
            
            var getProductsResponse = JsonConvert.DeserializeObject<GetProductsResponse>(respuesta);

            if (getProductsResponse != null)
                AddProductToTicket(getProductsResponse.Data[0]);
        }

        private void AddProductToTicket(Product product)
        {
            TicketDetail existingProduct = (from td in ListTicketDetail
                    where td.ProductId == product.ProductId
                    select td)
                .FirstOrDefault();

            if (existingProduct==null)
            {
                ListTicketDetail.Add(new TicketDetail
                    {
                        TicketDetailId = Guid.NewGuid(),
                        ProductId = product.ProductId,
                        Quantity = 1,
                        Description = product.Description,
                        Price = product.Price,
                        Amount = product.Price
                    }
                );
            }
            else
            {
                existingProduct.Quantity++;
            }

        }
        

        public async void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {

        }
    }
}