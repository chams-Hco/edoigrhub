using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class EodFilter
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
        public int? clientId { get; set; }
        public int? agentId { get; set; }
        public int? terminalId { get; set; }
        public string ministry { get; set; }
        public string RevenueCode { get; set; }
        //properties added for EOD report
        public bool? Status { get; set; }
        public string Terminal { get; set; }  //terminalID
        public List<string> TerminalIds { get; set; } = new List<string>();
    }
}