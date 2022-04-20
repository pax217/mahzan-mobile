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
                new SettingsOptions(){ Option ="Compañias",OptionDetail="Administra tus compañias."},
                new SettingsOptions(){ Option ="Tiendas",OptionDetail="Administra tus tiendas."},
                new SettingsOptions(){ Option ="Perfil",OptionDetail="Administra la información de tu perfil de usuario."},
            };
        }
    }

}