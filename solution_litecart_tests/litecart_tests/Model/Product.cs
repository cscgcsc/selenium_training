using System;
using System.Collections.Generic;

namespace litecart_tests
{
    public class Product : IComparable, IEquatable<Product>
    {
        public Product()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool Enable { get; set; }       
        public List<string> CategoriesId { get; set; }
        public string DefaultCategoryId { get; set; }
        public List<string> ProductGroupsId { get; set; }
        public double Quantity { get; set; }
        public string QuantityUnitId { get; set; }
        public string DeliveryStatusId { get; set; }
        public string SoldOutStatusId { get; set; }
        public string ImagePath { get; set; }
        public DateTime DateValidFrom { get; set; }
        public DateTime DateValidTo { get; set; }
        public string ManufacturerId { get; set; }
        public string SupplierId { get; set; }
        public string Keywords { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string HeadTitle { get; set; }
        public string MetaDescription { get; set; }
        public double PurchasePrice { get; set; }
        public double SalePrice { get; set; }
        public string PurchasePriceCurrencyCode { get; set; }
        public string TaxClassId { get; set; }
        public List<Price> Prices { get; set; }
        public List<Campaign> Campaigns { get; set; }
        public string PurchasePriceText { get; set; }
        public string SalePriceText { get; set; }
        public string PurchasePriceColor { get; set; }
        public string PurchasePriceFont { get; set; }
        public string SalePriceColor { get; set; }
        public string SalePriceFont { get; set; }
        public List<string> Stickers { get; set; }
        public string Url { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null)
                return 1;

            if (obj is Product other)
                return Name.CompareTo(other.Name);
            else
                throw new ArgumentException("Object is not a Product");
        }

        public bool Equals(Product other)
        {
            if (other == null)
                return false;

            //if (Object.ReferenceEquals(this, other) || Id == other.Id && Name == other.Name)
            //    return true;
            if (ReferenceEquals(this, other) || Name == other.Name)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            if (this is null) return 0;
            //int hashId = Id == null ? 0 : Id.GetHashCode();
            //int hashName = Name.GetHashCode();
            //return hashId ^ hashName;

            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return "Id: " + Id + " Name: " + Name;
        }
    }
}
