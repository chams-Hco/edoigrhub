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
        public string RoleCode { get; set; }
        public string Password { get; set; }
        public int PasswordStatus { get; set; }
        public int? TerminalID { get; set; }
        public string TerminalCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Status { get; set; }
    }
}