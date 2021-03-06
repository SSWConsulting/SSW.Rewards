﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SSW.Rewards.Application.Common.Interfaces;
using System;

namespace SSW.Rewards.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Add Secrets Provider
#if DEBUG
            bool useLocalSecrets = Convert.ToBoolean(configuration["UseLocalSecrets"]);
            if (useLocalSecrets)
            {
                services.AddSingleton<ISecretsProvider, LocalSecretsProvider>();
            }
            else
#endif
            {
                services.AddSingleton<ISecretsProvider, KeyVaultSecretsProvider>();
                services.AddSingleton<IKeyVaultClientProvider, KeyVaultClientProvider>();
            }

            // ConfigureStorageProviders
            services.AddSingleton<ICloudBlobClientProvider, CloudBlobClientProvider>();
            services.AddSingleton<IStorageProvider, AzureStorageProvider>();

            services.AddScoped<IProfileStorageProvider, ProfileStorageProvider>();
            services.AddScoped<IProfilePicStorageProvider, ProfilePicStorageProvider>();
            services.AddScoped<IRewardPicStorageProvider, RewardPicStorageProvider>();

            services.AddSingleton<IDateTimeProvider, DefaultDateTimeProvider>();

            services.AddScoped<IEmailService, EmailService>();

            services.AddScoped<IRewardSender, RewardSender>();


            SMTPSettings smtpSettings = new SMTPSettings();

            configuration.Bind("SMTPSettings", smtpSettings);

            services.AddFluentEmail(smtpSettings.DefaultSender)
                .AddRazorRenderer()
                .AddSmtpSender(smtpSettings.Host, smtpSettings.Port);

            return services;
        }
    }
}
