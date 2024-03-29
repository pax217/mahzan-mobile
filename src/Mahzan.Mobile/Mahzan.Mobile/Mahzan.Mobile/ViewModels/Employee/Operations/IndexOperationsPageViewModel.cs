using System.Collections.ObjectModel;
using Mahzan.Mobile.Models.Menu.Operations;
using Prism.Navigation;

namespace Mahzan.Mobile.ViewModels.Employee.Operations
{
    public class IndexOperationsPageViewModel: ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public ObservableCollection<OperationsOptions> ListOperationsOptionsItems { get; set; }
        private OperationsOptions _selectedOpetationOptions { get; set; }

        public OperationsOptions SelectedOperationOptions
        {
            get
            {
                return _selectedOpetationOptions;
            }
            set
            {
                if (_selectedOpetationOptions != value)
                {
                    _selectedOpetationOptions = value;
                    HandleSelectedWorkEnviromentOptions();
                }
            }
        }
        public IndexOperationsPageViewModel(
            INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;

            ListOperationsOptionsItems = new ObservableCollection<OperationsOptions>()
            {
                new OperationsOptions()
                {
                    Option ="Abrir/Cerrar Punto de Venta",
                    OptionDetail="Abrir, Cerra, Billetes, Monedas"
                },
                new OperationsOptions()
                {
                    Option ="Ventas",
                    OptionDetail="Vender"
                },
            };
        }
        
        public void HandleSelectedWorkEnviromentOptions()
        {
            switch (_selectedOpetationOptions.Option)
            {
                case "Abrir/Cerrar Punto de Venta":
                    _navigationService.NavigateAsync("AdminPointSaleStatePage");
                    break;
                case "Ventas":
                    _navigationService.NavigateAsync("ListSalesPage");
                    break;
                default:
                    break;
            }

        }
    }
}