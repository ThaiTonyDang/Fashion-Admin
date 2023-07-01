using FashionWeb.Domain.Model;
using System.Text.Json.Serialization;

namespace FashionWeb.Domain.ResponseModel
{
    public class ResponseDataApi<T> : BaseReponseApi
    {
        [JsonPropertyName("data")]
        public T Data { get; set; }
    }
}
