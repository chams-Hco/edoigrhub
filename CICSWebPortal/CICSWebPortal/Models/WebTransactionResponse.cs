using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class WebTransactionResponse
    {
        public string ResponseCode { get; set; }      
        public string ResponseDescription { get; set; }
        public string RemittanceCode { get; set; }      
        public string TransactionCode { get; set; }       
        public string TerminalCode { get; set; }
    }
}