using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Model
{
    public class ErrorResponseApi : BaseReponseApi
    {
        [JsonPropertyName("errors")]
        public string[] Errors { get; set; }
    }
}
