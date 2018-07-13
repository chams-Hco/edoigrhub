using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int ClientId { get; set; }
        public int AgentId { get; set; }
        public string AgentName { get; set; }
        public string LocationName { get; set; }
        public int TerminalId { get; set; }
        public string TerminalCode { get; set; }
        public string TransactionCode { get; set; }
        public string ResidentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueName { get; set; }
        public string Ministry { get; set; }
        public string RevenueHead { get; set; }
        public decimal Amount { get; set; }
        public string PaymentReference { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime UploadDate { get; set; }
        public int Status { get; set; }
        public int? LocationId { get; internal set; }
        public decimal? Income { get; set; }
        public decimal? Percentage { get; set; }
        public decimal? FoodAmount { get; set; }
        public decimal? DrinkAmount { get; set; }
        public decimal? RentalAmount { get; set; }
        public decimal? OtherAmount { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Name { get; set; }
        public string BatchCode { get; set; } //Maps to transaction ref in EOD
    }
}