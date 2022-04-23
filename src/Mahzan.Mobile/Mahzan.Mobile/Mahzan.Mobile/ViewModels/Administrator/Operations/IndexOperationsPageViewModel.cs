using System.Collections.ObjectModel;
using Mahzan.Mobile.Models.Menu.Operations;
using Prism.Navigation;

namespace Mahzan.Mobile.ViewModels.Administrator.Operations
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
                    Option ="Productos",
                    OptionDetail="Fotos,Código,Descripción,Costo,Precio,Inventario"
                },
            };
        }

        public void HandleSelectedWorkEnviromentOptions()
        {
            switch (_selectedOpetationOptions.Option)
            {
                case "Productos":
                    _navigationService.NavigateAsync("ListProductsPage");
                    break;
                default:
                    break;
            }

        }
    }
}