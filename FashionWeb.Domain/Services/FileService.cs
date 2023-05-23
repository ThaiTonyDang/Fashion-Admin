using FashionWeb.Domain.ResponseModel;
using FashionWeb.Domain.Services.HttpClients;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace FashionWeb.Domain.Services
{
    public class FileService : IFileService
    {
        private readonly IHttpClientService _urlService;
        public FileService(IHttpClientService urlService)
        {
            _urlService = urlService;
        }
        public async Task<Tuple<ResponseApi<List<string>>, string>> GetResponeUploadFileAsync(IFormFile file, HttpClient httpClient)
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
                var responseList = JsonConvert.DeserializeObject<ResponseApi<List<string>>>
                                (await response.Content.ReadAsStringAsync());
                var isSuccess = responseList.IsSuccess;
                var message = responseList.Message;

                return Tuple.Create(responseList, message);
            }
            catch
            {
                return Tuple.Create(default(ResponseApi<List<string>>), "An Error Has Occurred Server Side ! Upload Image Fail");
            }         
        }
    }
}
