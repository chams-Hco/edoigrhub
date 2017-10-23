using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    public class Payment : Response
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
    }

    public class PaymentReq : Payment
    {
        public int InvoiceId { get; set; }
    }

    public class GetInvoicePaymentRes : Response
    {
        public IEnumerable<Payment> Payments { get; set; }
    }

    public class MakePaymentRes : Response
    {
        public string Code { get; set; }
        public string ClientId { get; set; }
    }
}
