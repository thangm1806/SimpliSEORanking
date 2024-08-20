using Simpli.Service.SEOChecker.Builder;
using System.Web;

namespace Simpli.Service.SEOChecker.Constants
{
    public static class ServiceConstants
    {
        public static string GetFullSearchEngineUrl(string baseUrl, string searchTerm, int maxResults) => $"{baseUrl}/search?q={HttpUtility.UrlEncode(searchTerm)}&num={maxResults}";
        public static string GetMemoryCacheKey(SearchEngine engine) => $"SEOCheckerService_{engine.ToString()}";
    }
}
