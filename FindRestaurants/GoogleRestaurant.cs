using System.Collections.Generic;

namespace FindRestaurants
{
    public class GoogleResult
    {
        public List<GoogleRestaurant> Results { get; set; }
    }

    public class GoogleRestaurant
    {
        public Location Geometry { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Vicinity { get; set; }
    }

    public class Location
    {
        public string Lat { get; set; }
        public string Lng { get; set; }
    }
}
