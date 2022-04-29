using Mahzan.Mobile.Services.Category;
using Mahzan.Mobile.Services.CommercialBusiness;
using Mahzan.Mobile.Services.Company;
using Mahzan.Mobile.Services.Department;
using Mahzan.Mobile.Services.Employee;
using Mahzan.Mobile.Services.PointSale;
using Mahzan.Mobile.Services.Product;
using Mahzan.Mobile.Services.SHA1;
using Mahzan.Mobile.Services.Store;
using Mahzan.Mobile.Services.SubCategory;
using Mahzan.Mobile.Services.Tax;
using Mahzan.Mobile.Services.UnitSale;
using Mahzan.Mobile.Services.UnitsSale;
using Mahzan.Mobile.Services.User;
using Mahzan.Mobile.SqLite._Base;
using Mahzan.Mobile.SqLite.Entities;
using Mahzan.Mobile.ViewModels;
using Mahzan.Mobile.ViewModels.Administrator;
using Mahzan.Mobile.ViewModels.Administrator.Operations;
using Mahzan.Mobile.ViewModels.Administrator.Operations.Products;
using Mahzan.Mobile.ViewModels.Administrator.Settings;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Categories;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Companies;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Departaments;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Employees;
using Mahzan.Mobile.ViewModels.Administrator.Settings.PointsSale;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Printers;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Profile;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Stores;
using Mahzan.Mobile.ViewModels.Administrator.Settings.SubCategories;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Taxes;
using Mahzan.Mobile.ViewModels.Administrator.Settings.Tickets;
using Mahzan.Mobile.ViewModels.Administrator.Settings.UnitsSale;
using Mahzan.Mobile.ViewModels.Employee;
using Mahzan.Mobile.Views;
using Mahzan.Mobile.Views.Administrator;
using Mahzan.Mobile.Views.Administrator.Operations;
using Mahzan.Mobile.Views.Administrator.Operations.Products;
using Mahzan.Mobile.Views.Administrator.Settings;
using Mahzan.Mobile.Views.Administrator.Settings.Categories;
using Mahzan.Mobile.Views.Administrator.Settings.Companies;
using Mahzan.Mobile.Views.Administrator.Settings.Departments;
using Mahzan.Mobile.Views.Administrator.Settings.Employees;
using Mahzan.Mobile.Views.Administrator.Settings.PointsSale;
using Mahzan.Mobile.Views.Administrator.Settings.Printers;
using Mahzan.Mobile.Views.Administrator.Settings.Profile;
using Mahzan.Mobile.Views.Administrator.Settings.Stores;
using Mahzan.Mobile.Views.Administrator.Settings.SubCategories;
using Mahzan.Mobile.Views.Administrator.Settings.Taxes;
using Mahzan.Mobile.Views.Administrator.Settings.Tickets;
using Mahzan.Mobile.Views.Administrator.Settings.UnitsSale;
using Mahzan.Mobile.Views.Employee;
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
            containerRegistry.Register<ICommercialBusinessService, CommercialBusinessService>();
            containerRegistry.Register<ITaxService, TaxService>();
            containerRegistry.Register<IUnitSaleService, UnitSaleService>();
            containerRegistry.Register<IPointSaleService, PointSaleService>();
            containerRegistry.Register<IEmployeeService, EmployeeService>();
            
        }
        
        protected void RegisterNavigationDependencies(IContainerRegistry containerRegistry)
        {
            //Navigation
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
            containerRegistry.RegisterForNavigation<ResetPasswordPage, ResetPasswordPageViewModel>();
            
            //Administrator
            RegisterAdministratorSettingsNavigation(containerRegistry);
            
            //Employee
            RegisterAdministratorEmployeeNavigation(containerRegistry);
        }

        protected void RegisterAdministratorSettingsNavigation(IContainerRegistry containerRegistry)
        {
            // Administrator
            containerRegistry.RegisterForNavigation<AdministratorDashboardPage, AdministratorDashboardPageViewModel>();
            
            // Settings
            RegisterAdministratorSettingsCompaniesNavigation(containerRegistry);

            // Operations
            RegisterAdministratorOperationsCompaniesNavigation(containerRegistry);
            
        }
        protected void RegisterAdministratorEmployeeNavigation(IContainerRegistry containerRegistry)
        {
            
            containerRegistry.RegisterForNavigation<SelectStorePage, SelectStorePageViewModel>();
            containerRegistry.RegisterForNavigation<SelectPointSalePage, SelectPointSalePageViewModel>();
            
            containerRegistry.RegisterForNavigation<EmployeeDashboardPage, EmployeeDashboardPageViewModel>();
            
            
        }

        protected void RegisterAdministratorSettingsCompaniesNavigation(IContainerRegistry containerRegistry)
        {
            // Settings
            containerRegistry.RegisterForNavigation<IndexSettingsPage, IndexSettingsPageViewModel>();

            // Companies
            containerRegistry.RegisterForNavigation<ListCompaniesPage, ListCompaniesPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminCompanyPage, AdminCompanyPageViewModel>();
            
            // Stores
            containerRegistry.RegisterForNavigation<ListStoresPage, ListStoresPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminStorePage, AdminStorePageViewModel>();
            
            // Departments
            containerRegistry.RegisterForNavigation<ListDepartmentsPage, ListDepartmentsPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminDepartmentPage, AdminDepartmentPageViewModel>();
            
            // Categories
            containerRegistry.RegisterForNavigation<ListCategoriesPage, ListCategoriesPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminCategoryPage, AdminCategoryPageViewModel>();
            
            // SubCategories
            containerRegistry.RegisterForNavigation<ListSubCategoriesPage, ListSubCategoriesPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminSubCategoryPage, AdminSubCategoryPageViewModel>();
            
            // Units Sale
            containerRegistry.RegisterForNavigation<ListUnitsSalePage, ListUnitsSalePageViewModel>();
            containerRegistry.RegisterForNavigation<AdminUnitSalePage, AdminUnitSalePageViewModel>();
            
            // Taxes
            containerRegistry.RegisterForNavigation<ListTaxesPage, ListTaxesPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminTaxPage, AdminTaxPageViewModel>();
            
            // Points Sale
            containerRegistry.RegisterForNavigation<ListPointsSalePage, ListPointsSalePageViewModel>();
            containerRegistry.RegisterForNavigation<AdminPointSalePage, AdminPointSalePageViewModel>();
            
            // Employees
            containerRegistry.RegisterForNavigation<ListEmployeesPage, ListEmployeesPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminEmployeePage, AdminEmployeePageViewModel>();
            
            // Printers
            containerRegistry.RegisterForNavigation<SelectPrinterPage, SelectPrinterPageViewModel>();
            
            // Tickets
            containerRegistry.RegisterForNavigation<TicketsSettingsPage, TicketsSettingsPageViewModel>();
            
            // Profile
            containerRegistry.RegisterForNavigation<AdminProfilePage, AdminProfilePageViewModel>();
        }
        
        protected void RegisterAdministratorOperationsCompaniesNavigation(IContainerRegistry containerRegistry)
        {
            // Operations
            containerRegistry.RegisterForNavigation<IndexOperationsPage, IndexOperationsPageViewModel>();
            
            // Poducts
            containerRegistry.RegisterForNavigation<ListProductsPage, ListProductsPageViewModel>();
            containerRegistry.RegisterForNavigation<AdminProductPage, AdminProductPageViewModel>();

        }
    }
}