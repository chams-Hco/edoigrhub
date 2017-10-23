using System;
using System.Collections.Generic;

namespace CICSWebPortal.Models
{
    public class Biodata
    {
        public int BiodataId { get; set; }
        public int UserId { get; set; }
        public string Surname { get; set; }
        public string Firstname { get; set; }
        public string Email { get; set; }
        public int RIN { get; set; }
        public DateTime EntryDate { get; set; }
    }
}