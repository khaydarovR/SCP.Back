﻿
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SCP.Application.Core.Access;
using SCP.Application.Core.ApiKeyC;
using SCP.Application.Core.Record;
using SCP.Application.Core.Safe;
using SCP.Application.Core.ApiKey;
using SCP.Application.Core.UserAuth;
using SCP.Application.Services;
using SCP.DAL;
using SCP.Domain.Entity;
using System.Net.NetworkInformation;
using System.Reflection;

namespace SCP.Application.Common.Configuration
{
    public static class ConfigureServicesExtensions
    {
        public static IServiceCollection ConfigureApplication(this IServiceCollection services, IConfiguration config)
        {
            //конфигурация приложения
            services.InjectDB(config);
            services.Configure<MyOptions>(config.GetSection(nameof(MyOptions)));

            services.AddStackExchangeRedisCache(options => {
                options.Configuration = "localhost";
                options.InstanceName = "local";
            });

            services.AddIdentity<AppUser, IdentityRole<Guid>>(options =>
            {
                options.Tokens.EmailConfirmationTokenProvider = "email";
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<Microsoft.AspNetCore.Identity.EmailTokenProvider<AppUser>>("email")
                .AddUserManager<UserManager<AppUser>>()
                .AddErrorDescriber<IdentityMessageRu>();


            services.AddScoped<JwtService>();
            services.AddSingleton<SymmetricCryptoService>();
            services.AddSingleton<AsymmetricCryptoService>();
            services.AddTransient<EmailService>();
            services.AddTransient<TwoFactorAuthService>();
            services.AddTransient<RLogService>();
            services.Configure<EmailServiceOptions>(config.GetSection("EmailService"));
            services.AddScoped<CacheService>();

            services.AddScoped<UserAuthCore>();
            services.AddScoped<SafeCore>();
            services.AddScoped<RecordCore>();
            services.AddScoped<AccessCore>();
            services.AddScoped<SafeGuardCore>();
            services.AddScoped<ApiKeyCore>();



            return services;
        }
    }
}
