using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.ViewModels
{
    public class UserDashBoardViewModel
    {
        public int UserId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public bool CanCreateWebUsers { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public int UserTypeParentId { get; set; }
        public string ClientLogoUrl { get; set; }
        public int? TerminalId { get; set; }
        public int? ZoneID { get; set; }
        public string TerminalCode { get; set; }
        public string ZoneName { get; set; }
        public string ZoneCode { get; set; }
        public string Address { get; set; }
        public string Name { get;  set; }
    }
}