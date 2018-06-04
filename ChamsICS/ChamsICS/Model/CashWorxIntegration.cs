using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService.Model
{
    public class CashWorxIntegration
    {

        public string mdacode { get; set; }
        public string accesskey { get; set; }
        [JsonProperty("invoices")]
        public List<CashWorkInvoice> invoices { get; set; }
    }

    public class CashWorkInvoice
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
        [JsonProperty("items")]
        public List<CashWorkItem> items { get; set; }
    }

    public class CashWorkItem
    {
        public string itemname { get; set; }
        public string itemcode { get; set; }
        public decimal itemamount { get; set; }
    }

}