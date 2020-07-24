using System;

namespace litecart_tests
{
    public class Zone : IComparable, IEquatable<Zone>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string CountryName { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Zone other)
                return Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Object is not a Zone");
        }

        public bool Equals(Zone other)
        {
            if (other == null)
                return false;

            return ReferenceEquals(this, other) || Id == other.Id && Name == other.Name;
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
