using System.Collections.ObjectModel;
using Mahzan.Mobile.Models.Menu.WorkEnviroment;
using Prism.Navigation;

namespace Mahzan.Mobile.ViewModels.Administrator.WorkEnviroment
{
    public class IndexWorkEnviromentPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<WorkEnviromentOptions> ListWorkEnviromentOptionItems { get; set; }

        private WorkEnviromentOptions _selectedWorkEnviromentOptions { get; set; }

        public WorkEnviromentOptions SelectedWorkEnviromentOptions
        {
            get
            {
                return _selectedWorkEnviromentOptions;
            }
            set
            {
                if (_selectedWorkEnviromentOptions!=value)
                {
                    _selectedWorkEnviromentOptions = value;
                    HandleSelectedWorkEnviromentOptions();
                }
            }
        }

        public IndexWorkEnviromentPageViewModel(
            INavigationService navigationService)
            :base(navigationService)
        {
            _navigationService = navigationService;

            ListWorkEnviromentOptionItems = new ObservableCollection<WorkEnviromentOptions>()
            {
                new WorkEnviromentOptions(){ Option ="Tiendas",OptionDetail="Administra tus Tiendas"},
                new WorkEnviromentOptions(){ Option ="TPVs",OptionDetail="Administra tus TPVs"},
            };
        }

        public void HandleSelectedWorkEnviromentOptions() 
        {
            switch (_selectedWorkEnviromentOptions.Option)
            {
                case "Tiendas":
                    _navigationService.NavigateAsync("ListStoresPage");
                    break;
                case "TPVs":
                    _navigationService.NavigateAsync("ListPointsOfSalesPage");
                    break;
                default:
                    break;
            }

        }
    }
}