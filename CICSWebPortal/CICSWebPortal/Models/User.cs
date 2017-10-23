using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class User:AuditTrailData
    {
        public int UserId { get; set; }
        public int UserTypeParentId { get; set; }
        public int ClientId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public int PasswordStatus { get; set; }
        public bool Status { get; set; }
    }
}