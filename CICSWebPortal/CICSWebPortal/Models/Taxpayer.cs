using System;
using System.Collections.Generic;

namespace CICSWebPortal.Models
{
    public class Taxpayer : Biodata
    {
        public int id { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public string Mobile { get; set; }
    }

    public class GenerateInvoice
    {
        public string RevenueCode { get; set; }
        public int TaxpayerId { get; set; }
    }
}