namespace Simpli.API
{
    public interface IHostConfig
    {
        string? GoogleSearchBaseUrl {  get; }
        int GoogleSearchLimit { get; }

    }
    public class ApplicationConfig : IHostConfig
    {
        public string? GoogleSearchBaseUrl { get; set; } = "https://www.google.com.au/";
        public int GoogleSearchLimit { get; set; } = 100;
    }
}
