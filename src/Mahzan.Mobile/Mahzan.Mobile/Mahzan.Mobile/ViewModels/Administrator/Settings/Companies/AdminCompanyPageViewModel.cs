using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.CommercialBusiness;
using Mahzan.Mobile.Commands.Company;
using Mahzan.Mobile.Models.CommercialBusiness;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.CommercialBusiness;
using Mahzan.Mobile.Services.Company;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Companies
{
    public class AdminCompanyPageViewModel:  BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly ICommercialBusinessService _commercialBusinessService;

        private readonly ICompanyService _companyService;
        
        //Commercial Business
        private ObservableCollection<CommercialBusiness> _commercialBusiness;
        public ObservableCollection<CommercialBusiness> CommercialBusiness
        {
            get => _commercialBusiness;
            set => SetProperty(ref _commercialBusiness, value);
        }
        private CommercialBusiness _selectedCommercialBusiness;
        public CommercialBusiness SelectedCommercialBusiness
        {
            get => _selectedCommercialBusiness;
            set
            {
                if (_selectedCommercialBusiness != value)
                {
                    _selectedCommercialBusiness = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedCommercialBusiness))); // Notify that there was a change on this property
            }
        }

        public Guid? CompanyId { get; set; }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name))); // Notify that there was a change on this property
            }
        }
        
        private string _rfc;
        public string Rfc
        {
            get { return _rfc; }
            set
            {
                _rfc = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Rfc)));
            }
        }
        
        private string _street;
        public string Street
        {
            get { return _street; }
            set
            {
                _street = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Street)));
            }
        }
        
        private string _number;
        public string Number
        {
            get { return _number; }
            set
            {
                _number = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Number)));
            }
        }
        
        private string _internalNumber;
        public string InternalNumber
        {
            get { return _internalNumber; }
            set
            {
                _internalNumber = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(InternalNumber)));
            }
        }
        
        private string _postalCode;
        public string PostalCode
        {
            get { return _postalCode; }
            set
            {
                _postalCode = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(PostalCode)));
            }
        }
        
        private string _contactName;
        public string ContactName
        {
            get { return _contactName; }
            set
            {
                _contactName = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(ContactName)));
            }
        }
        
        private string _email;
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Email)));
            }
        }
        
        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Phone)));
            }
        }
        
        private string _webSite;
        public string WebSite
        {
            get { return _webSite; }
            set
            {
                _webSite = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(WebSite)));
            }
        }

        public ICommand SaveCompanyCommand { get; set; }
        
        public ICommand DeleteCompanyCommand { get; set; }
        

        public AdminCompanyPageViewModel(
            INavigationService navigationService,
            ICommercialBusinessService commercialBusinessService,
            ICompanyService companyService)
        {
            _navigationService = navigationService;
            _commercialBusinessService = commercialBusinessService;
            _companyService = companyService;

            Task.Run(GetCommercialBusiness);


            SaveCompanyCommand = new Command(async () => await OnSaveCompanyCommand());
            
            DeleteCompanyCommand = new Command(async () => await OnDeleteCompanyCommand()); 
        }

        public async Task GetCommercialBusiness()
        {
            var httpResponseMessage = await _commercialBusinessService.Get(new GetComercialBusinessCommand());

            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
            if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetCommercialBusiness", errorApi.Message, "Ok");
            }
            
            var getCommercialBusinessResponse = JsonConvert.DeserializeObject<GetCommercialBusinessResponse>(respuesta);
            CommercialBusiness=new ObservableCollection<CommercialBusiness>(getCommercialBusinessResponse.Data);
        }

        public async Task OnSaveCompanyCommand()
        {
            if (CompanyId==null)
            {
                var httpResponseMessage = await _companyService.Create(new CreateCompanyCommand
                {
                    Company = new CompanyCommand
                    {
                        ComercialBusinessId = _selectedCommercialBusiness.CommercialBusinessId,
                        Name = Name,
                        RFC = Rfc,
                    },
                    Adress = new AdressCommand
                    {
                        Street = Street,
                        Number = Number,
                        InternalNumber = InternalNumber,
                        PostalCode = PostalCode,
                    },
                    Contact = new ContactCommand
                    {
                        ContactName = ContactName,
                        Email = Email,
                        Phone = Phone,
                        WebSite = WebSite  
                    }
                });
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert("OnSaveCompanyCommand", errorApi.Message, "Ok");
                    
                }
                
                await App.Current.MainPage.DisplayAlert(
                    "Creación de Compañia", 
                    $"Se ha creado correctamente la compañía {Name}", 
                    "Ok");
            }
        }

        public async Task OnDeleteCompanyCommand()
        {
            
        }


        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }
    }
}