using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;

namespace Nocrastination.Settings
{
    public class IdentityServerConfig
    {

	    public static IEnumerable<IdentityResource> GetIdentityResources()
	    {
		    return new List<IdentityResource>
		    {
			    new IdentityResources.OpenId(),
			    new IdentityResources.Profile(),
		    };
	    }

		// Defining the API
		public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource(IdentityServerSettings.Api, "api")
            };
        }

        // Defining the client
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = IdentityServerSettings.Client,
                    ClientName = "Frontend",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    AccessTokenType = AccessTokenType.Jwt,
                    AccessTokenLifetime = IdentityServerSettings.AccessTokenLifetime,

                    ClientSecrets =
                    {
                        new Secret(IdentityServerSettings.Secret.Sha256())
                    },

                    AllowedScopes =
                    {
	                    IdentityServerConstants.StandardScopes.OpenId,
	                    IdentityServerConstants.StandardScopes.Profile,
	                    IdentityServerSettings.Api
					},

                    AllowOfflineAccess = true
                }
            };
        }
    }
}
