using System;

namespace CICSWebPortal.Models
{
    public class Payment
    {
        public string Reference { get; set; }
        public string Target { get; set; }
        public int? WalletId { get; set; }
        public int? TransactionId { get; set; }
        public decimal Amount { get; set; }
        public bool Status { get; set; }
        public string Method { get; set; }
        public int UserId { get; set; }
        public string Email { get; set; }
        public DateTime EntryDate { get; set; }

        public string TransactionInfo { get; set; }
    }

    public class PaymentReq : Payment
    {
        public int InvoiceId { get; set; }
    }
}