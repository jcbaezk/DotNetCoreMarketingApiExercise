using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Marketing.Persistence.DbContexts;
using Xunit;

namespace Marketing.Api.IntegrationTests.TestSetup
{
    public class ApiTest : IClassFixture<ClassTestFixture>
    {
        private readonly ClassTestFixture _testFixture;

        public ApiTest(ClassTestFixture testFixture)
        {
            _testFixture = testFixture;
        }

        public HttpClient Client => _testFixture.Client;

        public MarketingDbContext Context => _testFixture.Context;

        protected async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            var response = await _testFixture.Client.SendAsync(CreateRequest(HttpMethod.Get, endpoint));
            return response;
        }

        protected async Task<HttpResponseMessage> PostAsync<TModel>(string endpoint, TModel model)
        {
            var response = await _testFixture.Client.SendAsync(CreateRequest(HttpMethod.Post, endpoint, model));
            return response;
        }

        protected async Task<HttpResponseMessage> PutAsync<TModel>(string endpoint, TModel model)
        {
            var response = await _testFixture.Client.SendAsync(CreateRequest(HttpMethod.Put, endpoint, model));
            return response;
        }

        protected async Task<HttpResponseMessage> DeleteAsync(string endpoint)
        {
            var response = await _testFixture.Client.SendAsync(CreateRequest(HttpMethod.Delete, endpoint));
            return response;
        }

        private static HttpRequestMessage CreateRequest(HttpMethod method, string endpoint)
        {
            return new HttpRequestMessage(method, endpoint);
        }

        private static HttpRequestMessage CreateRequest<TModel>(HttpMethod method, string endpoint, TModel data)
        {
            var request = new HttpRequestMessage(method, endpoint)
            {
                Content = new ObjectContent<TModel>(data, new JsonMediaTypeFormatter())
            };
            return request;
        }
    }
}