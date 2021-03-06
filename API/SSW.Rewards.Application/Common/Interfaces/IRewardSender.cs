﻿using System.Threading;
using System.Threading.Tasks;

namespace SSW.Rewards.Application.Common.Interfaces
{
    public interface IRewardSender
    {
        Task SendReward(SSW.Rewards.Domain.Entities.User user, SSW.Rewards.Domain.Entities.Reward reward);
        Task SendRewardAsync(SSW.Rewards.Domain.Entities.User user, SSW.Rewards.Domain.Entities.Reward reward, CancellationToken cancellationToken);
    }
}
