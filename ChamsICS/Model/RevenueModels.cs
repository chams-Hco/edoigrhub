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

    public class CreateRevenueRes : ResponseModel
    {
        public int? RevenueId { get; set; }
    }

    public class FindRevenueRes : ResponseModel
    {
        public Revenue Revenue { get; set; }
    }

    public class GetAllRevenueRes : ResponseModel
    {
        public IEnumerable<Revenue> Revenues { get; set; }
    }
}
