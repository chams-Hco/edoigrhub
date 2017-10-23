namespace ChamsICSWebService.Model
{
    public class Ministry
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }
}