using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurants.Shared.Models
{
    public class DbSettings
    {
        public string EndPoint { get; set; }
        public string DbName { get; set; }
        public string ContainerName { get; set; }
    }
}
