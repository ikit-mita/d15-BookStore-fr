using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace BookStore.DataAccess.WebApi
{
    abstract class WebApiClient
    {
        protected HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:62598/api/")
            };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept
                .Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        protected async Task<T> GetAsync<T>(string url)
        {
            using (var client = CreateHttpClient())
            {
                var responseMessage = await client.GetAsync(url);
                responseMessage.EnsureSuccessStatusCode();
                var result = await responseMessage.Content.ReadAsAsync<T>();
                return result;
            }
        } 
    }
}
