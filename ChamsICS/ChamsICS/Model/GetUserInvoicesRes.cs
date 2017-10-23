using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService.Model
{
    public class GetUserInvoicesRes : Response
    {
        public IEnumerable<Invoice> UserInvoices { get; set; }
    }

    public class Invoice : Response
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string Description { get; set; }
        public DateTime EntryDate { get; set; }
        public string Email { get; set; }
        public string RevenueCode { get; set; }
        public decimal Amount { get; set; }
        public List<InvoiceItem> InvoiceItem { get; set; }
    }

    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        public string Title { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string EntryDate { get; set; }
    }
}