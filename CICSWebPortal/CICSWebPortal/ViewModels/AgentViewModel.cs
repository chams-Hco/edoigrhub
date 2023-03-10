using CICSWebPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CICSWebPortal.ViewModels
{
    public class AgentViewModel
    {
        [Display(Name = "Client")]
        public int SelectedClientId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ddlClients { get; set; }
        public int AgentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }

        public bool Status { get; set; }
    }
}