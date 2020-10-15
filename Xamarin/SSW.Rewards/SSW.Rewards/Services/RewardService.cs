using SSW.Rewards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SSW.Rewards.Services
{
    public class RewardService : IRewardService
    {
        private readonly IUserService _userService;

        private RewardClient _rewardClient { get; set; }
        private HttpClient _httpClient { get; set; }

        public RewardService(IUserService userService)
        {
            _httpClient = new HttpClient();
            _ = Initialise();
            _userService = userService;
        }

        private async Task Initialise()
        {
            string token = await _userService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string baseUrl = App.Constants.ApiBaseUrl;

            _rewardClient = new RewardClient(baseUrl, _httpClient);
        }

        public async Task<List<Reward>> GetRewards()
        {
            List<Reward> rewardList = new List<Reward>();

            var rewards = await _rewardClient.ListAsync();
            
            foreach(var reward in rewards.Rewards)
            {
                rewardList.Add(new Reward
                {
                    Cost = reward.Cost,
                    Id = reward.Id,
                    ImageUri = reward.ImageUri,
                    Name = reward.Name
                });
            }

            return rewardList;
        }

        public async Task<ClaimRewardResult> RedeemReward(Reward reward)
        {
            throw new NotImplementedException();
        }

        public async Task<ClaimRewardResult> RewardRewardWithQRCode(string QRCode)
        {
            throw new NotImplementedException();
        }
    }
}
