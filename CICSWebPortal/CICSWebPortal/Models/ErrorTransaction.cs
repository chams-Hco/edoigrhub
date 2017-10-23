using System;

namespace CICSWebPortal.Models
{
    public class ErrorTransaction
    {
        public string Address { get; set; }
        public int? ClientId { get; set; }
        public int? AgentId { get; set; }
        public int? TerminalId { get; set; }
        public string AgentName { get; set; }
        public Decimal? Amount { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public int? TransactionId { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Ministry { get; set; }
        public string PaymentReference { get; set; }
        public string PhoneNumber { get; set; }
        public string ResidentId { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueHead { get; set; }
        public string RevenueName { get; set; }
        public int? Status { get; set; }
        public string TerminalCode { get; set; }
        public string TransactionCode { get; set; }
        public DateTime? TransactionDate { get; set; }
        public DateTime? UploadDate { get; set; }
        public string UploadError { get; set; }
        public string ResolutionError { get; set; }
        public int? ResolutionStatus { get; set; }
        public DateTime? ResolutionDate { get; set; }
    }
}