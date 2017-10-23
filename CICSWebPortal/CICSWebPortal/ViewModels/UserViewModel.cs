using CICSWebPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CICSWebPortal.ViewModels
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Display(Name = "Role")]
        public int SelectedRoleId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ddlRoles { get; set; }

        [Display(Name = "Client")]
        public int SelectedClientId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ddlClients { get; set; }


        [Display(Name = "Agent")]
        public int SelectedAgentId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ddlAgents { get; set; }

        public string Email { get; set; }

        public string Mobile { get; set; }

        public string Password { get; set; }

        public bool Status { get; set; }

    }
}