using IdentityServer4.Models;
using System.Collections.Generic;

namespace Identity.API.IdentityConfiguration
{
    public static class IdentityConfiguration
    {
        public static IEnumerable<IdentityResource> GetResources()
        {
            return new List<IdentityResource>();
        }

        public static IEnumerable<ApiScope> GetScopes()
        {
            return new List<ApiScope>();
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>();
        }
    }
}
