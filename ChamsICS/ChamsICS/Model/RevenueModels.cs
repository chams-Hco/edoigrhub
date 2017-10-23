using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService.Model
{
    public class Revenue
    {
        public int RevenueId { get; set; }

        public int ClientId { get; set; }

        public string RevenueCode { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string MDA { get; set; }

        public int Status { get; set; }
    }

    public class ServiceRevenue
    {
        public string RevenueCode { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; }

        public string Ministry { get; set; }

        public string RevenueHead { get; set;}

        public string Category { get; set; }
        public int? Status { get; set; }
    }

    public class CreateRevenueRes : Response
    {
        public int? RevenueId { get; set; }
    }

    public class FindRevenueRes : Response
    {
        public Revenue Revenue { get; set; }
    }

    public class GetAllRevenueRes : Response
    {
        public IEnumerable<Revenue> Revenues { get; set; }
    }

    public class ServiceRevenueReq
    {
        public string AgentCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class ServiceFindRevenueReq
    {
        public string AgentCode { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string RevenueCode { get; set; }
    }

    public class GetServiceRevenueRes : Response
    {
        public IEnumerable<ServiceRevenue> ServiceRevenues { get; set; }
    }
}
