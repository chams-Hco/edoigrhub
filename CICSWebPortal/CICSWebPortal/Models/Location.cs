namespace CICSWebPortal.Models
{
    public class Location : AuditTrailData
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int AgentId { get; set; }
        public string LocationCode { get; set; }
        public string LocationDescription { get; set; }
    }
}