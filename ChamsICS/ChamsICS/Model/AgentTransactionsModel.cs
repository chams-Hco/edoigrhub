using System;

namespace ChamsICSWebService.Model
{
    internal class AgentTransactionsModel
    {
        public int? AgentId { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? Date { get; set; }
        public string RevenueCode { get; set; }
        public string RevenueHeadName { get; set; }
        public string RevenueItemName { get; set; }
        public int? TerminalId { get; set; }
    }
}