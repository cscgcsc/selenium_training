using System;

namespace litecart_tests
{
    public class Customer : IComparable, IEquatable<Customer>
    {
        public Customer()
        {          
        }

        public string Id { get; set; }       
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string TaxId { get; set; }
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Postcode { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string ZoneCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public bool Newsletter { get; set; }
        public string Password { get; set; }
        public string ConfirmedPassword { get; set; }


        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Customer other)
            {
                int result = Firstname.CompareTo(other.Firstname);
                if (result != 0)
                    return result;
                return Lastname.CompareTo(other.Lastname);
            }
            else
                throw new ArgumentException("Object is not a Customer");
        }

        public bool Equals(Customer other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other) || Firstname == other.Firstname && Lastname == other.Lastname)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            if (this is null) return 0;

            //int hashId = Id == null ? 0 : Id.GetHashCode();           
            int hashFirstname = Firstname.GetHashCode();
            int hashLastname = Lastname.GetHashCode();

            return hashLastname ^ hashFirstname;
        }

        public override string ToString()
        {
            return string.Format("Id: {0} FirstName: {1} LastName: {2}", Id, Firstname, Lastname); 
        }
    }
}
