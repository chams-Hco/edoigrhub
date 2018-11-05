using ChamsICSWebService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract(Namespace = "http://3rdpartyservices.cics.chams.com")]
    [Description("Chams IGR Consolidation System - CICS. For Technical support, contact itsupport@chams.com ")]
    public interface iChamsICSService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/AuthoriseTerminal", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to retreive TerminalCode when setting up a terminal for the first time. For Technical support, contact itsupport@chams.com ")]
        AuthoriseTerminalRes AuthoriseTerminal(AuthoriseTerminalReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/AuthoriseWebUser", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to retreive TerminalCode when setting up a terminal for the first time. For Technical support, contact itsupport@chams.com ")]
        AuthoriseTerminalRes AuthoriseWebUser(AuthoriseWebUserReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/VerifyResidentId", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to verify the identity of a resident. For Technical support, contact itsupport@chams.com ")]
        VerifyResidentIdRes VerifyResidentId(VerifyResidentIdReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/VerifyAnambraResidentID", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to verify the identity of an anambra resident. For Technical support, contact itsupport@chams.com ")]
        Task<VerifyResidentRes> VerifyAnambraResidentID(VerifyResidentIdReq req);


        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/UploadTransaction", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to upload transactions. For Technical support, contact itsupport@chams.com ")]
        UploadTransactionRes UploadTransaction(UploadTransactionReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/QueryUploadTransaction", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to check status upload transactions. For Technical support, contact itsupport@chams.com ")]
        QueryTransactionRes QueryUploadTransaction(QueryTransactionReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetTerminalDetails", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service get all Terminals Authorised for the Agent. For Technical support, contact itsupport@chams.com ")]
        GetTerminalDetailsRes GetTerminalDetails(GetTerminalsReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetTerminal", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to a Terminal Information. For Technical support, contact itsupport@chams.com ")]
        GetTerminalsRes GetTerminal(FindTerminalReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetTerminals", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service get all Terminals Authorised for the Agent. For Technical support, contact itsupport@chams.com ")]
        GetTerminalsRes GetTerminals(GetTerminalsReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FindTerminal", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to a Terminal Information. For Technical support, contact itsupport@chams.com ")]
        GetTerminalsRes FindTerminal(FindTerminalReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetRevenue", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to download revenue head Information. For Technical support, contact itsupport@chams.com ")]
        GetServiceRevenueRes GetRevenue(ServiceRevenueReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/FindRevenue", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to Get the details of a revenue Item. For Technical support, contact itsupport@chams.com ")]
        GetServiceRevenueRes FindRevenue(ServiceFindRevenueReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetLattestRevenue", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to download lattest Revenue Information. For Technical support, contact itsupport@chams.com ")]
        GetServiceRevenueRes GetLattestRevenue(ServiceRevenueReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetAgentLocations", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to get all locations details for the agent. For Technical support, contact itsupport@chams.com ")]
        GetAgentLocationsRes GetAgentLocations(GetAgentLocationsReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetAllTransactions", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to get all transactions. This service was made due to the needs of Express pay")]
        GetAllTransactionRes GetAllTransactions(GetAllTransactionRequest req);

        //service interface for End of day transaction
        #region EOD SERVICE INTERFACE
        /// <summary>
        /// POST method called by terminal vendor to create new EOD
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/EOD/CreateEODTransaction", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to create new EndOfDay transaction. For Technical support, contact itsupport@chams.com ")]
        CreateEndOfDayRes CreateEODTransaction(CreateEndOfDayReq req);

        /// <summary>
        /// POST method called by NIBSS to check if an EndOfDay transaction exists
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/EOD/ValidateEODTransaction", BodyStyle = WebMessageBodyStyle.Bare)]
        //[Description("Call this service to check if an EndOfDay transaction exists. For Technical support, contact itsupport@chams.com ")]
        //ValidateEndOfDayRes ValidateEODTransaction(ValidateEndOfDayReq req);

        /// <summary>
        /// POST method called by terminal vendor to check the payment status of an EndOfDay transaction
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/EOD/QueryEODStatus", BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebInvoke(Method = "POST", UriTemplate = "/EOD/QueryEODStatus", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to check the payment status of an EndOfDay transaction. For Technical support, contact itsupport@chams.com ")]
        QueryEndOfDayStatusRes QueryEODStatus(QueryEndOfDayStatusReq TransactionRef);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/EOD/ValidateTransaction", BodyStyle = WebMessageBodyStyle.Bare)]
        //[Description("Call this service to validate an EndOfDay transaction. For Technical support, contact itsupport@chams.com ")]
        //string ValidateTransaction(CustomStreamContent req);

        //[OperationContract]
        //[WebInvoke(Method = "POST", UriTemplate = "/EOD/SendNotification", BodyStyle = WebMessageBodyStyle.Bare)]
        //[Description("Call this service to validate an EndOfDay transaction. For Technical support, contact itsupport@chams.com ")]
        //string SendNotification(NotificationRequest req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/EOD/BulkPushEodTransactions", BodyStyle = WebMessageBodyStyle.Bare)]
        //[WebInvoke(Method = "POST", UriTemplate = "/EOD/QueryEODStatus", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to push Bulk EOD Transaction. For Technical support, contact itsupport@chams.com ")]
        List<CreateEndOfDayRes> BulkPushEodTransactions(List<EodRequest> request);
        #endregion
    }

}
