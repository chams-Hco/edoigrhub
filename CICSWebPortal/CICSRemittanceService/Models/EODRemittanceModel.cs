using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Web;
using System.Web.Services.Protocols;
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
        public List<Param> Param { get; set; } = new List<Param>();
    }
    /// <summary>
    /// Model that holds request data from NIBSS to validate a transaction
    /// </summary>

   // [XmlRoot(ElementName ="ValidationRequest")]
   // [MessageContract(IsWrapped = false)]
        [DataContract]
        public class ValidationRequest
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
        //[XmlArray(ElementName = "Param", IsNullable = false, Order = 1)]
        public List<Param> Param { get; set; } = new List<Param>();
        
        //[XmlElement(ElementName = "SourceBankName")]
        public string SourceBankName { get; set; }
        //[XmlElement(ElementName = "InstitutionCode")]
        public int InstitutionCode { get; set; }
        //[XmlElement(ElementName = "Step")]
        public int Step { get; set; }
        //[XmlElement(ElementName = "StepCount")]
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
    public class ValidationResponse : NIBSSEODServiceBaseRes
    {
        public int NextStep { get; set; }
    }
    
    public class NotificationRequest : NIBSSEODServiceBaseReq
    {
        public string SessionID { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Fee { get; set; }
        public string TransactionFeeBearer { get; set; }
        public int SplitType { get; set; }
        public string DestinationBankCode { get; set; }
        public string Narration { get; set; }
        public string PaymentReference { get; set; }
        public long TransactionInitiatedDate { get; set; }
        public long TransactionApprovalDate { get; set; }
        //[DataMember]
        //public List<Param> Params { get; set; } = new List<Param>();
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
        public List<Param> Param { get; set; } = new List<Param>();
    }

    public class NIBSSSoapExtension : SoapExtension
    {
        Stream oldStream;
        Stream newStream;

        // Save the Stream representing the SOAP request or SOAP response into
        // a local memory buffer.
        public override Stream ChainStream(Stream stream)
        {
            oldStream = stream;
            newStream = new MemoryStream();
            return newStream;
        }

        public override object GetInitializer(LogicalMethodInfo methodInfo, SoapExtensionAttribute attribute)
        {
            return ((NIBSSSoapExtensionAttribute)attribute).ExtensionType;
        }

        public override object GetInitializer(Type serviceType)
        {
            return 1; //can return anything
        }

        public override void Initialize(object initializer)
        {
            
        }

        public override void ProcessMessage(SoapMessage message)
        {
            switch (message.Stage)
            {
                case SoapMessageStage.BeforeSerialize:
                    var stream  = message.Stream;
                    break;
                case SoapMessageStage.AfterSerialize:
                    break;
                case SoapMessageStage.BeforeDeserialize:
                    //put your methods to modify the stream. in this case to wrap soap:envelope around validaterequest object
                    break;
                case SoapMessageStage.AfterDeserialize:
                    break;
                default:
                    throw new Exception("invalid stage");
            }
        }
    }

    // Create a SoapExtensionAttribute for the SOAP Extension that can be
    // applied to a Web service method.
    [AttributeUsage(AttributeTargets.Method)]
    public class NIBSSSoapExtensionAttribute : SoapExtensionAttribute
    {
        private int priority;

        public override Type ExtensionType
        {
            get { return typeof(NIBSSSoapExtension); }
        }

        public override int Priority
        {
            get { return priority; }
            set { priority = value; }
        }
    }
}