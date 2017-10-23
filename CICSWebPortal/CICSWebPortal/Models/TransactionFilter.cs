using System;

namespace CICSWebPortal.Models
{
    public class TransactionFilter
    {
        public int? Limit { get; set; }
        public int UserType { get; set; }
        public int? RoleId {get; set;}
        public int? UserTypeId { get; set;}
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}