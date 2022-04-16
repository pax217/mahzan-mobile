using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Behaviors;
using Mahzan.Mobile.Commands.CommercialBusiness;
using Mahzan.Mobile.Commands.Company;
using Mahzan.Mobile.Models.CommercialBusiness;
using Mahzan.Mobile.Models.Company;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Services.CommercialBusiness;
using Mahzan.Mobile.Services.Company;
using Mahzan.Mobile.Views.Administrator.Settings.Companies;
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

        public Guid CompanyId { get; set; }
        
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Name)));
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(PassedValidations)));
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
        
        private NotEmptyValidatorBehavior CompanyNameValidator;
        
        private NotEmptyValidatorBehavior RfcValidator;
        
        public bool PassedValidations
        {
            get
            {
                return (CompanyNameValidator.IsValid != null &&
                        RfcValidator.IsValid != null);
            }
        }
        
        
        public AdminCompanyPageViewModel(
            INavigationService navigationService,
            ICommercialBusinessService commercialBusinessService,
            ICompanyService companyService,
            NotEmptyValidatorBehavior companyNameValidator,
            NotEmptyValidatorBehavior rfcValidator)
        {
            _navigationService = navigationService;
            _commercialBusinessService = commercialBusinessService;
            _companyService = companyService;
            
            CompanyNameValidator = companyNameValidator;
            RfcValidator = rfcValidator;

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
            
            if (!PassedValidations)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Guarda la compañia", 
                    $"Debes capturar los campos * requeridos. ", 
                    "Ok");
                return;
            }
            else
            {
                if (CompanyId==Guid.Empty)
                {
                    await CreateCompany();
                }
                else
                {
                    await UpdateCompany();
                }   
            }
        }

        public async Task OnDeleteCompanyCommand()
        {
            if (CompanyId==Guid.Empty)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Elimina la compañia", 
                    $"Debes seleccionar una compañia", 
                    "Ok");   
            }
            else
            {
                await DeleteCompany();
            }
        }

        private async Task CreateCompany()
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
                await App.Current.MainPage.DisplayAlert("CreateCompany", errorApi.Message, "Ok");
                
            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Creación de Compañia", 
                    $"Se ha creado correctamente la compañía {Name}", 
                    "Ok");
            
                await _navigationService.GoBackAsync();   
            }
        }
        private async Task UpdateCompany()
        {
            var httpResponseMessage = await _companyService.Update(new UpdateCompanyCommand
            {
                Company = new UpdateCompany
                {
                    CompanyId = CompanyId,
                    ComercialBusinessId = _selectedCommercialBusiness.CommercialBusinessId,
                    Name = Name,
                    RFC = Rfc,
                },
                Adress = new UpdateAdress
                {
                    Street = Street,
                    Number = Number,
                    InternalNumber = InternalNumber,
                    PostalCode = PostalCode,
                },
                Contact = new UpdateContact()
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
                await App.Current.MainPage.DisplayAlert("UpdateCompany", errorApi.Message, "Ok");

            }
            else
            {
                await App.Current.MainPage.DisplayAlert(
                    "Actualización de Compañia", 
                    $"Se ha actualizado correctamente la compañía {Name}", 
                    "Ok");
            
                await _navigationService.GoBackAsync();    
            }
        }
        private async Task DeleteCompany()
        {
            var answer = await Application
                .Current
                .MainPage
                .DisplayAlert("Atención!",
                    "¿Estas seguro de borrar la Compañia?", "Si", "No");

            if (answer)
            {
                var httpResponseMessage = await _companyService.Delete(CompanyId.ToString());
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert("DeleteCompany", errorApi.Message, "Ok");
                   
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Elimina Compañia", 
                        $"Se ha elimindo correctamente la compañía {Name}", 
                        "Ok");

                    await _navigationService.GoBackAsync();
                }
            }
        }

        private async Task GetCompany()
        {
            var httpResponseMessage= await _companyService.Get(new GetCompaniesCommand
            {
                CompanyId = CompanyId.ToString()
            });
          
            var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
            if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
            {
                var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                await App.Current.MainPage.DisplayAlert("GetCompanies", errorApi.Message, "Ok");
            }
          
            var getCompaniesResponse = JsonConvert.DeserializeObject<GetCompaniesResponse>(respuesta);
            if (getCompaniesResponse != null)
            {
                var company = getCompaniesResponse.Data.FirstOrDefault();
                if (company != null)
                {
                    SelectedCommercialBusiness = CommercialBusiness.FirstOrDefault(
                        c =>
                            c.CommercialBusinessId == getCompaniesResponse.Data.FirstOrDefault().CommercialBusinessId);
                    
                    Name = company.Name;
                    Rfc = company.RFC;
                    Street = company.Street;
                    Number = company.Number;
                    InternalNumber = company.InternalNumber;
                    PostalCode = company.PostalCode;
                    ContactName = company.ContactName;
                    Email = company.Email;
                    if (company.Phone!= null)
                    {
                        Phone = "+52 " + company.Phone;      
                    }
                    WebSite = company.WebSite;
                }
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            CompanyId = parameters.GetValue<Guid>("companyId");
            if (CompanyId!=null)
            {
                 await GetCompany();
            }
        }
    }
}