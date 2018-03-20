using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
//using System.ServiceModel;
using System.Web;
//using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace CICSRemittanceService.Models
{
    public class NIBSSEODServiceBaseReq
    {
        //[XmlElement(ElementName = "SourceBankCode")]
        public string SourceBankCode { get; set; }
        //[XmlElement(ElementName = "ChannelCode")]
        public int ChannelCode { get; set; }
        //[XmlElement(ElementName = "CustomerName")]
        public string CustomerName { get; set; }
        //[XmlElement(ElementName = "CustomerAccountNumber")]
        public string CustomerAccountNumber { get; set; }
        //[XmlElement(ElementName = "BillerID")]
        public int BillerID { get; set; }
        //[XmlElement(ElementName = "BillerName")]
        public string BillerName { get; set; }
        //[XmlElement(ElementName = "ProductID")]
        public int ProductID { get; set; }
        //[XmlElement(ElementName = "ProductName")]
        public string ProductName { get; set; }
        //[XmlElement(ElementName = "Amount")]
        public decimal Amount { get; set; }
        //[XmlElement(ElementName = "Param")]
        public List<Param> Params { get; set; } = new List<Param>();
    }
    /// <summary>
    /// Model that holds request data from NIBSS to validate a transaction
    /// </summary>
        public class ValidationRequest : NIBSSEODServiceBaseReq
    {
        public string SourceBankName { get; set; }
        //[XmlElement(ElementName = "InstitutionCode")]
        public int InstitutionCode { get; set; }
        //[XmlElement(ElementName = "Step")]
        public int Step { get; set; }
        //[XmlElement(ElementName = "StepCount")]
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
    public class ValidationResponse : NIBSSEODServiceBaseRes
    {
        public int NextStep { get; set; }
        public PaymentDetail PaymentDetail { get; set; }
    }
    
    public class NotificationRequest : NIBSSEODServiceBaseReq
    {
        public string SessionID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Fee { get; set; }
        public string TransactionFeeBearer { get; set; }
        public string SplitType { get; set; } //from int to string
        public string DestinationBankCode { get; set; }
        public string Narration { get; set; }
        public string PaymentReference { get; set; }
        public long TransactionInitiatedDate { get; set; }
        public long TransactionApprovalDate { get; set; }
        public string SourceBankName { get; set; }
    }

    public class NotificationResponse : NIBSSEODServiceBaseRes
    {
        public string SessionID { get; set; }
    }
    
    public class NIBSSEODServiceBaseRes
    {
        public int BillerID { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public List<Param> Params { get; set; } = new List<Param>();
    }

    public class PaymentDetail
    {
        public decimal Amount { get; set; } 
    }
}