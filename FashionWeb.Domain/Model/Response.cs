namespace FashionWeb.Domain.Model
{
    public interface IResponse 
    {
        bool IsSuccess { get; set; }
        string Message { get; set; }
    }

    public class Response<T> : IResponse
    {
        public string Message { get; set; }
        public bool IsSuccess { get; set ; }
        public T Data { get; set; }
        public string [] Errors { get; set; }

    }
}
