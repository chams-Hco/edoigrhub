using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CICSWebPortal.Models
{
   public class Revenue
    {
        public int RevenueId { get; set; }

        public int ClientId { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string MDA { get; set; }

        public bool Status { get; set; }
    }
}
