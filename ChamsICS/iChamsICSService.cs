using ChamsICSWebService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

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
        [WebInvoke(Method = "POST", UriTemplate = "/VerifyResidentId", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to verify the identity of a resident. For Technical support, contact itsupport@chams.com ")]
        VerifyResidentIdRes VerifyResidentId(VerifyResidentIdReq req);

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
        GetTerminalDetailsRes GetTerminalDetails(GetTerminalDetailsReq req);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/GetTerminal", BodyStyle = WebMessageBodyStyle.Bare)]
        [Description("Call this service to a Terminal Information. For Technical support, contact itsupport@chams.com ")]
        GetTerminalsRes GetTerminal(GetTerminalsReq req);



    }

}
