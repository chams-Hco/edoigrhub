using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class EndofDayViewModel
    {
        public int userID { get; set; }
        public int roleId { get; set; }
        public List<Terminal> Terminals { get; set; }
    }
}