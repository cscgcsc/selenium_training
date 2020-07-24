
namespace litecart_tests
{
    public class Price
    {
        public Price(double cost, string currencyCode)
        {
            Cost = cost;
            Currency = new Currency(currencyCode);
        }

        public double Cost { get; set; }
        public double CostIncTax { get; set; }
        public Currency Currency { get; set; }
    }
}
