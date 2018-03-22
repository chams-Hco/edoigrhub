using CICSWebPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CICSWebPortal.ViewModels
{
    public class EndofDayViewModel
    {
        //public int userID { get; set; }
        //public int roleId { get; set; }
        //public List<Terminal> Terminals { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Agent")]
        public int? SelectedAgentId { get; set; }

        [Display(Name = "Terminal")]
        public string SelectedTerminalId { get; set; }

        [Display(Name = "Status")]
        public bool SelectedStatus { get; set; }

        public decimal TotalTransactionValue { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> agentList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> terminalList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> statusList { get; set; }



        //[Display(Name = "Revenue Code")]
        //public string SelectedRevenueCode { get; set; }

        //[Display(Name = "Ministry")]
        //public string SelectedMinistry { get; set; }

        //[Display(Name = "Revenue Code")]
        //public string SelectedRevenueCode { get; set; }

        //public IEnumerable<System.Web.Mvc.SelectListItem> clientList { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> agentList { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> terminalList { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> ministryList { get; set; }
        //public IEnumerable<System.Web.Mvc.SelectListItem> revenueList { get; set; }

        public IEnumerable<EndOfDay> EODReport { get; set; }
    }
}