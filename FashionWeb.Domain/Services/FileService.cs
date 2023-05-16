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
        public async Task<List<string>> UploadFileAsync(IFormFile file, HttpClient httpClient)
        {
            try
            {
                var uploadApiUrl = _urlService.GetBaseUrl() + "api/File/upload";
                var fileName = file.FileName;
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
                if (isSuccess)
                {
                    return responseList.Data;
                }
                 
                return new List<string>();
            }
            catch (Exception exception)
            {
                return null;
            }         
        }
    }
}
