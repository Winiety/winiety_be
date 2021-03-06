﻿using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Identity.API.IdentityConfiguration
{
    public static class IdentityConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("ai", "Full Access for AI Service"),
                new ApiScope("fines", "Full Access for Fines Service"),
                new ApiScope("payment", "Full Access for Payment Service"),
                new ApiScope("pictures", "Full Access for Pictures Service"),
                new ApiScope("rides", "Full Access for Rides Service"),
                new ApiScope("statistics", "Full Access for Statistics Service"),
                new ApiScope("userprofile", "Full Access for Profile Service"),
                new ApiScope("notification", "Full Access for Notification Service"),
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("ai", "AI Service"){ Scopes = { "ai" } },
                new ApiResource("fines", "Fines Service"){ Scopes = { "fines" } },
                new ApiResource("payment", "Payment Service"){ Scopes = { "payment" } },
                new ApiResource("pictures", "Pictures Service"){ Scopes = { "pictures" } },
                new ApiResource("rides", "Rides Service"){ Scopes = { "rides" } },
                new ApiResource("statistics", "Statistics Service"){ Scopes = { "statistics" } },
                new ApiResource("userprofile", "Profile Service"){ Scopes = { "userprofile" } },
                new ApiResource("notification", "Notification Service"){ Scopes = { "notification" } },
            };
        }

        public static IEnumerable<Client> GetClients(Dictionary<string, string> clientUrls)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "react",
                    ClientName = "Winiety React App OpenId Client",
                    ClientUri = $"{clientUrls["ReactApp"]}",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{clientUrls["ReactApp"]}/sign-in" },
                    RequireConsent = false,
                    PostLogoutRedirectUris = { $"{clientUrls["ReactApp"]}/sign-out" },
                    AllowedCorsOrigins = { $"{clientUrls["ReactApp"]}" },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "ai",
                        "fines",
                        "payment",
                        "pictures",
                        "rides",
                        "statistics",
                        "userprofile",
                        "notification"
                    },
                },
                new Client
                {
                    ClientId = "aiswaggerui",
                    ClientName = "AI Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["AIApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["AIApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "ai"
                    }
                },
                new Client
                {
                    ClientId = "finesswaggerui",
                    ClientName = "Fines Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["FinesApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["FinesApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "fines"
                    }
                },
                new Client
                {
                    ClientId = "paymentswaggerui",
                    ClientName = "Payment Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["PaymentApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["PaymentApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "payment"
                    }
                },
                new Client
                {
                    ClientId = "picturesswaggerui",
                    ClientName = "Pictures Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["PicutresApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["PicutresApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "pictures"
                    }
                },
                new Client
                {
                    ClientId = "ridesswaggerui",
                    ClientName = "Rides Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["RidesApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["RidesApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "rides"
                    }
                },
                new Client
                {
                    ClientId = "statisticsswaggerui",
                    ClientName = "Statistics Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["StatisticsApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["StatisticsApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "statistics"
                    }
                },
                new Client
                {
                    ClientId = "userprofileswaggerui",
                    ClientName = "Profile Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["ProfileApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["ProfileApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "userprofile"
                    }
                },
                new Client
                {
                    ClientId = "notificationswaggerui",
                    ClientName = "Notification Swagger UI",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,

                    RedirectUris = { $"{clientUrls["NotificationApi"]}/swagger/oauth2-redirect.html" },
                    PostLogoutRedirectUris = { $"{clientUrls["NotificationApi"]}/swagger/" },

                    AllowedScopes =
                    {
                        "notification"
                    }
                },
            };
        }

        public static IEnumerable<IdentityRole<int>> GetRoles()
        {
            return new List<IdentityRole<int>>
            {
                new IdentityRole<int>() { Name = "user" },
                new IdentityRole<int>() { Name = "admin" },
                new IdentityRole<int>() { Name = "corrector" },
                new IdentityRole<int>() { Name = "analyst" },
                new IdentityRole<int>() { Name = "police" },
            };
        }
    }
}
