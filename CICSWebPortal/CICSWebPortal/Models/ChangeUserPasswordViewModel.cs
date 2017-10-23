namespace CICSWebPortal.Models
{
    public class ChangeUserPasswordViewModel: AuditTrailData
    {
        public string NewPassword { get; internal set; }
        public string OldPassword { get; internal set; }
        public string UserEmail { get; internal set; }
        public int UserId { get; internal set; }
    }
}