namespace CICSWebPortal.Models
{
    public class ChangeUserPasswordModel: AuditTrailData
    {
        public string UserEmail { get; set; }
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}