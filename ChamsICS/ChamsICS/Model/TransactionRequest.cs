using ChamsICSLib.Global;
using System;

namespace ChamsICSWebService.Model
{
    public class GetTransactionRequest
    {
        public int UserType { get; set; }
        public int UserTypeId { get; set; }
        public bool RequireLimit { get; set; }
        public int?  Limit { get; set; }
        public bool RequireDateFilter { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndtDate { get; set; }
    }
}