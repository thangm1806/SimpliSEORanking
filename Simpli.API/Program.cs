using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Simpli.API;
using Simpli.API.Middlewares;
using Simpli.Service.SEOChecker;

var builder = WebApplication.CreateBuilder(args);

// Build configuration
string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).AddJsonFile("appsettings." + envName + ".json", optional: true, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();

Func<IServiceProvider, ApplicationConfig> applicationConfigFunc = x => x.GetRequiredService<IOptionsMonitor<ApplicationConfig>>().CurrentValue;
builder.Services.Configure<ApplicationConfig>(builder.Configuration);
builder.Services.AddScoped<IHostConfig>(applicationConfigFunc);

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Search Engine API", Version = "v1" });
});


// Add services to the container.
builder.Services.AddServices(options => options.CacheExpiryInMinutes = int.Parse(builder.Configuration.GetSection("CacheExpiryInMinutes").Value ?? "0"));
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpExceptionLoggingMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();