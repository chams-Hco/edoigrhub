using System;

namespace CICSWebPortal.Models
{
    public class Report
    {
        public int TransactionId { get; set; }
        public string Agent { get; set; }
        public string Terminal { get; set; }
        public string RevenueName { get; set; }
        public string Ministry { get; set; }
        public string RevenueHead { get; set; }
        public string ResidentId { get; set; }
        public string TransactionCode { get; set; }
        public string RemittanceCode { get; set; }
        public string RevenueCode { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public decimal? FoodAmount { get; set; }
        public decimal? DrinkAmount { get; set; }
        public decimal? RentalAmount { get; set; }
        public decimal? OtherAmount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}