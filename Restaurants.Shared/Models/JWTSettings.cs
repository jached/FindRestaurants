using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurants.Shared.Models
{
    public class JWTSettings
    {
        public string ValidAudience { get; set; }
        public string ValidIssuer { get; set; }
        public string ExpirationInHours { get; set; }
    }
}
