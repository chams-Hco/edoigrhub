using CICSWebPortal.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CICSWebPortal.ViewModels
{
    public class WebUserViewModel
    {
         public int SelectedClientId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Clients { get; set; }
        public int SelectedAgentId { get; set; }
        public IEnumerable<System.Web.Mvc.SelectListItem> Agents { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string RePassword { get; set; }
        public string Phone { get; set; }
        [Required]
        public int RoleId { get; set; }
        public int TerminalId { get; set; }
        public int createdby { get; set; }           
        public bool Status { get; set; }
        public string AgentUsername { get; set; }
        public string AgentCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

    }
}