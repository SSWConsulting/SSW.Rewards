using SSW.Rewards.Models;
using SSW.Rewards.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SSW.Rewards.ViewModels
{
    public class RewardDetailsViewModel
    {
        private readonly IRewardService _rewardService;
        private readonly IUserService _userService;

        public string Name { get; set; }
        public string ImageUri { get; set; }
        public int Cost { get; set; }
        public RewardType RewardType { get; set; }

        public ICommand RedeemTapped { get; set; }

        public bool RedeemEnabled { get; set; } = false;

        public RewardDetailsViewModel(
            Reward reward,
            IRewardService rewardService,
            IUserService userService)
        {
            Reward = reward;
            _rewardService = rewardService;
            _userService = userService;
            Name = reward.Name;
            Cost = reward.Cost;
            ImageUri = reward.ImageUri;
            RewardType = reward.RewardType;
        }

        private Reward Reward { get; }
    }
}
