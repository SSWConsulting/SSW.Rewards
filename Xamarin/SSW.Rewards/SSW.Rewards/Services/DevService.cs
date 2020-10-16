﻿using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using SSW.Rewards.Models;
using Xamarin.Forms;

namespace SSW.Rewards.Services
{
    public class DevService : IDevService
    {
        private StaffClient _staffClient;
        private HttpClient _httpClient;
        private IUserService _userService;

        public DevService(IUserService userService)
        {
            _userService = userService;
            _httpClient = new HttpClient();
            _ = Initialise();
        }

        private async Task Initialise()
        {
            string token = await _userService.GetTokenAsync();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string baseUrl = App.Constants.ApiBaseUrl;

            _staffClient = new StaffClient(baseUrl, _httpClient);
        }

        public async Task<IEnumerable<DevProfile>> GetProfilesAsync()
        {
			List<DevProfile> profiles = new List<DevProfile>();
            int id = 0;

            try
            {
                StaffListViewModel profileList = await _staffClient.GetAsync();

                foreach (StaffDto profile in profileList.Staff)
                {
                    DevProfile dev = new DevProfile
                    {
                        id = id,
                        FirstName = profile.Name,
                        Bio = profile.Profile,
                        Email = profile.Email,
                        Picture = string.IsNullOrWhiteSpace(profile.ProfilePhoto?.ToString()) ? "dev_placeholder" : profile.ProfilePhoto.ToString(),
						Title = profile.Title,
                        TwitterID = profile.TwitterUsername,
						Skills = profile.Skills?.ToList(),
                        IsExternal = profile.IsExternal
					};

                    profiles.Add(dev);
                    id++;
                }
            }
            catch (ApiException e)
            {
                if (e.StatusCode == 401)
                {
                    await App.Current.MainPage.DisplayAlert("Authentication Failure", "Looks like your session has expired. Choose OK to go back to the login screen.", "OK");
                    Application.Current.MainPage = new SSW.Rewards.Views.LoginPage();
                }
                else
                {
                    await App.Current.MainPage.DisplayAlert("Oops...", "There seems to be a problem loading the profiles. Please try again soon.", "OK");
                }
            }

            return profiles.OrderBy(d => d.FirstName);
        }
    }
}
