﻿using FluentValidation;

namespace SSW.Rewards.Application.Reward.Commands
{
    public class ClaimRewardForUserCommandValidator : AbstractValidator<ClaimRewardForUserCommand>
    {
        public ClaimRewardForUserCommandValidator()
        {
            RuleFor(x => x.Code).NotEmpty().MaximumLength(128);
        }
    }
}
