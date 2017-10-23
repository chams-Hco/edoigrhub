using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class ReportViewModel
    {
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Client")]
        public int? SelectedClientId { get; set; }

        [Display(Name = "Agent")]
        public int? SelectedAgentId { get; set; }

        [Display(Name = "Terminal")]
        public int? SelectedTerminalId { get; set; }

        [Display(Name = "Ministry")]
        public string SelectedMinistry { get; set; }

        [Display(Name = "Revenue Code")]
        public string SelectedRevenueCode { get; set; }

        public decimal TotalTransactionValue { get; set; }

        public IEnumerable<System.Web.Mvc.SelectListItem> clientList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> agentList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> terminalList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ministryList { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> revenueList { get; set; }

        public IEnumerable<Models.Report> Report { get; set; }
    }
}