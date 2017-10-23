namespace CICSWebPortal.Models
{
    public class ResetPasswordModel:AuditTrailData
    {
        public string Email { get; set; }
        public int UserId { get; set; }
    }
}