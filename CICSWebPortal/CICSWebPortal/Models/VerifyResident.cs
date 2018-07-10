using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    
    public class VerifyResident 
    {
        public string ResponseCode { get; set; }
        public string ResponseDescription { get; set; }

        public List<Resident> Resident { get; set; } = new List<Resident>();
        
    }

    public class Resident
    {
        public string ResidentId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string WebAccessPin { get; set; }
    }
}