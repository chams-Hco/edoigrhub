using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService.Model
{
    public class RevenueItem
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int MinistryId { get; set; }
        public int CategoryId { get; set; }
        public int RevenueHeadId { get; set; }
        public string Ministry { get; set; }
        public string RevenueHead { get; set; }
        public string Category { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal? Amount { get; set; }
        public int Status { get; set; }

        public AuditTrailData AuditTrailData { get; set; }
    }
}