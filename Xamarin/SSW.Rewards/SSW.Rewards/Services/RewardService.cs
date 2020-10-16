using SSW.Rewards.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            _userService = userService;
            _ = Initialise();
        }

        private async Task Initialise()
        {
            string token = await _userService.GetTokenAsync();
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            

            string baseUrl = App.Constants.ApiBaseUrl;

            _rewardClient = new RewardClient(baseUrl, _httpClient);
            Debug.WriteLine("Reward client instantiated");
        }

        public async Task<List<Reward>> GetRewards()
        {
            try
            {
                Debug.WriteLine("Rewards service getrewards called");
                List<Reward> rewardList = new List<Reward>();
                Debug.WriteLine("Instantiated blank rewards list");

                Debug.WriteLine("Calling getrewards from reward client");
                var rewards = await _rewardClient.ListAsync();
                Debug.WriteLine("Got rewards from reward client");

                Debug.WriteLine("Adding rewards from client to blank list");

                foreach (var reward in rewards.Rewards)
                {
                    rewardList.Add(new Reward
                    {
                        Cost = reward.Cost,
                        Id = reward.Id,
                        ImageUri = reward.ImageUri,
                        Name = reward.Name
                    });
                }

                Debug.WriteLine("Returning rewards list");
                return rewardList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
