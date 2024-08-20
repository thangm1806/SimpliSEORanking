using MediatR;

namespace Simpli.Infrastructure.MediatR.Requests
{
    public class GoogleSearchRequest : IRequest<GoogleSearchRequest.ResultModel>
    {
        public string? SearchTerm { get; set; }
        public int SearchLimit {  get; set; }
        public string? SearchUrl { get; set; }
        public string? BaseUrl { get; set; }

        public class ResultModel
        {
            public string? Result {  get; set; }
        }
    }
}
