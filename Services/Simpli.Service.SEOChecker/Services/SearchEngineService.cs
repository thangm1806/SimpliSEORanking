using Simpli.Infrastructure;
using Simpli.Infrastructure.MediatR.Requests;
using Simpli.Service.SEOChecker.Builder;
using System.Net.Http;

namespace Simpli.Service.SEOChecker.Services
{
    public interface ISearchEngineService
    {
        Task<Response<GoogleSearchRequest.ResultModel>> SearchAsync(string url, string searchUrl, int limit, SearchEngine engine);
    }
    public class SearchEngineService : ISearchEngineService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private const string BrowserUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36";
        private const string CouldNotSearchResult = "Could not search any results.";
        public SearchEngineService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<Response<GoogleSearchRequest.ResultModel>> SearchAsync(string url, string searchUrl, int limit, SearchEngine engine)
        {
            using HttpClient client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", BrowserUserAgent);
            var response = (await client.GetAsync(url)).EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                return new Response<GoogleSearchRequest.ResultModel> { Success = false, Message = CouldNotSearchResult };

            var builder = SearchResultBuilderFactory.CreateBuilder(engine, content, searchUrl, limit);
                

            return new Response<GoogleSearchRequest.ResultModel> { Success = true, Data = new GoogleSearchRequest.ResultModel { Result = builder.GetFinalResult() } } ;
        }
    }
}
