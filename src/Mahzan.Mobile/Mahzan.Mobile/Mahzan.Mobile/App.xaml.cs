using System;
using Mahzan.Mobile.Services.Category;
using Mahzan.Mobile.Services.CommercialBusiness;
using Mahzan.Mobile.Services.Company;
using Mahzan.Mobile.Services.Department;
using Mahzan.Mobile.Services.Product;
using Mahzan.Mobile.Services.SHA1;
using Mahzan.Mobile.Services.Store;
using Mahzan.Mobile.Services.SubCategory;
using Mahzan.Mobile.Services.User;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Mahzan.Mobile.ViewModels;
using Mahzan.Mobile.ViewModels.Administrator;
using Mahzan.Mobile.ViewModels.Administrator.Products;
using Mahzan.Mobile.ViewModels.Administrator.Products.Inventory;
using Mahzan.Mobile.ViewModels.Administrator.Settings;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Companies;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Printers;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Tickets;
using Mahzan.Mobile.ViewModels.Administrator.WorkEnviroment;
using Mahzan.Mobile.ViewModels.Administrator.WorkEnviroment.Stores;
using Mahzan.Mobile.ViewModels.Administrator.WorkEnviroment.Tpvs;
using Mahzan.Mobile.Views;
using Mahzan.Mobile.Views.Administrator;
using Mahzan.Mobile.Views.Administrator.Products;
using Mahzan.Mobile.Views.Administrator.Products.Inventory;
using Mahzan.Mobile.Views.Administrator.Settings;
using Mahzan.Mobile.Views.Administrator.Settings.Companies;
using Mahzan.Mobile.Views.Administrator.Settings.Printers;
using Mahzan.Mobile.Views.Administrator.Settings.Tickets;
using Mahzan.Mobile.Views.Administrator.WorkEnviroment;
using Mahzan.Mobile.Views.Administrator.WorkEnviroment.Stores;
using Mahzan.Mobile.Views.Administrator.WorkEnviroment.Tpvs;
using Plugin.Toasts;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Mahzan.Mobile
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null)
        {
            
        }

        public App(IPlatformInitializer initializer) : base(initializer)
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            //await NavigationService.NavigateAsync(nameof(Mobile.Views.MainPage) + "/" + nameof(NavigationPage) + "/" + nameof(LoginPage));
            await NavigationService.NavigateAsync("LoginPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegisterDependencies(containerRegistry);
        }

        protected void RegisterDependencies(IContainerRegistry containerRegistry)
        {
            RegisterRepositoryDependencies(containerRegistry);
            RegisterServicesDependencies(containerRegistry);
            RegisterNavigationDependencies(containerRegistry);
        }

        protected void RegisterRepositoryDependencies(IContainerRegistry containerRegistry)
        {
            //Repository
            containerRegistry.Register<IRepository<User>, Repository<User>>();
            containerRegistry.Register<IRepository<BluetoothDevice>, Repository<BluetoothDevice>>();
        }
        
        protected void RegisterServicesDependencies(IContainerRegistry containerRegistry)
        {
            //Services
            containerRegistry.Register<ISHA1, SHA1>();
            
            containerRegistry.Register<IUserService, UserService>();
            containerRegistry.Register<IProductsService, ProductsService>();
            containerRegistry.Register<IDepartmentService, DepartmentService>();
            containerRegistry.Register<ICategoryService, CategoryService>();
            containerRegistry.Register<ISubCategoryService, SubCategoryService>();
            containerRegistry.Register<IStoreService, StoreService>();
            containerRegistry.Register<ICompanyService, CompanyService>();
            containerRegistry.Register<ICompanyService, CompanyService>();
            containerRegistry.Register<ICommercialBusinessService, CommercialBusinessService>();
        }
        
        protected void RegisterNavigationDependencies(IContainerRegistry containerRegistry)
        {
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            
            //Administrator
            RegisterAdministratorSettingsNavigation(containerRegistry);
                
            // Products
            containerRegistry.RegisterForNavigation<IndexProductsPage, IndexProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<ListProductsPage, ListProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<AddProductPage, AddProductPageViewModel>();
            
            // Work Enviroment
            containerRegistry.RegisterForNavigation<IndexWorkEnviromentPage, IndexWorkEnviromentPageViewModel>();
            
            containerRegistry.RegisterForNavigation<ListStoresPage, ListStoresPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminStorePage, AdminStorePageViewModel>();
            
            containerRegistry.RegisterForNavigation<ListTpvsPage, ListTpvsPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminTpvsPage, AdminTpvsPageViewModel>();
        }

        protected void RegisterAdministratorSettingsNavigation(IContainerRegistry containerRegistry)
        {
            // Administrator
            containerRegistry.RegisterForNavigation<AdministratorDashboardPage, AdministratorDashboardPageViewModel>();
            
            // Settings
            containerRegistry.RegisterForNavigation<IndexSettingsPage, IndexSettingsPageViewModel>();
            
            // Companies
            RegisterAdministratorSettingsCompaniesNavigation(containerRegistry);
            
            
            containerRegistry.RegisterForNavigation<SelectPrinterPage, SelectPrinterPageViewModel>();
            
            // Tickets
            containerRegistry.RegisterForNavigation<IndexTicketsPage, IndexTicketsPageViewModel>();
        }

        protected void RegisterAdministratorSettingsCompaniesNavigation(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ListCompaniesPage, ListCompaniesPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminCompanyPage, AdminCompanyPageViewModel>();
        }
    }
}