using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookStore.DataAccess.Models;

namespace BookStore.DataAccess
{
    public interface IGetClientsOperation
    {
        Task<ICollection<GetClientModel>> ExecuteAsync(string clientLastName = null);
    }
}
