namespace QianShiChat.Application.Services;

public class FileOnlineTransmission
{
    public string Id { get; init; }
    public int FromId { get; init; }
    public int ToId { get; init; }
    public FileBaseInfo FileInfo { get; init; }
    public TransmissionStatus Status { get; private set; }
    [JsonIgnore]
    public Channel<string>? FileChannel { get; private set; }
    public string? ClientType { get; private set; }

    public FileOnlineTransmission(string id, int fromId, int toId, FileBaseInfo fileInfo)
    {
        Id = id;
        FromId = fromId;
        ToId = toId;
        FileInfo = fileInfo;
    }

    public void PassedClient(string clientType)
    {
        ClientType = clientType;
        FileChannel = Channel.CreateUnbounded<string>();
    }

    public void Cancel()
    {
        Status = TransmissionStatus.Cancel;
        FileChannel?.Writer.Complete(new Exception("user cancel."));
    }
}

public enum TransmissionStatus
{
    Confirm,
    Passed,
    Reject,
    Cancel,
    Transmitting,
}
