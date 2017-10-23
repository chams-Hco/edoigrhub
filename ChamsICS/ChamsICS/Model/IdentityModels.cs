using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService.Model
{
    public class Identity
    {
        public int IdentityId { get; set; }
        public int ClientId { get; set; }
        public string URL { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Status { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }

    public class CreateIdentityRes : Response
    {
        public int? IdentityId { get; set; }
    }

    public class FindIdentityRes : Response
        {
            public Identity Identity { get; set; }
        }

    public class GetAllIdentityRes : Response
        {
            public IEnumerable<Identity> Identities { get; set; }
        }
}