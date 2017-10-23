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
        public string RoleName { get; set; }
        public string ClientName { get; set; }
        public int ClientId { get; set; }
        public int UserTypeParentId { get; set; }
        public string ClientLogoUrl { get; set; }
    }
}