using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DataAccess;
using BookStore.DataAccess.Models;
using IkitMita.Mvvm.ViewModels;

namespace BookStore.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CreateOrderViewModel : ChildViewModelBase
    {
        private ICollection<GetClientModel> _clients;
        private GetClientModel _selectedClient;

        public CreateOrderViewModel()
        {
            Title = "Создание заказа";
        }

        public ICollection<GetClientModel> Clients
        {
            get { return _clients; }
            set
            {
                _clients = value;
                OnPropertyChanged();
            }
        }

        public GetClientModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public async void InitializeAsync()
        {
            using (StartOperation())
            {
                Clients = await GetClientsOperation.ExecuteAsync();
            }
        }
        [Import]
        private IGetClientsOperation GetClientsOperation { get; set; }
    }
}
