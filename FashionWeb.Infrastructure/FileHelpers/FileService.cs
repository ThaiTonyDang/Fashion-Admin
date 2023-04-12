using FashionWeb.Infrastructure.Config;
using Microsoft.Extensions.Options;

namespace FashionWeb.Infrastructure.FileHelpers
{
    public class FileService : IFileService
    {
        private readonly FileConfig _systemConfig;
        public FileService(IOptions<FileConfig> options)
        {
            this._systemConfig = options.Value;
        }

        public async Task SaveFile(string fileFolder, string ImagePath, byte[] data)
        {
            var imageFolder = Path.Combine(GetSystemPath(), fileFolder);

            if (!Directory.Exists(GetSystemPath()) || !Directory.Exists(imageFolder))
                Directory.CreateDirectory(imageFolder);

            var fullPath = Path.Combine(imageFolder, ImagePath);
            if (File.Exists(fullPath))
                return;

            
            await File.WriteAllBytesAsync(fullPath, data);
        }

        public string RefactorFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return string.Empty;

            var strDateTime = DateTime.Now.ToString("yyyyMMddhhmmss");
            var fileNameOrigin = Path.GetFileNameWithoutExtension(name);
            var extensions = Path.GetExtension(name);

            var fileName = fileNameOrigin.Replace(" ", "").Trim() + strDateTime + extensions;
            return fileName;
        }

        public string GetSystemPath()
        {
            return this._systemConfig.ImagePath;
        }
    }
}