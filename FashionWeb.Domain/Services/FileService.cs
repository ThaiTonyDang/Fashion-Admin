using FashionWeb.Domain.Extensions;
using FashionWeb.Domain.ResponseModel;
using FashionWeb.Utilities.GlobalHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FashionWeb.Domain.Services
{
    public class FileService : IFileService
    {
        private readonly IUrlService _urlService;
        public FileService(IUrlService urlService)
        {
            _urlService = urlService;
        }
        public async Task<Tuple<ResponseAPI<List<string>>, string>> GetResponeUploadFileAsync(IFormFile file, HttpClient httpClient)
        {
            try
            {
                var fileName = ""; 
                var uploadApiUrl = _urlService.GetBaseUrl() + "api/File/upload";                
                fileName = file.FileName;
                var content = new MultipartFormDataContent();
                
                content.Add(new StreamContent(file.OpenReadStream())
                {
                    Headers =
                    {
                        ContentLength = file.Length,
                        ContentType = new MediaTypeHeaderValue(file.ContentType)
                    }
                }, "File", fileName);

                var response = await httpClient.PostAsync(uploadApiUrl, content);
                var responseList = JsonConvert.DeserializeObject<ResponseAPI<List<string>>>
                                (await response.Content.ReadAsStringAsync());
                var isSuccess = responseList.Success;
                var message = responseList.Message;

                return Tuple.Create(responseList, message);
            }
            catch
            {
                return Tuple.Create(default(ResponseAPI<List<string>>), "An Error Has Occurred Server Side ! Upload Image Fail");
            }         
        }
    }
}
