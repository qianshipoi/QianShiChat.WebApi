using AutoMapper;

using Microsoft.EntityFrameworkCore;

using QianShiChat.Models;
using QianShiChat.WebApi.Core.AutoDI;
using QianShiChat.WebApi.Models;

using System.Net;

namespace QianShiChat.WebApi.Services
{
    public class FriendService : IFriendService, ITransient
    {
        private readonly ChatDbContext _context;
        private readonly ILogger<FriendService> _logger;
        private readonly IMapper _mapper;

        public FriendService(ChatDbContext context, ILogger<FriendService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<bool> IsFriendAsync(int userId, int friendId, CancellationToken cancellationToken = default)
        {
            return await _context.UserRealtions
                .AnyAsync(x => x.UserId == userId && x.FriendId == friendId, cancellationToken);
        }

        public async Task<List<int>> GetFriendIdsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserRealtions.AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.FriendId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<UserDto>> GetFriendsAsync(int userId, CancellationToken cancellationToken = default)
        {
            var friends = await _context.UserRealtions
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .Include(x => x.Friend)
                .Select(x => x.Friend)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<UserDto>>(friends);
        }
    }
}
