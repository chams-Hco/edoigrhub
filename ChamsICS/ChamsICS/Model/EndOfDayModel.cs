using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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

    /// <summary>
    /// model that holds EOD data returned to portal
    /// </summary>
    public class FetchEndOfDayRes : Response
    {
        public IEnumerable<EndOfDayModel> EndOfDayReport { get; set; }
    }

    /// <summary>
    /// model that holds EOD request from portal
    /// </summary>
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

    /// <summary>
    /// model that holds data passed by external caller for the creation of new EOD transaction
    /// </summary>
    [DataContract]
    public class CreateEndOfDayReq
    {
        [DataMember]
        public int TerminalId { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        //[DataMember]
        //public bool Status { get; set; }
        [DataMember]
        public int EODCount { get; set; }
    }
    /// <summary>
    /// model that holds the data to be returned to external caller after creating new EOD transaction
    /// </summary>
    [DataContract]
    public class CreateEndOfDayRes : Response
    {
        [DataMember]
        public int TerminalId { get; set; }
        [DataMember]
        public string TransactionRef { get; set; }
    }
    /// <summary>
    /// Model that holds data passed by external caller to validate if an EOD transaction exists
    /// </summary>
    [DataContract]
    public class ValidateEndOfDayReq
    {
        [DataMember]
        public string TransactionRef { get; set; }
        public string SourceBankCode { get; set; }
        public int MyProperty { get; set; }
    }
    /// <summary>
    /// model that holds the data to be returned to external caller after validate an EOD transactioni
    /// </summary>
    [DataContract]
    public class ValidateEndOfDayRes : Response
    {
        [DataMember]
        public EndOfDayModel EODTransaction { get; set; }
    }

    /// <summary>
    /// Model that holds data passed by external caller to check payment status of an EOD transaction
    /// </summary>
    [DataContract]
    public class QueryEndOfDayStatusReq
    {
        [DataMember]
        public string TransactionRef { get; set; }
    }

    [DataContract]
    public class QueryEndOfDayStatusRes : Response
    {
        [DataMember]
        public string Status { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class EODServiceBaseReq
    {
        [DataMember]
        public string SourceBankCode { get; set; }
        [DataMember]
        public int ChannelCode { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public string CustomerAccountNumber { get; set; }
        [DataMember]
        public int BillerID { get; set; }
        [DataMember]
        public string BillerName { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductName { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
        [DataMember]
        public List<Param> Param { get; set; } = new List<Param>();
    }
    /// <summary>
    /// Model that holds request data from NIBSS to validate a transaction
    /// </summary>
    [DataContract]
    public class ValidationRequest : EODServiceBaseReq
    {
        [DataMember]
        public string SourceBankName { get; set; }
        [DataMember]
        public int InstitutionCode { get; set; }
        [DataMember]
        public int Step { get; set; }
        [DataMember]
        public string StepCount { get; set; }
    }

    public class Param
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    /// <summary>
    /// Model that holds response data sent to NIBSS for transaction validation
    /// </summary>
    [DataContract]
    public class ValidationResponse
    {
        [DataMember]
        public int BillerID { get; set; }
        [DataMember]
        public int NextStep { get; set; }
        [DataMember]
        public string ResponseCode { get; set; }
        [DataMember]
        public List<Param> Param { get; set; } = new List<Param>();
    }

    [DataContract]
    public class NotificationRequest : EODServiceBaseReq
    {
        [DataMember]
        public string SessionID { get; set; }
        [DataMember]
        public decimal TotalAmount { get; set; }
        [DataMember]
        public decimal Fee { get; set; }
        [DataMember]
        public string TransactionFeeBearer { get; set; }
        [DataMember]
        public int SplitType { get; set; }
        [DataMember]
        public string DestinationBankCode { get; set; }
        [DataMember]
        public string Narration { get; set; }
        [DataMember]
        public string PaymentReference { get; set; }
        [DataMember]
        public long TransactionInitiatedDate { get; set; }
        [DataMember]
        public long TransactionApprovalDate { get; set; }
    }

    [DataContract]
    public class NotificationResponse
    {
        [DataMember]
        public string SessionID { get; set; }
        [DataMember]
        public int BillerID { get; set; }
        [DataMember]
        public string ResponseCode { get; set; }
        [DataMember]
        public string ResponseMessage { get; set; }
        [DataMember]
        public List<Param> Param { get; set; } = new List<Param>();
    }
}