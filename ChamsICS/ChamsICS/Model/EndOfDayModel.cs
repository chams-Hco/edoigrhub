using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Xml.Serialization;
namespace ChamsICSWebService.Model
{
    public class EndOfDayModel
    {
        public int Id { get; set; }
        public int TerminalId { get; set; }
        public int? AgentId { get; set; }
        public string TerminalCode { get; set; }
        public string RemittanceCode { get; set; }
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
    [DataContract]
    public class TAMSBaseRequest
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }
    }
    /// <summary>
    /// model that holds data passed by external caller for the creation of new EOD transaction
    /// </summary>
    [DataContract]
    public class CreateEndOfDayReq : TAMSBaseRequest
    {
        [DataMember]
        public string TerminalCode { get; set; }
        [DataMember]
        public decimal Amount { get; set; }
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
        public string TerminalCode { get; set; }
        [DataMember]
        public string TransactionRef { get; set; }
    }

    /// <summary>
    /// Model that holds data passed by external caller to check payment status of an EOD transaction
    /// </summary>
    [DataContract]
    public class QueryEndOfDayStatusReq : TAMSBaseRequest
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

    [DataContract]
    public class AuthenticatableReq
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string password { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    //[DataContract]
    [Serializable]
    public class NIBSSEODServiceBaseReq
    {
        //[DataMember]
        [XmlElement(ElementName = "SourceBankCode")]
        public string SourceBankCode { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "ChannelCode")]
        public int ChannelCode { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "CustomerName")]
        public string CustomerName { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "CustomerAccountNumber")]
        public string CustomerAccountNumber { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "BillerID")]
        public int BillerID { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "BillerName")]
        public string BillerName { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "ProductID")]
        public int ProductID { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "ProductName")]
        public string ProductName { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "Amount")]
        public decimal Amount { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "Param")]
        public List<Param> Param { get; set; } = new List<Param>();
        //[DataMember]
        //public Param Param { get; set; }
    }
    /// <summary>
    /// Model that holds request data from NIBSS to validate a transaction
    /// </summary>
    //[DataContract]
    [DataContract]
    public class ValidationRequest
    {
        [DataMember]
        [XmlElement(ElementName = "SourceBankCode")]
        public string SourceBankCode { get; set; }
        [DataMember]
        [XmlElement(ElementName = "ChannelCode")]
        public int ChannelCode { get; set; }
        [DataMember]
        [XmlElement(ElementName = "CustomerName")]
        public string CustomerName { get; set; }
        [DataMember]
        [XmlElement(ElementName = "CustomerAccountNumber")]
        public string CustomerAccountNumber { get; set; }
        [DataMember]
        [XmlElement(ElementName = "BillerID")]
        public int BillerID { get; set; }
        [DataMember]
        [XmlElement(ElementName = "BillerName")]
        public string BillerName { get; set; }
        [DataMember]
        [XmlElement(ElementName = "ProductID")]
        public int ProductID { get; set; }
        [DataMember]
        [XmlElement(ElementName = "ProductName")]
        public string ProductName { get; set; }
        [DataMember]
        [XmlElement(ElementName = "Amount")]
        public decimal Amount { get; set; }
        [DataMember]
        [XmlElement("Param")]
        //[XmlArray(ElementName = "Param", IsNullable = false, Order = 1)]
        public List<Param> Param { get; set; } = new List<Param>();



        //[DataMember]
        [XmlElement(ElementName = "SourceBankName")]
        public string SourceBankName { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "InstitutionCode")]
        public int InstitutionCode { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "Step")]
        public int Step { get; set; }
        //[DataMember]
        [XmlElement(ElementName = "StepCount")]
        public string StepCount { get; set; }
        //[DataMember]
        //public Param Param { get; set; }
    }

    public class Param
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    /// <summary>
    /// Model that holds response data sent to NIBSS for transaction validation
    /// </summary>
    //[Serializable]
    //[XmlRoot(IsNullable = false)]
    [DataContract]
    public class ValidationResponse : NIBSSEODServiceBaseRes
    {
        [DataMember]
        public int NextStep { get; set; }
    }

    [DataContract]
    public class PeriodicTransactionRequest
    {
        [DataMember]
        public int TerminalId { get; set; }
        [DataMember]
        public int AgentId { get; set; }
        [DataMember]
        public DateTime FromDate { get; set; }
        [DataMember]
        public DateTime ToDate { get; set; }
    }

    [DataContract]
    public class NotificationRequest : NIBSSEODServiceBaseReq
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
        public string SplitType { get; set; }
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
        //[DataMember]
        //public List<Param> Params { get; set; } = new List<Param>();
    }

    public class NotificationResponse : NIBSSEODServiceBaseRes
    {
        [DataMember]
        public string SessionID { get; set; }
    }

    [DataContract]
    public class NIBSSEODServiceBaseRes
    {
        [DataMember]
        public int BillerID { get; set; }
        [DataMember]
        public string ResponseCode { get; set; }
        [DataMember]
        public string ResponseMessage { get; set; }
        [DataMember]
        public List<Param> Param { get; set; } = new List<Param>();
    }

    [DataContract]
    public class CustomStreamContent : System.Net.Http.StreamContent
    {
        public CustomStreamContent(Stream content) : base(content)
        {
        }
    }

    public class EodRequest
    {
        public string DATETRANSACTION { get; set; }
        public decimal TOTAL { get; set; }
        public int EODCOUNT { get; set; }
        public string EODSTATUS { get; set; }
        public string TERMINALCODE { get; set; }
        public string AGENTEMAIL { get; set; }
        public string AGENTFULLNAME { get; set; }
    }
}