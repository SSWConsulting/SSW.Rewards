﻿using System;
using System.Threading.Tasks;
using Microsoft.AppCenter.Auth;

namespace SSW.Consulting.Services
{
    public interface IUserService
    {
        Task<int> GetMyUserIdAsync();
        Task<string> GetMyNameAsync();
        Task<string> GetMyEmailAsync();
        Task<string> GetMyProfilePicAsync();
        Task<string> GetMyPointsAsync();
        Task<bool> IsLoggedInAsync();
        Task<string> GetTokenAsync();
        Task<bool> SignInAsync();
        Task SignOutAsync();
    }
}
