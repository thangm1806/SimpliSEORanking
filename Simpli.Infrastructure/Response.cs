namespace Simpli.Infrastructure
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public List<string> Errors { get; set; }

        public Response()
        {
            Success = true;
        }
    }
}
