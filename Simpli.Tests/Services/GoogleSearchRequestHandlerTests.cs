using Castle.Core.Logging;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
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

namespace Simpli.Tests.Controller
{
    [TestClass]
    [TestCategory("Unit")]
    public class GoogleSearchRequestHandlerTests
    {
        private readonly Mock<ISearchEngineService> _searchServiceMock;
        private readonly GoogleSearchRequestHandler _requestHandler;
        private readonly Mock<IMemoryCache> _memoryCacheMock;
        private readonly IOptions<SearchEngineOptions> _options;

        public GoogleSearchRequestHandlerTests()
        {
            _options = Options.Create(new SearchEngineOptions { CacheExpiryInMinutes = 60 });
            _searchServiceMock = new Mock<ISearchEngineService>();
            _memoryCacheMock = new Mock<IMemoryCache>();
            _requestHandler = new GoogleSearchRequestHandler(_searchServiceMock.Object, _options, _memoryCacheMock.Object);
        }

        [TestMethod]
        public async Task GoogleSearchRequestHandler_EmptyCache_ShouldFetchResultFromSearchService()
        {
            var expectedResult = new Response<GoogleSearchRequest.ResultModel> { Success = true, Data = new GoogleSearchRequest.ResultModel { Result = "1,2,3" } };
            // Arrange
            //_memoryCacheMock.Setup(s => s.Get(It.IsAny<string?>())).Returns(string.Empty);
            _searchServiceMock.Setup(s => s.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SearchEngine>())).ReturnsAsync(expectedResult);
            _memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);
            // Act
            var result = await _requestHandler.Handle(new GoogleSearchRequest(), CancellationToken.None);

            // Assert
            result.Result.Should().Be(expectedResult.Data.Result);
        }

        //[TestMethod]
        //public async Task GoogleSearchRequestHandler_CacheHasValue_ShouldReturnFromCache()
        //{
        //    var expectedResult = "1,2,3";
        //    var expectedResponse = new Response<GoogleSearchRequest.ResultModel> { Success = true, Data = new GoogleSearchRequest.ResultModel { Result = expectedResult } };
        //    // Arrange
        //    var cacheMemory = new CacheMe
        //    _memoryCacheMock
        //   .Setup(mc => mc.TryGetValue(It.IsAny<object>(), out expectedResult))
        //   .Callback(new OutDelegate<object, object>((object k, out object v) =>
        //       v = new object())) // mocked value here (and/or breakpoint)
        //   .Returns(true);
        //    //_memoryCacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

        //    _searchServiceMock.Setup(s => s.SearchAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<SearchEngine>())).ReturnsAsync(new Response<GoogleSearchRequest.ResultModel>());

        //    // Act
        //    var result = await _requestHandler.Handle(new GoogleSearchRequest(), CancellationToken.None);

        //    // Assert
        //    result.Result.Should().Be(expectedResult);
        //}
    }
}
