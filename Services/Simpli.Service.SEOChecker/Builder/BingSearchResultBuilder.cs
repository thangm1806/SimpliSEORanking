namespace Simpli.Service.SEOChecker.Builder
{
    sealed class BingSearchResultBuilder : BaseSearchResultBuilder
    {
        public BingSearchResultBuilder(string rawContent, string searchUrl, int resultLimit) : base(rawContent, searchUrl, resultLimit)
        {
        }
    }
}
