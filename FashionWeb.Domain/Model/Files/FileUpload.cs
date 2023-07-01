using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Model.Files
{
    public class FileUpload
    {
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [JsonPropertyName("fileLink")]
        public string FileLink { get; set; }
    }
}
