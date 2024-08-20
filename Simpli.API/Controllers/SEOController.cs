using MediatR;
using Microsoft.AspNetCore.Mvc;
using Simpli.API.Requests.RequestModels;
using Simpli.Infrastructure.MediatR.Requests;
using System.Web;

namespace Simpli.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SEOController : ControllerBase
    {
        private readonly IHostConfig _hostConfig;
        private readonly IMediator _mediator;
        private const string ModelIsNotValid = "Model is invalid.";
        private const string SearchUrlIsInvalid = "Search url is invalid.";
        private const string SearchTermCouldNotBeEmpty = "Search term could not be empty.";

        public SEOController(IMediator mediator, IHostConfig hostConfig)
        {
            _hostConfig = hostConfig;
            _mediator = mediator;
        }

        [HttpGet(nameof(SearchGoogle))]
        public async Task<IActionResult> SearchGoogle([FromQuery] SearchEngineRequestModel request)
        {
            var validation = ValidateModel(request);
            if (!validation.isValid)
                return BadRequest(validation.message);

            var result = await _mediator.Send(new GoogleSearchRequest { SearchTerm = request.Keyword, BaseUrl = _hostConfig.GoogleSearchBaseUrl, SearchLimit = _hostConfig.GoogleSearchLimit, SearchUrl = request.Url });
            return Ok(result);
        }

        private (bool isValid, string message) ValidateModel(SearchEngineRequestModel model)
        {
            if (model == null)
                return (isValid: false, message: ModelIsNotValid);

            if (string.IsNullOrWhiteSpace(model.Url))
                return (isValid: false, message: SearchUrlIsInvalid);

            if (string.IsNullOrWhiteSpace(model.Keyword))
                return (isValid: false, message: SearchTermCouldNotBeEmpty);

            return (isValid: true, message: string.Empty);
        }
    }
}
