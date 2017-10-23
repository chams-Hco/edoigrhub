using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class Agent: AuditTrailData
    {
        public int ClientId { get; set; }
        public int AgentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }

        public bool Status { get; set; }
    }
}