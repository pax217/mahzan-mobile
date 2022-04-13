using System.Collections.ObjectModel;
using Mahzan.Mobile.Models.Settings;
using Prism.Mvvm;
using Prism.Navigation;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings
{
    public class IndexSettingsPageViewModel : BindableBase
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<SettingsOptions> ListSettingsOptionsItem { get; set; }

        private SettingsOptions _selectedSettingOption { get; set; }

        public SettingsOptions SelectedEnviromentOptions
        {
            get
            {
                return _selectedSettingOption;
            }
            set
            {
                if (_selectedSettingOption != value)
                {
                    _selectedSettingOption = value;
                    HandleSelectedSettingOption();
                }
            }
        }

        private void HandleSelectedSettingOption()
        {
            switch (_selectedSettingOption.Option)
            {
                case "Compañias":
                    _navigationService.NavigateAsync("ListCompaniesPage");
                    break;
                case "Tickets":
                    _navigationService.NavigateAsync("TicketsSettingsPage");
                    break;
                case "Impresora":
                    _navigationService.NavigateAsync("SelectPrinterPage");
                    break;
                case "Perfil":
                    _navigationService.NavigateAsync("AdminProfilePage");
                    break;
                default:
                    break;
            }
        }

        public IndexSettingsPageViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;

            ListSettingsOptionsItem = new ObservableCollection<SettingsOptions>()
            {
                new SettingsOptions(){ Option ="Compañias",OptionDetail="Administra tus compañias."},
                new SettingsOptions(){ Option ="Tiendas",OptionDetail="Administra tus tiendas."},
                new SettingsOptions(){ Option ="Puntos de Venta",OptionDetail="Administra tus puntos de venta."},
                new SettingsOptions(){ Option ="Departamentos",OptionDetail="Administra tus Departamentos."},
                new SettingsOptions(){ Option ="Categorías",OptionDetail="Administra tus Categoriías."},
                new SettingsOptions(){ Option ="SubCategorías",OptionDetail="Administra tus Sub categorías."},
                new SettingsOptions(){ Option ="Empleados",OptionDetail="Administra tus Empleados."},
                new SettingsOptions(){ Option ="Tickets",OptionDetail="Configura la forma en que se imprimirán tus tickets."},
                new SettingsOptions(){ Option ="Impresora",OptionDetail="Selecciona tu impresora bluetooth."},
                new SettingsOptions(){ Option ="Perfil",OptionDetail="Administra la información de tu perfil de usuario."},
            };
        }
    }

}