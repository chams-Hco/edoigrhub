using System;
using System.Collections.Generic;

namespace ChamsICSWebService.Model
{
    public class AuditTrail
    {
        public string AuditLog { get; set; }
        public string Client { get; set; }
        public DateTime? LogDate { get; set; }
        public string TableAffected { get; set; }
        public string Type { get; set; }
        public string User { get; set; }
    }
    public class GetAuditTrailsRes : Response
    {
        public List<AuditTrail> AuditTrails { get; set; }
    }

}