using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using BookStore.DataAccess.Models;

namespace BookStore.DataAccess.WebApi.Operations
{
    [Export(typeof(IGetClientsOperation))]
    class GetClientsOperation : IGetClientsOperation
    {
        public Task<ICollection<GetClientModel>> ExecuteAsync(string clientLastName = null)
        {
            throw new NotImplementedException();
        }
    }
}
