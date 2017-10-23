using System;

namespace ChamsICSWebService.Model
{
    public class ExecutiveDashboardReq
    {
        public int RoleId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}