using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class EndOfDayServiceResponse
    {
        public DateTime DATETRANSACTION { get; set; }
        public decimal TOTAL { get; set; }
        public int EODCOUNT { get; set; }
        public String EODSTATuS { get; set; }
        public string TERMINALCODE { get; set; }
        public string AGENTEMAIL { get; set; }
        public string AGENTFULLNAME { get; set; }

    }

   
}