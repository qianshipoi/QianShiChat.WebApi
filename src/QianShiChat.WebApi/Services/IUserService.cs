using QianShiChat.Models;
using QianShiChat.WebApi.Models;

namespace QianShiChat.WebApi.Services
{
    public interface IUserService
    {
        Task<UserDto> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);

        Task<UserInfo?> GetUserByAccountAsync(string account, CancellationToken cancellationToken = default);

        Task<bool> AccountExistsAsync(string account, CancellationToken cancellationToken = default);

        Task AddAsync(CreateUserDto dto,CancellationToken cancellationToken = default);
    }
}