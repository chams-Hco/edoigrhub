using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CICSWebPortal.ViewModels
{
    public class RevenueViewModel
    {
        public int RevenueId { get; set; }

        [Display(Name = "Client")]
        public int SelectedClientId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ddlClients { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string MDA { get; set; }

        public bool Status { get; set; }
    }
}