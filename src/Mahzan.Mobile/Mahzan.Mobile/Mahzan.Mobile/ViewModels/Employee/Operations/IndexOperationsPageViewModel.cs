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
                    Option ="Abrir/Cerrar Caja",
                    OptionDetail="Abrir, Cerra, Billetes, Monedas"
                },
            };
        }
        
        public void HandleSelectedWorkEnviromentOptions()
        {
            switch (_selectedOpetationOptions.Option)
            {
                case "Abrir/Cerrar Caja":
                    _navigationService.NavigateAsync("ListPage");
                    break;
                default:
                    break;
            }

        }
    }
}