using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class Client:AuditTrailData
    {
        public int ClientId { get; set; } //wont display on the form
        public string ClientCode { get; set; }
        public string ClientName { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNo1 { get; set; }
        public string PhoneNo2 { get; set; }
        public bool HasWebUsers { get; set; }
        public bool Status { get; set; }
        public IEnumerable<Agent> Agents { get; set; }
        public ClientSetting ClientSetting { get; set; }
    }

    public class ClientSetting
    {
        public decimal? PercentageDeduction { get; set; }
        public int? DefaultRevenueItemId { get; set; }
        public bool? ShowAmountUnpaid { get; set; } = true;
        public int? ConsumptionTaxRevenueId { get; set; }
        public int? WithholdingTaxRevenueItem { get; set; }
        public int? PayeeRevenueItem { get; set; }

    }
}