namespace QianShiChat.WebApi.Services;

/// <summary>
/// user service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// get user by id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// get user by account.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserInfo?> GetUserByAccountAsync(string account, CancellationToken cancellationToken = default);

    /// <summary>
    /// check account exist.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<bool> AccountExistsAsync(string account, CancellationToken cancellationToken = default);

    /// <summary>
    /// add user.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserDto> AddAsync(CreateUserDto dto, string avatarPath, CancellationToken cancellationToken = default);

    /// <summary>
    /// get user by account.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<UserDto>> GetUserByAccontAsync(string account, CancellationToken cancellationToken = default);

    /// <summary>
    /// get user by nickname.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="size"></param>
    /// <param name="nickName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<List<UserDto>> GetUserByNickNameAsync(int page, int size, string nickName, CancellationToken cancellationToken = default);

    /// <summary>
    /// get user count by nickname.
    /// </summary>
    /// <param name="nickName"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<long> GetUserCountByNickNameAsync(string nickName, CancellationToken cancellationToken = default);
}