namespace QianShiChat.Common.Helpers;

public class FileHelper
{
    public static string[] ImageExts = new string[] { ".jpeg", ".jpg", ".png", ".gif" };
    public static string[] VideoExts = new string[] { ".mp4", ".avi" };

    public static bool IsAllowImages(string filename)
    {
        var ext = Path.GetExtension(filename).ToLower();
        return ImageExts.Any(x => x == ext);
    }

    public static bool IsAllowVideos(string filename)
    {
        var ext = Path.GetExtension(filename).ToLower();
        return VideoExts.Any(x => x == ext);
    }
}