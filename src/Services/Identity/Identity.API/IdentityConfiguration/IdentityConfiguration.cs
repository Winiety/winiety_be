using IdentityServer4;
using IdentityServer4.Models;
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
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris = { $"{clientUrls["ReactApp"]}/" },
                    RequireConsent = false,
                    PostLogoutRedirectUris = { $"{clientUrls["ReactApp"]}/" },
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
            };
        }
    }
}
