using System.Diagnostics;
using Prism.Navigation;

namespace Mahzan.Mobile.ViewModels.Administrator
{
    public class AdministratorDashboardPageViewModel :ViewModelBase
    {
        public AdministratorDashboardPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            Debug.WriteLine("entra");
        }
    }
}