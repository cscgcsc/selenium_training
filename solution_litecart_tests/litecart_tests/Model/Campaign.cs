using System;
using System.Collections.Generic;

namespace litecart_tests
{
    public class Campaign
    {
        public Campaign()
        {
        }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double Percentage { get; set; }
        public List<Price> Prices { get; set; }
    }
}
