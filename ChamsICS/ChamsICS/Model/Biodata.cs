using ChamsICSWebService.Model;
using System;
using System.Collections.Generic;

namespace ChamsICSWebService.Model
{
    public class Biodata : Response
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