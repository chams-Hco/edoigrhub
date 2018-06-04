using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSResidencyAPI.Models
{
    public class Member
    {
        public string MembershipNo { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Sex { get; set; }

    }
}