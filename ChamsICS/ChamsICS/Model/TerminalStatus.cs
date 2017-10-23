namespace ChamsICSWebService.Model
{
    public class TerminalStatus
    {
        public int TerminalId { get; set; }
        public int status { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }
}