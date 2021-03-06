﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSW.Rewards.Application.Common.Interfaces;
using SSW.Rewards.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SSW.Rewards.Application.Achievement.Command.ClaimAchievementForUser
{
    public class ClaimAchievementForUserCommand : IRequest<ClaimAchievementResult>
    {
        public int UserId { get; set; }
        public string Code { get; set; }
    }

    public class ClaimAchievementForUserCommandHandler : IRequestHandler<ClaimAchievementForUserCommand, ClaimAchievementResult>
    {
        private readonly ISSWRewardsDbContext _context;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly ILogger<ClaimAchievementForUserCommand> _logger;

        public ClaimAchievementForUserCommandHandler(
                ISSWRewardsDbContext context,
                IDateTimeProvider dateTimeProvider,
                ILogger<ClaimAchievementForUserCommand> logger)
        {
            _context = context;
            _dateTimeProvider = dateTimeProvider;
            _logger = logger;
        }

        public async Task<ClaimAchievementResult> Handle(ClaimAchievementForUserCommand request, CancellationToken cancellationToken)
        {
            var achievement = await _context
                .Achievements
                .Where(a => a.Code == request.Code)
                .FirstOrDefaultAsync(cancellationToken);

            if (achievement == null)
            {
                _logger.LogError("Achievement was not found for code: {0}", request.Code);
                return new ClaimAchievementResult
                {
                    status = AchievementStatus.NotFound
                };
            }

            var user = await _context.Users
                .Where(u => u.Id == request.UserId)
                .Include(u => u.UserAchievements).ThenInclude(ua => ua.Achievement)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                _logger.LogError("User was not found for id: {0}", request.UserId);
                return new ClaimAchievementResult
                {
                    status = AchievementStatus.Error
                };
            }

            var achievementCheck = user
                .UserAchievements
                .Where(ua => ua.Achievement.Code == request.Code)
                .FirstOrDefault();

            if (achievementCheck != null)
            {
                _logger.LogError("User already has achievement: {0}", request.Code);
                return new ClaimAchievementResult
                {
                    status = AchievementStatus.Duplicate
                };
            }

            try
            {
                await _context
                    .UserAchievements
                    .AddAsync(new UserAchievement
                    {
                        UserId = user.Id,
                        AchievementId = achievement.Id
                    }, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                return new ClaimAchievementResult
                {
                    status = AchievementStatus.Error
                };
            }

            return new ClaimAchievementResult
            {
                status = AchievementStatus.Claimed
            };
        }
    }
}