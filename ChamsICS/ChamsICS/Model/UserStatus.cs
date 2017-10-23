namespace ChamsICSWebService.Model
{
    public class UserStatus
    {
        public int UserId { get; set; }
        public int status { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }
}