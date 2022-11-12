using AutoMapper;

using Microsoft.EntityFrameworkCore;

using QianShiChat.WebApi.Core.AutoDI;
using QianShiChat.WebApi.Models;

namespace QianShiChat.WebApi.Services
{
    public class FirendService : IFirendService, ITransient
    {
        private readonly ChatDbContext _context;
        private readonly ILogger<FirendService> _logger;
        private readonly IMapper _mapper;

        public FirendService(ChatDbContext context, ILogger<FirendService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<int>> GetFirendIdsAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserRealtions.AsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.FriendId)
                .ToListAsync(cancellationToken);
        }
    }
}
