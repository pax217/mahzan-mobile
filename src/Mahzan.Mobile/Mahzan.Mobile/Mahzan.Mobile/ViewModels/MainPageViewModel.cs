using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mahzan.Mobile.Models.Menu;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Mahzan.Mobile.Views;
using Mahzan.Mobile.Views.Administrator.Operations;
using Mahzan.Mobile.Views.Administrator.Settings;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private INavigationService _navigationService;
        
        private readonly IRepository<User> _userRepository;
        public ObservableCollection<MyMenuItem> MenuItems { get; set; }
        
        private MyMenuItem selectedMenuItem;
        
        public MyMenuItem SelectedMenuItem
        {
            get => selectedMenuItem;
            set => SetProperty(ref selectedMenuItem, value);
        }

        public DelegateCommand NavigateCommand { get; private set; }

        private string _role { get; set; }
        public string Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Role)));
            }
        }

        private string _userName { get; set; }
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(UserName)));
            }
        }

        private string _storeName { get; set; }
        public string StoreName
        {
            get => _storeName;
            set
            {
                _storeName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(StoreName)));
            }
        }

        private string _pontOfSale { get; set; }
        public string PontOfSale
        {
            get => _pontOfSale;
            set
            {
                _pontOfSale = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(PontOfSale)));
            }
        }
        
        public MainPageViewModel(
            INavigationService navigationService,
            IRepository<User> userRepository)
        {
            //Navigation
            _navigationService = navigationService;

            //Repository
            _userRepository = userRepository;
            
            Task.Run(() => BuildMenu());
            
            NavigateCommand = new DelegateCommand(Navigate);
        }
        
        async void BuildMenu() 
        {
            MenuItems = new ObservableCollection<MyMenuItem>();

            List<User> aspNetUsers = await _userRepository.Get();

            Role = aspNetUsers.FirstOrDefault().Role;
            UserName = aspNetUsers.FirstOrDefault().UserName;
            // StoreName = aspNetUsers.FirstOrDefault().StoreName;
            // PontOfSale = aspNetUsers.FirstOrDefault().PointOfSaleName;

            switch (aspNetUsers.FirstOrDefault().Role)
            {
                case "Administrator":
                    MenuItems = BuildMenuMember();
                    break;
                default:
                    break;
            }
        }
        
        async void Navigate()
        {
            if (!SelectedMenuItem.ExitAplication)
            {
                await _navigationService.NavigateAsync(nameof(NavigationPage) + "/" + SelectedMenuItem.PageName);
            }
            else 
            {
                Application.Current.MainPage = new LoginPage();
            }
        }
        
        private ObservableCollection<MyMenuItem> BuildMenuMember()
        {
            ObservableCollection<MyMenuItem> result = new ObservableCollection<MyMenuItem>();

            /*result.Add(new MyMenuItem()
            {
                Icon = "ic_viewa",
                PageName = nameof(IndexSalesPage),
                Title = "Ventas",
            });*/
            
            /*
             * Operaciones
             * Procesos
             * Consultas
             * Reportes
             * Configuración
             */
            
            
            result.Add(new MyMenuItem()
            {
                Icon = "menu_icon_operations",
                PageName = nameof(IndexOperationsPage),
                Title = "Operaciones",
            });

            result.Add(new MyMenuItem()
            {
                Icon = "menu_icon_settings",
                PageName = nameof(IndexSettingsPage),
                Title = "Configuración",
            });

            result.Add(new MyMenuItem()
            {
                Icon = "menu_icon_exit",
                PageName = nameof(LoginPage),
                Title = "Salir",
                ExitAplication = true
            });

            return result;
        }
    }
}