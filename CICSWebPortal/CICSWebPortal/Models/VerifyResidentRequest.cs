using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class VerifyResidentRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string AgentCode { get; set; }
        public string TerminalCode { get; set; }
        public string ResidentId { get; set; }
    }
}