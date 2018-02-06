using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChamsICSWebService.Model
{
    public class EndOfDayModel
    {
        public int Id { get; set; }
        public int TerminalId { get; set; }
        public int? AgentId { get; set; }
        public string TerminalCode { get; set; }
        public string HandlerName { get; set; }
        public string HandlerEmail { get; set; }
        public string HandlerPhone { get; set; }
        public decimal Amount { get; set; }
        public int? EODCount { get; set; }
        public bool Status { get; set; }
        public System.DateTime Date { get; set; }
    }

    public class FetchEndOfDayRes : Response
    {
        public IEnumerable<EndOfDayModel> EndOfDayReport { get; set; }
    }

    public class FetchEndOfDayReq
    {
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        //public int? clientId { get; set; }
        public int? AgentId { get; set; }
        public String TerminalId { get; set; }
        //public int? terminalId { get; set; }
        //public string Ministry { get; set; }
        public List<string> TerminalIds { get; set; } //timi added
        public bool? status { get; set; }
    }
}