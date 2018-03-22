using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Models
{
    public class EndofDayViewModel
    {
        internal List<SelectListItem> terminalList;

        public int userID { get; set; }
        public int roleId { get; set; }
        public List<Terminal> Terminals { get; set; }
        
    }
}