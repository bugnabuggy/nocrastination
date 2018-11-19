using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nocrastination.Settings
{
    public class IdentityServerSettings
    {
        public static ApiSettings Api { get; set; }
        public static ClientSettings Client { get; set; }
        public static int AccessTokenLifetime { get; set; }
    }

    public class ApiSettings
    {
        public static string Name { get; set; }
    }

    public class ClientSettings
    {
        public static string Id { get; set; }
        public static string Secret { get; set; }
    }
}
