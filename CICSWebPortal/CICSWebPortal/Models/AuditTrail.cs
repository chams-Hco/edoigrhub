using System;

namespace CICSWebPortal.Models
{
    public class AuditTrail
    {
        public string AuditLog { get; set; }
        public string Client { get; set; }
        public string LogDate { get; set; }
        public string TableAffected { get; set; }
        public string Type { get; set; }
        public string User { get; set; }
    }
}