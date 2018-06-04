using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService.Model
{
    public class CashWorkIntegrationRes
    {
        public Dictionary<string, CashWorkIntegrationResponses> responses { get; set; }
    }

    public class CashWorkIntegrationResponses
    {
        public string invoicelogid { get; set; }
        public int statusid { get; set; }
        public string statusmsg { get; set; }
    }
}