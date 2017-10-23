using System;
using System.Collections.Generic;

namespace ChamsICSWebService.Model
{
    public class TransactionSummary
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int? clientId { get; set; }
        public int? AgentId { get; set; }
        public int? terminalId { get; set; }
        public string Ministry { get; set; }
        public string RevenueCode { get; set; }
    }

    public class TransactionSummaryRes: Response
    {
        public IEnumerable<Model.Report> Report { get; set; }
    }
}