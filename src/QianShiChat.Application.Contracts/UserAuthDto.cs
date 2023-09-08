namespace QianShiChat.Application.Contracts;

public record UserAuthDto([property: Required, MaxLength(32)] string Account, [property: Required, MaxLength(32)] string Password);
