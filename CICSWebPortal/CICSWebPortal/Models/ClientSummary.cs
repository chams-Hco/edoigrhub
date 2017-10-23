namespace CICSWebPortal.Models
{
    public class ClientSummary
    {
        public int? ClientId { get; set; }
        public string ClientName { get; set; }
        public int? TotalAgent { get; set; }
        public int? TotalActiveTerminals { get; set; }
        public int? TotalTerminal { get; set; }
        public int? TotalTransactionCount { get; set; }
        public decimal? TotalTransactionValue { get; set; }
        public int? TotalRevenueHeads { get; internal set; }
    }
}