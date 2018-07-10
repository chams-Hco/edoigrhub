using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class CashWoxModel
    { 
        public string mdacode { get; set; }
        public string accesskey { get; set; }
        public List<CashWoxInvoiceModel> invoices { get; set; }
    }

    public class CashWoxInvoiceModel
    {
        public string invoicelogid { get; set; }
        public int isreversal { get; set; }
        public string invoicedate { get; set; }
        public string invoicevaliduntil { get; set; }
        public string invoiceno { get; set; }
        public string originalamount { get; set; }
        public string customerid { get; set; }
        public string customername { get; set; }
        public string customerfileno { get; set; }
        public List<CashWoxItemModel> items { get; set; }
    }

    public class CashWoxItemModel
    {
        public string itemname { get; set; }
        public string itemcode { get; set; }
        public decimal itemamount { get; set; }
    }

    public class CashWoxIntegrationResponse
    {
        public Dictionary<string, CashWoxIntegrationResponses> responses { get; set; } = new Dictionary<string, CashWoxIntegrationResponses>();
    }

    public class CashWoxIntegrationResponses
    {
        public string invoicelogid { get; set; }
        public int statusid { get; set; }
        public string statusmsg { get; set; }
    }
}