using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.AccessControl;
using BookStore.BusinessLogic;
using BookStore.DataAccess;
using BookStore.DataAccess.Models;
using IkitMita;
using IkitMita.Mvvm.ViewModels;

namespace BookStore.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CreateOrderViewModel : ChildViewModelBase
    {
        private ICollection<GetClientModel> _clients;
        private GetClientModel _selectedClient;
        private ICollection<SearchBookModel> _foundBooks;
        private GetEmployeeModel _currentEmployee;
        private DelegateCommand<string> _searchBooksCommand;
        private DelegateCommand<SearchBookModel> _selectBookCommand;

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

        public ICollection<SearchBookModel> FoundBooks
        {
            get { return _foundBooks; }
            set
            {
                _foundBooks = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<SaveOrderedBookModel> OrderedBooks { get; } = new ObservableCollection<SaveOrderedBookModel>();

        public GetClientModel SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                _selectedClient = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand<string> SearchBooksCommand => _searchBooksCommand
               ?? (_searchBooksCommand = new DelegateCommand<string>(SearchBooks));

        public DelegateCommand<SearchBookModel> SelectBookCommand => _selectBookCommand
               ?? (_selectBookCommand = new DelegateCommand<SearchBookModel>(SelectBook));

        public async void InitializeAsync()
        {
            using (StartOperation())
            {
                Clients = await GetClientsOperation.ExecuteAsync();
                var user = SecurityManager.GetCurrentUser();
                _currentEmployee = await GetEmployeeOperation.ExecuteAsync(user.Id);
            }
        }

        private async void SearchBooks(string searchString)
        {
            using (StartOperation())
            {
                FoundBooks = await SearchBooksOperation.ExecuteAsync(searchString, _currentEmployee.BranchId);
            }
        }

        private void SelectBook(SearchBookModel book)
        {
            //var bookModel = new SaveOrderedBookModel
            //{
            //    Amount = book.Amount,
            //    Price = book.Price,
            //    BookId = book.BookId,
            //    BookTitle = book.BookTitle
            //};

            SaveOrderedBookModel bookModel = OrderedBooks.FirstOrDefault(ob => ob.BookId == book.BookId);

            if (bookModel == null)
            {
                bookModel = new SaveOrderedBookModel().CopyProperties(book);
                bookModel.Amount = 0;
                bookModel.MaxAmount = book.Amount;
                OrderedBooks.Add(bookModel);
            }

            bookModel.Amount += 1;
        }

        [Import]
        private IGetClientsOperation GetClientsOperation { get; set; }

        [Import]
        private ISearchBooksOperation SearchBooksOperation { get; set; }

        [Import]
        private IGetEmployeeOperation GetEmployeeOperation { get; set; }

        [Import]
        private ISecurityManager SecurityManager { get; set; }




    }
}
