using System.Text.Json.Serialization;

namespace FashionWeb.Domain.ResponseModel
{
    public class ResponseApi<T>
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("errors")]
        public string[] Errors { get; set; }

        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
