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
                case "Tiendas":
                    _navigationService.NavigateAsync("ListStoresPage");
                    break;
                case "Departamentos":
                    _navigationService.NavigateAsync("ListDepartmentsPage");
                    break;
                case "Categorías":
                    _navigationService.NavigateAsync("ListCategoriesPage");
                    break;
                case "Sub Categorías":
                    _navigationService.NavigateAsync("ListSubCategoriesPage");
                    break;
                case "Unidades de Venta":
                    _navigationService.NavigateAsync("ListUnitsSalePage");
                    break;
                case "Impuestos":
                    _navigationService.NavigateAsync("ListTaxesPage");
                    break;
                case "Puntos de Venta":
                    _navigationService.NavigateAsync("ListPointsSalePage");
                    break;
                case "Empleados":
                    _navigationService.NavigateAsync("ListEmployeesPage");
                    break;
                case "Impresora":
                    _navigationService.NavigateAsync("SelectPrinterPage");
                    break;
                case "Perfil":
                    _navigationService.NavigateAsync("AdminProfilePage");
                    break;
            }
        }

        public IndexSettingsPageViewModel(
            INavigationService navigationService)
        {
            _navigationService = navigationService;

            ListSettingsOptionsItem = new ObservableCollection<SettingsOptions>()
            {
                new SettingsOptions()
                {
                    Option ="Compañias",OptionDetail="Administra tus compañias."
                },
                new SettingsOptions()
                {
                    Option ="Tiendas",OptionDetail="Administra tus tiendas."
                },
                new SettingsOptions()
                {
                    Option ="Departamentos",OptionDetail="Administra los departamentos existentes en tus tiendas."
                },
                new SettingsOptions()
                {
                    Option ="Categorías",OptionDetail="Administra las categorías de tus productos."
                },
                new SettingsOptions()
                {
                    Option ="Sub Categorías",OptionDetail="Administra las sub categorías de tus productos."
                },
                new SettingsOptions()
                {
                    Option ="Unidades de Venta",OptionDetail="Administra tus unidades de venta."
                },
                new SettingsOptions()
                {
                    Option ="Impuestos",OptionDetail="Administra los impuestos de tus productos."
                },
                new SettingsOptions()
                {
                    Option ="Puntos de Venta",OptionDetail="Administra tus puntos de Venta."
                },
                new SettingsOptions()
                {
                    Option ="Empleados",OptionDetail="Crea, Modifica, Elimina, Asigna TPV."
                },
                new SettingsOptions()
                {
                    Option ="Impresora",OptionDetail="Selecciona tu impresora bluetooth."
                },
                new SettingsOptions()
                {
                    Option ="Perfil",OptionDetail="Administra la información de tu perfil de usuario."
                },
            };
        }
    }

}