using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService.Model
{
    public class AssessmentModelRes : Response
    {
        public IEnumerable<RevenueItem> RevenueItems { get; set; }
    }

    public class AssessmentReq
    {
        public int roleid { get; set; }
        public string email { get; set; }
    }
}