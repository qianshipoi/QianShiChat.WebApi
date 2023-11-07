namespace QianShiChat.Application.Contracts.Dtos;

public record GroupJoinToken(string GroupCode, string AuthKey, DateTime ExpirationTime);

public record ValidateGroupTokenResult(bool Successful, GroupTokenPayload Payload);

public record GroupTokenPayload(int GroupId, int Initiator, long ExpirationTime);
