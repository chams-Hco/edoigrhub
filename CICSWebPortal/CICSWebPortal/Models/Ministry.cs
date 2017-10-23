using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class Ministry
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}