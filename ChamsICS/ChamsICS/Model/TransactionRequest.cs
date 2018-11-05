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

    public class GetAllTransactionRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public String StartDate { get; set; }
        public String EndDate { get; set; }
    }
}