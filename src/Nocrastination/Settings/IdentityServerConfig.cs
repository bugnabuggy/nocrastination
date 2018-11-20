using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;

namespace Nocrastination.Settings
{
    public class IdentityServerConfig
    {
        // Defining the API
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(ApiSettings.Name, "api")
            };
        }

        // Defining the client
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = ClientSettings.Id,
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = IdentityServerSettings.AccessTokenLifetime,

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret(ClientSettings.Secret.Sha256())
                    },

                    AllowedScopes =
                    {
                        ApiSettings.Name
                    },

                    AllowOfflineAccess = true
                }
            };
        }
    }
}
