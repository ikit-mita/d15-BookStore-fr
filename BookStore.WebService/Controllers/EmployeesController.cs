using System.Threading.Tasks;
using System.Web.Http;
using BookStore.DataAccess.EF.Operations;
using BookStore.DataAccess.Models;

namespace BookStore.WebService.Controllers
{
    public class EmployeesController : ApiController
    {
        public async Task<GetEmployeeModel> GetEmployeeAsync(int id)
        {
            var getEmployeeOperation = new GetEmployeeOperation();

            return await getEmployeeOperation.ExecuteAsync(id);
        }
    }
}