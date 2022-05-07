using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Mahzan.Mobile.Models.Menu;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Mahzan.Mobile.ViewModels.Employee.Operations;
using Mahzan.Mobile.Views;
using Mahzan.Mobile.Views.Administrator.Operations;
using Mahzan.Mobile.Views.Administrator.Settings;
using Mahzan.Mobile.Views.Employee.Operations;
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
        
        private string _state { get; set; }
        public string State 
        {
            get => _state;
            set
            {
                _state = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(State)));
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

            Role = ConvertUserRole(aspNetUsers.FirstOrDefault().Role);
            UserName = aspNetUsers.FirstOrDefault().UserName;
            StoreName = aspNetUsers.FirstOrDefault().StoreName;
            PontOfSale = aspNetUsers.FirstOrDefault().PointSaleName;
            State = ConvertPointSaleState(aspNetUsers.FirstOrDefault().State);

            switch (aspNetUsers.FirstOrDefault().Role)
            {
                case "Administrator":
                    MenuItems = BuildMenuMember();
                    break;
                case "Employee":
                    MenuItems = BuildMenuEmployee();
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
                PageName = nameof(Views.Administrator.Operations.IndexOperationsPage),
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
        
        private ObservableCollection<MyMenuItem> BuildMenuEmployee()
        {
            ObservableCollection<MyMenuItem> result = new ObservableCollection<MyMenuItem>();

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
                PageName = nameof(Views.Employee.Operations.IndexOperationsPage),
                Title = "Operaciónes",
            });
            
            result.Add(new MyMenuItem()
            {
                Icon = "menu_icon_settings",
                PageName = nameof(Views.Employee.Settings.IndexSettingsPage),
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
        
        private string ConvertUserRole(string role)
        {
            string result = String.Empty;

            switch (@role)
            {
                case "Administrator":
                    result = "Administrador";
                    break;
                case "Employee":
                    result = "Empleado";
                    break;
            }
            
            return result;
        }

        private string ConvertPointSaleState(string state)
        {
            string result = String.Empty;

            switch (@state)
            {
                case "CLOSED":
                    result = "CERRADA";
                    break;
                case "OPEN":
                    result = "ABIERTA";
                    break;
            }
            
            return result;
        }
    }
}