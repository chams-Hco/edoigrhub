using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;



namespace CICSWebPortal.Models
{
    public class EndOfDay
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int? EODCount { get; set; }
        public String EODStatus { get; set; }
        public int TerminalId { get; set; }
        public string TerminalCode { get; set; }
        public int? AgentId { get; set; }
        public string AgentEmail { get; set; }
        public string AgentName { get; set; }
        public string AgentPhone { get; set; }
        public string RemittanceCode { get; set; }
    }
}

