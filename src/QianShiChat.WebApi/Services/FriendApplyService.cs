﻿using AutoMapper;
using AutoMapper.QueryableExtensions;

using Microsoft.EntityFrameworkCore;

using QianShiChat.Common.Models;
using QianShiChat.Models;
using QianShiChat.WebApi.Core.AutoDI;
using QianShiChat.WebApi.Models;

namespace QianShiChat.WebApi.Services
{
    public class FriendApplyService : IFriendApplyService, ITransient
    {
        private readonly ILogger<FriendApplyService> _logger;
        private readonly ChatDbContext _context;
        private readonly IMapper _mapper;

        public FriendApplyService(ILogger<FriendApplyService> logger, ChatDbContext context, IMapper mapper)
        {
            _logger = logger;
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> IsApplyAsync(int userId, int friendId, CancellationToken cancellationToken = default)
        {
            return await _context.FriendApplies
                .Where(x => x.UserId == userId && x.FriendId == friendId && x.Status == ApplyStatus.Applied)
                .AnyAsync(cancellationToken);
        }

        public async Task<bool> IsApprovalAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.FriendApplies
                .Where(x => x.Id == id && x.Status != ApplyStatus.Applied)
                .AnyAsync(cancellationToken);
        }

        public async Task<FriendApplyDto> ApplyAsync(int userId, CreateFriendApplyDto dto, CancellationToken cancellationToken = default)
        {
            var apply = _mapper.Map<FriendApply>(dto);
            apply.UserId = userId;
            await _context.AddAsync(apply, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return _mapper.Map<FriendApplyDto>(apply);
        }

        public async Task<FriendApplyDto> UpdateApplyAsync(int userId, CreateFriendApplyDto dto, CancellationToken cancellationToken = default)
        {
            var apply = await _context.FriendApplies
                .Where(x => x.UserId == userId && x.FriendId == dto.UserId && x.Status == ApplyStatus.Applied)
                .SingleOrDefaultAsync(cancellationToken);

            apply.Remark = dto.Remark;
            apply.LaseUpdateTime = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<FriendApplyDto>(apply);
        }

        public async Task<List<FriendApplyDto>> GetPendingListByUserAsync(int page, int size, int userId, CancellationToken cancellationToken = default)
        {
            return await _context.FriendApplies
                  .AsNoTracking()
                  .Where(x => x.UserId == userId)
                  .OrderByDescending(x => x.LaseUpdateTime)
                  .Skip((page - 1) * size)
                  .Take(size)
                  .ProjectTo<FriendApplyDto>(_mapper.ConfigurationProvider)
                  .ToListAsync(cancellationToken);
        }

        public async Task<long> GetPendingListCountByUserAsync(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.FriendApplies
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.LaseUpdateTime)
                .CountAsync(cancellationToken);
        }

        public async Task<FriendApplyDto> ApprovalAsync(int userId, int id, ApplyStatus status, CancellationToken cancellationToken = default)
        {
            var apply = await _context.FriendApplies
                .Include(x => x.User)
                .Include(x => x.Friend)
                .Where(x => x.UserId == userId && x.Id == id && x.Status == ApplyStatus.Applied)
                .FirstAsync(cancellationToken);

            apply.Status = status;
            apply.LaseUpdateTime = DateTime.Now;

            if (status == ApplyStatus.Passed
                && !await _context.UserRealtions.AnyAsync(x => x.UserId == apply.UserId && x.FriendId == apply.FriendId, cancellationToken))
            {
                _context.UserRealtions.Add(new UserRealtion
                {
                    UserId = apply.FriendId,
                    FriendId = apply.UserId,
                    CreateTime = apply.LaseUpdateTime
                });

                _context.UserRealtions.Add(new UserRealtion
                {
                    UserId = apply.UserId,
                    FriendId = apply.FriendId,
                    CreateTime = apply.LaseUpdateTime
                });
            }

            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<FriendApplyDto>(apply);
        }
    }
}