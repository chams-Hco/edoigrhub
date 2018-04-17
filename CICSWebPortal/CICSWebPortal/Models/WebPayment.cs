using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class WebPayment
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string DatePeriod { get; set; }
        public int UserId { get; set; }
        public int TerminalId { get; set; }
        public decimal FoodAmount { get; set; }
        public decimal DrinkAmount { get; set; }
        public decimal RentalAmount { get; set; }
        public decimal OtherAmount { get; set; }
        public decimal GrossIncome { get; set; }
        public decimal PercentageDeduction { get; set; }
        public decimal Amount { get; set; }
        public int RevenueItemId { get; set; }
        public string Name { get;  set; }
    }
}