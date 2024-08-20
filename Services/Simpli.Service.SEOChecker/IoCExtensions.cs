using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Simpli.Service.SEOChecker.Configurations;
using Simpli.Service.SEOChecker.Services;
namespace Simpli.Service.SEOChecker
{
    public static class IoCExtensions
    {
        public static void AddServices(this IServiceCollection services, Action<SearchEngineOptions> options)
        {
            services.Configure(options);
            services.AddMediatR(typeof(IoCExtensions).Assembly);
            services.AddHttpClient();
            services.AddScoped<ISearchEngineService, SearchEngineService>();
        }
    }
}