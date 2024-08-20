using Castle.Core.Logging;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using OneOf.Types;
using Simpli.API;
using Simpli.API.Controllers;
using Simpli.API.Requests.RequestModels;
using Simpli.Infrastructure;
using Simpli.Infrastructure.MediatR.RequestHandlers;
using Simpli.Infrastructure.MediatR.Requests;
using Simpli.Service.SEOChecker.Builder;
using Simpli.Service.SEOChecker.Configurations;
using Simpli.Service.SEOChecker.Services;
using System.Net;

namespace Simpli.Tests.Controller
{
    [TestClass]
    [TestCategory("Unit")]
    public class SearchEngineServiceTests
    {
        private readonly Mock<IHttpClientFactory> _clientFactoryMock;
        private readonly ISearchEngineService _searchService;

        public SearchEngineServiceTests()
        {
            _clientFactoryMock = new Mock<IHttpClientFactory>();
            _searchService = new SearchEngineService(_clientFactoryMock.Object);
        }

        [TestMethod]
        public async Task SearchEngineService_FetchNoResult_ShouldReturnError()
        {
            var expectedResult = new Response<GoogleSearchRequest.ResultModel> { Success = true, Data = new GoogleSearchRequest.ResultModel { Result = "1,2,3" } };
            // Arrange

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(string.Empty)
                });


            var httpClient = new HttpClient(mockMessageHandler.Object);

            _clientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            // Act
            var result = await _searchService.SearchAsync("https://google.com", string.Empty, 0, SearchEngine.Google);

            // Assert
            result.Success.Should().Be(false);
        }

        [TestMethod]
        public async Task SearchEngineService_FetchMultiSearchResult_ShouldReturnValueSuccessfully()
        {
            var expectedResult = "www.sympli.com.au";
            // Arrange

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(expectedResult)
                });


            var httpClient = new HttpClient(mockMessageHandler.Object);

            _clientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            // Act
            var result = await _searchService.SearchAsync("https://google.com", expectedResult, 100, SearchEngine.Google);

            // Assert
            result.Success.Should().Be(true);
        }
    }
}
