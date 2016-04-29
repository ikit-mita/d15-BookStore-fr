using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Threading.Tasks;
using System.Web.Http;
using BookStore.DataAccess;
using BookStore.DataAccess.Models;
using Microsoft.Practices.ServiceLocation;

namespace BookStore.WebService.Controllers
{
    [Export]
    public class EmployeesController : ApiController
    {
        [Import]
        private IGetEmployeeOperation GetEmployeeOperation { get; set; }

        public async Task<GetEmployeeModel> GetEmployeeAsync(int id)
        {
            return await GetEmployeeOperation.ExecuteAsync(id);
        }
    }
}