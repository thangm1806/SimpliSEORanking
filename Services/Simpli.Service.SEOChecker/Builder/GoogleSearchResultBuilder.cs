namespace Simpli.Service.SEOChecker.Builder
{
    sealed class GoogleSearchResultBuilder : BaseSearchResultBuilder
    {
        public GoogleSearchResultBuilder(string rawContent, string searchUrl, int resultLimit) : base(rawContent, searchUrl, resultLimit)
        {
        }
    }
}
