using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Threading.Tasks;
using BookStore.DataAccess.Models;

namespace BookStore.DataAccess.WebApi.Operations
{
    [Export(typeof(ISearchBooksOperation))]
    class SearchBooksOperation : WebApiClient, ISearchBooksOperation
    {
        public async Task<ICollection<SearchBookModel>> ExecuteAsync(string searchString, int branchId)
        {
            try
            {
                return await GetAsync<List<SearchBookModel>>($"books?searchString={searchString}&branchId={branchId}");
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }

            return null;
        }
    }
}
