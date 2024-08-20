using Castle.Core.Logging;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Simpli.API;
using Simpli.API.Controllers;
using Simpli.API.Requests.RequestModels;

namespace Simpli.Tests.Controller
{
    [TestClass]
    [TestCategory("Unit")]
    public class SEOControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly SEOController _seoController;
        private readonly Mock<IHostConfig> _hostConfigMock;

        public SEOControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _hostConfigMock = new Mock<IHostConfig>();
            _seoController = new SEOController(_mediatorMock.Object, _hostConfigMock.Object);
        }

        [TestMethod]
        public async Task SEOCheck_ValidRequest_ShouldReturnOK()
        {
            // Arrange
            var model = new SearchEngineRequestModel
            {
                Keyword = "e-settlements",
                Url = "https://www.sympli.com.au"
            };

            // Act
            var result = await _seoController.SearchGoogle(model);

            // Assert
            OkObjectResult okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            var actualValue = (string)okResult.Value;
            actualValue.Should().NotBe(string.Empty);
        }

        [TestMethod]
        public async Task SEOCheck_InValidRequest_ShouldReturnBadRequest()
        {
            // Arrange
            var model = new SearchEngineRequestModel
            {
                Keyword = string.Empty,
                Url = string.Empty
            };

            // Act
            var result = await _seoController.SearchGoogle(model);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
        }
    }
}
