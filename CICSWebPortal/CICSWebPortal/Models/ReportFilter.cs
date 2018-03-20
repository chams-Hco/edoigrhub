using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class ReportFilter
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int? clientId { get; set; }
        public int? agentId { get; set; }
        public int? terminalId { get; set; }
        public string ministry { get; set; }
        public string RevenueCode { get; set; }
    }
}