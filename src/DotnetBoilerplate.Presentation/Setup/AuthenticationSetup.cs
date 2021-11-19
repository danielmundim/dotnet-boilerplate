using DotnetBoilerplate.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Net;

namespace DotnetBoilerplate.API.Setup
{
    public static class AuthenticationSetup
    {
        public static void AddAuthenticationConfiguration(this IServiceCollection services, IApplicationSettings applicationSettings)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (applicationSettings == null) throw new ArgumentNullException(nameof(applicationSettings));

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.SaveToken = true;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKeyResolver = (s, securityToken, identifier, parameters) =>
                        {
                            // Get JsonWebKeySet from AWS
                            var json = new WebClient().DownloadString(parameters.ValidIssuer + "/.well-known/jwks.json");
                            // Serialize the result
                            return JsonConvert.DeserializeObject<JsonWebKeySet>(json).Keys;
                        },
                        ValidIssuer = $"https://cognito-idp.{applicationSettings.AmazonSettings.Region}.amazonaws.com/{applicationSettings.AmazonSettings.CognitoPoolId}",
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        LifetimeValidator = (before, expires, token, param) => expires > DateTime.UtcNow,
                        ValidAudience = applicationSettings.AmazonSettings.CognitoAppClientId,
                        ValidateAudience = true,
                        RoleClaimType = "cognito:groups"
                    };
                });
        }
    }
}
