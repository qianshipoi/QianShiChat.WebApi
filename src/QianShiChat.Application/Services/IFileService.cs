namespace QianShiChat.Application.Services
{
    public interface IFileService
    {
        /// <summary>
        /// 格式化链接
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string FormatWwwRootFile(string filePath);
    }
}