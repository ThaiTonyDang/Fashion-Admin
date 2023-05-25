using System.Text.Json.Serialization;

namespace FashionWeb.Domain.Model
{
    public class BaseReponseApi
    {
        [JsonPropertyName("statusCode")]
        public int StatusCode { get; set; }

        [JsonPropertyName("isSuccess")]
        public bool IsSuccess { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
