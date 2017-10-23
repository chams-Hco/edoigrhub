using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSWebPortal.Models
{
    public class AgentChartData
    {
      
        public string Agent { get; set; }
        public int? terminals { get; set; }
        public int? transactions { get; set; }
        public decimal? transactionSum { get; set; }
    }

    public class SampleChartData
    {
        public string month { get; set; }
        public double value1 { get; set; }
        public double value2 { get; set; }
        public double value3 { get; set; }

    }
    public class AgentTransactionSumChart
    {

        public string Agent { get; set; }
        public decimal? transactionSum { get; set; }
    }

    public class ClientChartData
    {
        public string Client { get; set; }
        public int? agents { get; set; }
        public int? terminals { get; set; }
        public int? revenue { get; set; }
        public int? transactions { get; set; }
        public decimal? transactionSum { get; set; }
    }
}