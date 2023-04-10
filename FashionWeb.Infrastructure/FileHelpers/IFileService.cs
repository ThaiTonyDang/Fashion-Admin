namespace FashionWeb.Infrastructure.FileHelpers
{
    public interface IFileService
    {
        public string GetSystemPath();
        public Task SaveFile(string folderExtra, string filePath, byte[] data);
        public string RefactorFileName(string fileName);
    }
}