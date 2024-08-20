# Solution Overview

## How to run project
- Find to the solution folder.
- Run project by .net cli: dotnet build , dotnet restore, dotnet run
- Input search key and url string on swaggerAPI UI such as "e-settlements" and "www.sympli.com.au"
- Output will be the array of positions which matching with url string that appear on search result (Google,bing)

## Architecture
C#/.NET 8 WebAPI architecture

## Implementations
- Apply APIController class for exposes API endpoints.
- Use Mediator as a simplified request-response handling component, it help to archive loose coupling between controllers and bussiness services, 
it made the code more cleaner and modular, easy to understand and maintenance.
- Apply IMemoryCache for caching search result in application memory. But it could be replaced by other distributed cache system (like Redis) when the system growing rapidly and need to be scaled out.
- Apply ILogger for logs errors, warnings, and messages for debugging. Can integrate with service like Azure Application Insights for tracking and monitoring in production.
- Organizing application configurations (IHostConfig) to allow read config from setting file (appsettings.json) or azure application configuration (AAC).
- Utilize IHttpClientFactory for creating instance of httpClient efficently and avoid facing DNS issues.
- Using Moq, FluentAssertions for unit testing.