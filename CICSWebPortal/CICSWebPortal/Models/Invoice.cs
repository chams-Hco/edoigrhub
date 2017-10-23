using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class Invoice
    {
        public int InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public string Description { get; set; }
        public string EntryDate { get; set; }
        public string Email { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountPaid { get; set; }
        public string RevenueCode { get; set; }
        public bool Status { get; set; }
        //public List<InvoiceItem> InvoiceItem { get; set;  }
    }

    public class InvoiceItem
    {
        public int InvoiceItemId { get; set; }
        public string Title { get; set; }
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string EntryDate { get; set; }
    }
}