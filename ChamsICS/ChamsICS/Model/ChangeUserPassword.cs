namespace ChamsICSWebService.Model
{
    public class ChangeUserPassword
    {
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }
}