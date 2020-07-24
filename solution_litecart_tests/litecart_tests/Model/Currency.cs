
namespace litecart_tests
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code;
        }

        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public double Value { get; set; }
        public string Status { get; set; }
    }
}
