using System.Collections.Generic;

namespace ChamsICSWebService.Model
{
    public class AgentSummaryRes:Response
    {
        public IEnumerable<AgentSummary> AgentSummaries { get; set; }
    }

    public class AgentSummary
    {
        public int? AgentId { get; set; }
        public string AgentName { get; set; }
        public int? TotalTerminal { get; set; }
        public int? TotalActiveTerminals { get; set; }
        public int? TotalTransactionCount { get; set; }
        public decimal? TotalTransactionValue { get; set; }
    }

}