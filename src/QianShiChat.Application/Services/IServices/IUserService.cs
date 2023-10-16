namespace QianShiChat.Application.Services.IServices;

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
    /// <param name="avatarPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<UserDto> AddAsync(CreateUserRequest dto, string avatarPath, CancellationToken cancellationToken = default);

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

    /// <summary>
    /// get user nicknanme by id.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<string?> GetNickNameByIdAsync(int userId, CancellationToken cancellationToken = default);

    Task<bool> IsFriendAsync(int userId, int friendId, CancellationToken cancellationToken = default);
    Task<PagedList<UserDto>> SearchUsersAsync(UserSearchRequest request, CancellationToken cancellationToken = default);
}