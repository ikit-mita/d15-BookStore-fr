using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DataAccess.Models;
using BookStore.DataAccess.EF.Models;
using IkitMita;

namespace BookStore.DataAccess.EF.Operations
{
    [Export(typeof(IGetClientsOperation))]
    class GetClientsOperation : IGetClientsOperation
    {
        public async Task<ICollection<GetClientModel>> ExecuteAsync(string clientLastName = null)
        {
            using (var db = new BookStoreDbContext())
            {
               IQueryable<Client> query = db.Clients;

                if (!clientLastName.IsNullOrEmpty())
                {
                    query = query.Where(c => c.LastName.StartsWith(clientLastName));
                }

                var clientModel = await query
                    .Select(c => new GetClientModel
                    {
                           LastName = c.LastName,
                           FirstName = c.FirstName,
                           Id = c.Id,
                           MiddleName = c.MiddleName
                    }).ToListAsync();

                return clientModel;
            }
        }
    }
}
