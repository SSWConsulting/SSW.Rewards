﻿using System;

namespace SSW.Rewards.Models
{
    public class Reward
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Cost { get; set; }
        public DateTimeOffset? AwardedAt { get; set; }
        public bool Awarded { get; set; }
        public string ImageUri { get; set; }
        public RewardType RewardType { get; set; }
    }
}
