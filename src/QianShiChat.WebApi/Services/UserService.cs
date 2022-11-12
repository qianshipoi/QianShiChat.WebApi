using AutoMapper;

using Microsoft.EntityFrameworkCore;

using QianShiChat.Models;
using QianShiChat.WebApi.Core.AutoDI;
using QianShiChat.WebApi.Models;

namespace QianShiChat.WebApi.Services
{
    public class UserService : IUserService, ITransient
    {
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly ChatDbContext _context;

        public UserService(ChatDbContext context, IMapper mapper, ILogger<UserService> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserDto> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _context.UserInfos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserInfo?> GetUserByAccountAsync(string account, CancellationToken cancellationToken = default)
        {
            return await _context.UserInfos.Where(x => x.Account == account).FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> AccountExistsAsync(string account, CancellationToken cancellationToken = default)
        {
            return await _context.UserInfos
                 .AsNoTracking()
                 .AnyAsync(x => x.Account.Equals(account), cancellationToken);
        }

        public async Task AddAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserInfo>(dto);
            await _context.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
