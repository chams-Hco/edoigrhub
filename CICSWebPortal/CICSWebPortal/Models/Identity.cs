using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class Identity: AuditTrailData
    {    
        public int IdentityId { get; set; }

        public int ClientId { get; set; }

        public string URL { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Status { get; set; }

    }
}