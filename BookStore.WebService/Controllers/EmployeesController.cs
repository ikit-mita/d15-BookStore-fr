using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Web.Http;
using BookStore.DataAccess;
using BookStore.DataAccess.Models;

namespace BookStore.WebService.Controllers
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    [Authorize]
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