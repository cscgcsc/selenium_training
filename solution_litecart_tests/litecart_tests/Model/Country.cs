using System;
using System.Collections.Generic;

namespace litecart_tests
{
    public class Country : IComparable, IEquatable<Country>
    {
        public Country()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string CodeAlpha3 { get; set; }
        public string CodeAlpha2 { get; set; }
        public List<Zone> CountryZones { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Country other)
                return Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Object is not a Country");
        }

        public bool Equals(Country other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other) || Id == other.Id && Name == other.Name)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            if (this is null) return 0;

            int hashId = Id == null ? 0 : Id.GetHashCode();
            int hashName = Name.GetHashCode();

            return hashId ^ hashName;
        }

        public override string ToString()
        {
            return "Id: " + Id + " Name: " + Name;
        }
    }
}
