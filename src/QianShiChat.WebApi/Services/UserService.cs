using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using QianShiChat.Models;
using QianShiChat.WebApi.Core.AutoDI;
using QianShiChat.WebApi.Models;

using System.Drawing;

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

        public async Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.UserInfos
                .AsNoTracking()
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task<UserInfo?> GetUserByAccountAsync(string account, CancellationToken cancellationToken = default)
        {
            return await _context.UserInfos
                .Where(x => x.Account == account)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> AccountExistsAsync(string account, CancellationToken cancellationToken = default)
        {
            return await _context.UserInfos
                 .AsNoTracking()
                 .AnyAsync(x => x.Account.Equals(account), cancellationToken);
        }

        public async Task<UserDto> AddAsync(CreateUserDto dto, CancellationToken cancellationToken = default)
        {
            var user = _mapper.Map<UserInfo>(dto);
            await _context.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<UserDto>(user);
        }

        public async Task<List<UserDto>> GetUserByAccontAsync(string account, CancellationToken cancellationToken = default)
        {
            return await _context.UserInfos.AsNoTracking()
                .Where(x => x.Account == account)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserDto>> GetUserByNickNameAsync(int page, int size, string nickName, CancellationToken cancellationToken = default)
        {
            var users = await _context.UserInfos.AsNoTracking()
                .Where(x => EF.Functions.Like(x.NickName, $"%{nickName}%"))
                .OrderBy(x => x.CreateTime)
                .Skip((page - 1) * size)
                .Take(size)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<long> GetUserCountByNickNameAsync(string nickName, CancellationToken cancellationToken = default)
        {
            return await _context.UserInfos.AsNoTracking()
                    .Where(x => EF.Functions.Like(x.NickName, $"%{nickName}%"))
                    .CountAsync(cancellationToken);
        }
    }
}
