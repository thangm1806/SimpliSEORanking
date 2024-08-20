using MediatR;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Simpli.Infrastructure.MediatR.Requests;
using Simpli.Service.SEOChecker.Builder;
using Simpli.Service.SEOChecker.Configurations;
using Simpli.Service.SEOChecker.Constants;
using Simpli.Service.SEOChecker.Services;

namespace Simpli.Infrastructure.MediatR.RequestHandlers
{
    public class GoogleSearchRequestHandler : IRequestHandler<GoogleSearchRequest, GoogleSearchRequest.ResultModel>
    {
        private readonly ISearchEngineService _searchService;
        private readonly IMemoryCache _cache;
        private readonly SearchEngineOptions _searchEngineOptions;
        public GoogleSearchRequestHandler(ISearchEngineService searchService, IOptions<SearchEngineOptions> options, IMemoryCache memoryCache) 
        {
            _searchService = searchService;
            _searchEngineOptions = options.Value;
            _cache = memoryCache;
        }

        public async Task<GoogleSearchRequest.ResultModel> Handle(GoogleSearchRequest request, CancellationToken cancellationToken)
        {
            var cacheKey = ServiceConstants.GetMemoryCacheKey(SearchEngine.Google);
            var cacheValue = _cache.Get(cacheKey);
            if (cacheValue != null && !string.IsNullOrWhiteSpace(cacheValue.ToString()))
                return new GoogleSearchRequest.ResultModel { Result = cacheValue.ToString() };

            var googleSearchUrl = ServiceConstants.GetFullSearchEngineUrl(request.BaseUrl, request.SearchTerm, request.SearchLimit);
            var response = await _searchService.SearchAsync(googleSearchUrl, request.SearchUrl, request.SearchLimit, SearchEngine.Google);
            if (!response.Success || (response.Data != null && string.IsNullOrEmpty(response.Data.Result)))
            {
                _cache.Remove(cacheKey);
                return new GoogleSearchRequest.ResultModel();
            }

            _cache.Set(cacheKey, response.Data.Result, TimeSpan.FromMinutes(_searchEngineOptions.CacheExpiryInMinutes));
            return response.Data;
        }
    }
}
