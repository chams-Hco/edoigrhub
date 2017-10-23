using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class Terminal
    {
        internal int? clientId { get; set; }
        internal int? userId { get; set; }

        public string AgentName { get; set; }
        public int AgentId { get; set; }
        public int TerminalId { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }

        public string SerialNumber { get; set; }
        public bool status { get; set; }


    }
}