using System.Collections.Generic;

namespace ChamsICSWebService.Model
{
    public class ClientSummaryRes : Response
    {
        public List<ClientSummary> ClientSummaries { get; set; }
    }

    public class ClientSummary
    {
        public int? clientId { get; set; }
        public string ClientName { get; set; }
        public int TotalAgents { get; set; }
        public int? TotalRevenueHeads { get; set; }
        public int? TotalTerminal { get; set; }
        public int? TotalActiveTerminals { get; set; }
        public int? TotalTransactionCount { get; set; }
        public decimal? TotalTransactionValue { get; set; }
    }
}