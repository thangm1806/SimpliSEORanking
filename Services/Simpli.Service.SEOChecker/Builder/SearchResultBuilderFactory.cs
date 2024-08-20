using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simpli.Service.SEOChecker.Builder
{
    public enum SearchEngine
    {
        Google,
        Bing
    }

    public static class SearchResultBuilderFactory
    {
        public static BaseSearchResultBuilder CreateBuilder(SearchEngine engine, string rawContent, string searchUrl, int resultLimit)
        {
            switch (engine)
            {
                case SearchEngine.Bing:
                    return new BingSearchResultBuilder(rawContent, searchUrl, resultLimit);
                case SearchEngine.Google:
                default:
                    return new GoogleSearchResultBuilder(rawContent, searchUrl, resultLimit);
            }
        }
    }
}
