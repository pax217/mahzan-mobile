using System.Collections.ObjectModel;
using Mahzan.Mobile.Models.Menu.Products;
using Prism.Navigation;

namespace Mahzan.Mobile.ViewModels.Administrator.Products
{
    public class IndexProductsPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<ProductsOptions> ListProductsOptionsItems { get; set; }

        private ProductsOptions _selectedProductOptions { get; set; }

        public ProductsOptions SelectedProductOptions
        {
            get
            {
                return _selectedProductOptions;
            }
            set
            {
                if (_selectedProductOptions != value)
                {
                    _selectedProductOptions = value;
                    HandleSelectedWorkEnviromentOptions();
                }
            }
        }

        public IndexProductsPageViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;

            ListProductsOptionsItems = new ObservableCollection<ProductsOptions>()
            {
                new ProductsOptions(){ Option ="Inventario",OptionDetail="Agrega Productos, Administra tu Inventario"},
                new ProductsOptions(){ Option ="Categorías",OptionDetail="Administra tus Categorías de Producto"},
                new ProductsOptions(){ Option ="Unidades de Venta",OptionDetail="Administra tus Unidades de Venta"},
                new ProductsOptions(){ Option ="Impuestos",OptionDetail="Administra los impuestos."},
                //new ProductsOptions(){ Option ="Descuentos",OptionDetail="Crea descuentos para Productos"},

            };
        }

        public void HandleSelectedWorkEnviromentOptions()
        {
            switch (_selectedProductOptions.Option)
            {
                case "Inventario":
                    _navigationService.NavigateAsync("ListProductsPage");
                    break;
                case "Categorías":
                    _navigationService.NavigateAsync("ListCategoriesPage");
                    break;
                case "Unidades de Venta":
                    _navigationService.NavigateAsync("ListUnitsPage");
                    break;
                case "Impuestos":
                    _navigationService.NavigateAsync("ListTaxesPage");
                    break;
                default:
                    break;
            }

        }
    }
}