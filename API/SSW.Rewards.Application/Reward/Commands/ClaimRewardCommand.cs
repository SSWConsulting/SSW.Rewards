﻿
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSW.Rewards.Application.Common.Interfaces;
using SSW.Rewards.Application.Reward.Queries.GetRewardList;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SSW.Rewards.Application.Reward.Commands
{
    public class ClaimRewardCommand : IRequest<ClaimRewardResult>
    {
        public string Code { get; set; }

        public class ClaimRewardCommandHandler : IRequestHandler<ClaimRewardCommand, ClaimRewardResult>
        {
            private readonly ICurrentUserService _currentUserService;
            private readonly ISSWRewardsDbContext _context;
            private readonly IMapper _mapper;
            private readonly IDateTimeProvider _dateTimeProvider;

            public ClaimRewardCommandHandler(
                ICurrentUserService currentUserService,
                ISSWRewardsDbContext context,
                IMapper mapper,
                IDateTimeProvider dateTimeProvider)
            {
                _currentUserService = currentUserService;
                _context = context;
                _mapper = mapper;
                _dateTimeProvider = dateTimeProvider;
            }

            public async Task<ClaimRewardResult> Handle(ClaimRewardCommand request, CancellationToken cancellationToken)
            {
                var reward = await _context
                    .Rewards
                    .Where(r => r.Code == request.Code)
                    .FirstOrDefaultAsync(cancellationToken);

                if(reward == null)
                {
                    return new ClaimRewardResult
                    {
                        status = RewardStatus.NotFound
                    };
                }

                var user = await _currentUserService.GetCurrentUserAsync(cancellationToken);

                var userRewards = await _context
                    .UserRewards
                    .Where(ur => ur.UserId == user.Id)
                    .ToListAsync(cancellationToken);

                int pointsUsed = userRewards.Sum(ur => ur.Reward.Cost);

                int balance = user.Points - pointsUsed;

                if(balance < reward.Cost)
                {
                    return new ClaimRewardResult
                    {
                        status = RewardStatus.NotEnoughPoints
                    };
                }

                // TODO: the following logic is intended to 'debounce' reward
                // claiming, to prevent users claiming the same reward twice
                // within a 5 minute window. With the move from 'milestone' to
                // 'currency' model, this may not be required anymore.
                var userHasReward = userRewards
                    .Where(ur => ur.RewardId == reward.Id)
                    .FirstOrDefault();

                if(userHasReward != null && userHasReward.AwardedAt >= _dateTimeProvider.Now.AddMinutes(-5))
                {
                    return new ClaimRewardResult
                    {
                        status = RewardStatus.Duplicate
                    };
                }

                await _context
                    .UserRewards
                    .AddAsync(new Domain.Entities.UserReward
                    {
                        UserId = user.Id,
                        RewardId = reward.Id
                    }, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var rewardModel = _mapper.Map<RewardViewModel>(reward);

                return new ClaimRewardResult
                {
                    viewModel = rewardModel,
                    status = RewardStatus.Claimed
                };
            }
        }
    }
}
