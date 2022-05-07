using System.Collections.ObjectModel;
using Mahzan.Mobile.Models.Menu.Operations;
using Mahzan.Mobile.Models.Settings;
using Prism.Mvvm;
using Prism.Navigation;

namespace Mahzan.Mobile.ViewModels.Employee.Settings
{
    public class IndexSettingsPageViewModel: ViewModelBase, INavigationAware
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
        
        public IndexSettingsPageViewModel(
            INavigationService navigationService) : base(navigationService)
        { 
            _navigationService = navigationService;

            ListSettingsOptionsItem = new ObservableCollection<SettingsOptions>()
            {
                new SettingsOptions()
                {
                    Option ="Impresora",OptionDetail="Selecciona tu impresora bluetooth."
                },
            };
        }
        
        private void HandleSelectedSettingOption()
        {
            switch (_selectedSettingOption.Option)
            {
                case "Impresora":
                    _navigationService.NavigateAsync("SelectPrinterPage");
                    break;
            }
        }
    }
}