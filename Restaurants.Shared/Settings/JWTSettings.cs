

namespace Restaurants.Shared.Settings
{
    public class JWTSettings
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string ExpirationInHours { get; set; }
    }
}
