using CICSWebPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CICSWebPortal.ViewModels
{
    public class IdentityViewModel
    {

        public int IdentityId { get; set; }

        [Display(Name = "Client")]
        public int SelectedClientId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> ddlClients { get; set; }

        public string URL { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public bool Status { get; set; }
    }
}