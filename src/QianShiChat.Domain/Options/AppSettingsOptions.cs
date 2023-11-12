namespace QianShiChat.Domain.Options;

public class AppSettingsOptions
{
    public const string OptionsKey = "AppSettings";
    public string ApiUrl { get; set; } = string.Empty;
    public string WebUrl { get; set; } = string.Empty;
}
