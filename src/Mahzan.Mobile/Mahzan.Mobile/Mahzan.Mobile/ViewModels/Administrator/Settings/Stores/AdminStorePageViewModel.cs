using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Input;
using Mahzan.Mobile.Commands.Company;
using Mahzan.Mobile.Commands.Store;
using Mahzan.Mobile.Models.Company;
using Mahzan.Mobile.Models.Response;
using Mahzan.Mobile.Models.Store;
using Mahzan.Mobile.Services.Company;
using Mahzan.Mobile.Services.Store;
using Newtonsoft.Json;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;

namespace Mahzan.Mobile.ViewModels.Administrator.Settings.Stores
{
    public class AdminStorePageViewModel : BindableBase, INavigationAware
    {
        private readonly INavigationService _navigationService;

        private readonly ICompanyService _companyService;

        private readonly IStoreService _storeService;
        
        private Guid StoreId { get; set; }
        
        private string _code;
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Code))); // Notify that there was a change on this property
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Phone))); // Notify that there was a change on this property
            }
        }

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

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set
            {
                _comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(Comment))); // Notify that there was a change on this property
            }
        }

        //Companies
        private ObservableCollection<Company> _companies;
        public ObservableCollection<Company> Companies
        {
            get => _companies;
            set => SetProperty(ref _companies, value);
        }
        private Company _selectedCompany;
        public Company SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                if (_selectedCompany != value)
                {
                    _selectedCompany = value;
                }
                OnPropertyChanged(new PropertyChangedEventArgs(nameof(SelectedCompany))); // Notify that there was a change on this property
            }
        }
        
        public ICommand SaveStoreCommand { get; set; }
        
        public ICommand DeleteStoreCommand { get; set; }
        
        public AdminStorePageViewModel(
            INavigationService navigationService,
            ICompanyService companyService,
            IStoreService storeService)
        {
            _navigationService = navigationService;
            _companyService = companyService;
            _storeService = storeService;

            Task.Run(GetCompanies);
            
            //Commands
            SaveStoreCommand = new Command(async () => await OnSaveStoreCommand());
            DeleteStoreCommand = new Command(async () => await OnDeleteStoreCommand());
        }

        public async Task GetCompanies()
        {
          var httpResponseMessage= await _companyService.Get(new GetCompaniesCommand());
          
          var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
          if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
          {
              var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
              await App.Current.MainPage.DisplayAlert("GetCompanies", errorApi.Message, "Ok");
          }
          
          var getCompaniesResponse = JsonConvert.DeserializeObject<GetCompaniesResponse>(respuesta);
          if (getCompaniesResponse != null)
              Companies = new ObservableCollection<Company>(getCompaniesResponse.Data);
        }

        public async Task GetStore(string storeId)
        {
           var httpResponseMessageStores = await _storeService.Get(new GetStoreCommand
            {
                StoreId = storeId
            });
           
           var respuesta = await httpResponseMessageStores.Content.ReadAsStringAsync();
          
           if (httpResponseMessageStores.StatusCode != HttpStatusCode.OK)
           {
               var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
               await App.Current.MainPage.DisplayAlert("GetStore", errorApi.Message, "Ok");
           }
           
           var getStoresResponse = JsonConvert.DeserializeObject<GetStoresResponse>(respuesta);

           if (getStoresResponse != null)
           {
               SelectedCompany = Companies.FirstOrDefault(c => c.CompanyId == getStoresResponse.Data.FirstOrDefault().CompanyId);
               Code = getStoresResponse.Data.FirstOrDefault()?.Code;
               Name = getStoresResponse.Data.FirstOrDefault()?.Name;
               Phone = getStoresResponse.Data.FirstOrDefault()?.Phone;
               if (getStoresResponse.Data.FirstOrDefault()?.Phone!= null)
               {
                   Phone = "+52 " + getStoresResponse.Data.FirstOrDefault()?.Phone;      
               }
               Comment = getStoresResponse.Data.FirstOrDefault()?.Comment;
           }
        }

        #region Commads

        private async Task OnSaveStoreCommand()
        {
            if (StoreId==Guid.Empty)
            {
                var httpResponseMessage = await _storeService.Create(new CreateStoreCommand
                {
                    Code = Code,
                    Name = Name,
                    Phone = Phone,
                    Comment = Comment,
                    CompanyId = _selectedCompany.CompanyId.ToString()
                });
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await Application.Current.MainPage.DisplayAlert("OnSaveStoreCommand", errorApi.Message, "Ok");
                }
                
                await Application.Current.MainPage.DisplayAlert(
                    "Creación de Tienda", 
                    $"Se ha creado correctamenta la tienda {Name}", 
                    "Ok");

                await _navigationService.GoBackAsync();

            }
            else
            {
                var httpResponseMessage = await _storeService.Update(new UpdateStoreCommand
                {
                    StoreId = StoreId,
                    Code = Code,
                    Name = Name,
                    Phone = Phone,
                    Comment = Comment,
                    CompanyId = _selectedCompany.CompanyId.ToString()
                });
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
          
                if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await Application.Current.MainPage.DisplayAlert("OnSaveStoreCommand", errorApi.Message, "Ok");
                }
                
                await Application.Current.MainPage.DisplayAlert(
                    "Actualización de Tienda", 
                    $"Se ha actualizado correctamenta la tienda {Name}", 
                    "Ok");

                await _navigationService.GoBackAsync();
            }
        }

        public async Task OnDeleteStoreCommand()
        {
            if (StoreId==Guid.Empty)
            {
                await App.Current.MainPage.DisplayAlert(
                    "Elimina la tienda", 
                    $"Debes seleccionar una tienda", 
                    "Ok");
            }
            else
            {
                await DeleteStore();
            }
        }

        private async Task DeleteStore()
        {
            var answer = await Application
                .Current
                .MainPage
                .DisplayAlert("Atención!",
                    "¿Estas seguro de borrar la Tienda?", "Si", "No");
            
            if (answer)
            {
                var httpResponseMessage = await _storeService.Delete(StoreId.ToString());
                
                var respuesta = await httpResponseMessage.Content.ReadAsStringAsync();
            
                if (httpResponseMessage.StatusCode!=HttpStatusCode.OK)
                {
                    var errorApi = JsonConvert.DeserializeObject<ApiResponse>(respuesta);
                    await App.Current.MainPage.DisplayAlert("DeleteCompany", errorApi.Message, "Ok");
                   
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert(
                        "Elimina Tienda", 
                        $"Se ha elimindo correctamente la tienda {Name}", 
                        "Ok");

                    await _navigationService.GoBackAsync();
                }
            }
        }

        #endregion

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            throw new System.NotImplementedException();
        }

        public async void OnNavigatedTo(INavigationParameters parameters)
        {
            StoreId = parameters.GetValue<Guid>("storeId");
            if (StoreId!=Guid.Empty)
            {
                await GetStore(StoreId.ToString());
            }
        }
    }
}