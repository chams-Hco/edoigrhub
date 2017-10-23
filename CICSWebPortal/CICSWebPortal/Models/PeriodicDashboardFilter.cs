using System;

namespace CICSWebPortal.Models
{
    public class PeriodicDashboardFilter
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public int? userId { get; set; }
        public int? roleId { get; set; }

    }
}