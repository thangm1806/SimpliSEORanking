using System.Data;
using System.Text.RegularExpressions;

namespace Simpli.Service.SEOChecker.Builder
{
    public abstract class BaseSearchResultBuilder
    {
        private readonly string _rawContent;
        private readonly string _searchUrl;
        public readonly int _resultLimit;

        public BaseSearchResultBuilder(string rawContent, string searchUrl, int resultLimit)
        {
            _rawContent = rawContent;
            _searchUrl = searchUrl;
            _resultLimit = resultLimit;
        }

        protected virtual string GetContentPositions(List<string> urlContents)
        {
            var positions = new List<string>();
            // Remove white spaces
            var formattedContents = urlContents.Select(s => s.Trim());

            var count = 0;
            foreach (var content in formattedContents)
            {
                count++;
                if (!string.IsNullOrEmpty(content) && content.Contains(_searchUrl)) 
                {
                    positions.Add(count.ToString());
                }
            }

            return string.Join(',', positions);
        }

        protected virtual List<string> ExtractUrlsFromHtml(string htmlContent)
        {
            List<string> urls = new List<string>();
            string pattern = $@"href=['""](https?://[^'""]*)['""]";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(htmlContent);

            return matches.Select(m => m.ToString()).Take(_resultLimit).ToList();
        }

        public string GetFinalResult()
        {
            return GetContentPositions(ExtractUrlsFromHtml(_rawContent));
        }
    }
}
