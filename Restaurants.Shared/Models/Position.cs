using System;
using System.Collections.Generic;
using System.Text;

namespace Restaurants.Shared.Models
{
    public class Position
    {
        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }

    public class Area
    {
        public Position Position { get; set; }
        public int Radius { get; set; }
    }
}
