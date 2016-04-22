using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookStore.DataAccess.EF.Models;
using BookStore.DataAccess.Models;

namespace BookStore.DataAccess.EF.Operations
{
    [Export(typeof (ISaveOrderOperation))]
    class SaveOrderOperation : ISaveOrderOperation
    {
        public async Task<bool> ExecuteAsync(SaveOrderModel orderModel)
        {
            using (var db = new BookStoreDbContext())
            {
                var order = new Order
                {
                    ClientId = orderModel.ClientId,
                    EmployeeId = orderModel.EmployeeId,
                    OrdeDate = orderModel.OrderDate,
                    TotalConst = orderModel.TotalCost,
                };
                db.Orders.Add(order);

                foreach (var saveOrderedBookModel in orderModel.OrderedBooks)
                {
                    db.OrderedBooks.Add(new OrderedBook
                    {
                        Amount = saveOrderedBookModel.Amount,
                        BookId = saveOrderedBookModel.BookId,
                        Price = saveOrderedBookModel.Price,
                        Order = order
                    });
                }

                var books = orderModel.OrderedBooks
                    .ToDictionary(ob => ob.BookId, ob => ob.Amount);

                var bookAmounts = await db.BookAmounts
                    .Where(ba => ba.BranchId == orderModel.BranchId)
                    .Where(ba => books.Keys.Contains(ba.BookId))
                    .ToListAsync();

                foreach (var bookAmount in bookAmounts)
                {
                    bookAmount.Amount -= books[bookAmount.BookId];
                }

                await db.SaveChangesAsync();
                return true;
            }
        }
    }
}
