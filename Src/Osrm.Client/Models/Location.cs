using System;

namespace Osrm.Client.Models
{
    public class Location : IEquatable<Location>
    {
        public Location()
        {
        }

        public Location(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Location other && Equals(other);
        }

        public bool Equals(Location? p)
        {
            if (p is null)
            {
                return false;
            }

            return (Latitude == p.Latitude) && (Longitude == p.Longitude);
        }

        public override int GetHashCode()
        {
            return Latitude.GetHashCode() ^ Longitude.GetHashCode();
        }

        public static bool operator ==(Location? a, Location? b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if ((a is null) || (b is null))
            {
                return false;
            }

            return a.Latitude == b.Latitude && a.Longitude == b.Longitude;
        }

        public static bool operator !=(Location? a, Location? b)
        {
            return !(a == b);
        }
    }
}