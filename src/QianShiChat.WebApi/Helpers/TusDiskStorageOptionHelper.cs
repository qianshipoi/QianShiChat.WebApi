namespace QianShiChat.WebApi.Helpers;

public class TusDiskStorageOptionHelper
{
    public string StorageDiskPath { get; }

    public TusDiskStorageOptionHelper()
    {
        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data", "tusfiles");
        if (!File.Exists(path))
            Directory.CreateDirectory(path);
        StorageDiskPath = path;
    }
}
