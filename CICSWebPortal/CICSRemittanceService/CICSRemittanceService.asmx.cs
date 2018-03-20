using CICSRemittanceService.Helpers;
using CICSRemittanceService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml;
using System.Xml.Serialization;

namespace CICSRemittanceService
{
    /// <summary>
    /// Summary description for CICSRemittanceService
    /// </summary>
    [WebService(Namespace = "")]
    [WebServiceBinding(ConformsTo = WsiProfiles.None)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CICSRemittanceService : System.Web.Services.WebService
    {
        public CICSRemittanceService()
        {

        }
        private RemittanceServiceHelper _serviceHelper = new RemittanceServiceHelper();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost/VEABMService/UpdateOrder", RequestNamespace = "http://localhost/VEABMService", ResponseNamespace = "http://localhost/VEABMService", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]    
        public string HelloWorld1()
        {
            return "Hello World";
        }

        [WebMethod]
        
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("/test", RequestNamespace = "/test", ResponseNamespace = "/test", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        public string Test(string data)
        {
            return "Hello World";
        }

        [WebMethod]
        //[NIBSSSoapExtensionAttribute]
        [SoapDocumentMethod(ParameterStyle = SoapParameterStyle.Bare, Use = System.Web.Services.Description.SoapBindingUse.Literal, RequestElementName ="")]
        //[XmlSerializerFormat(Style = OperationFormatStyle.Document, SupportFaults =true, Use = OperationFormatUse.Literal)]
        //[WebInvoke(BodyStyle = WebMessageBodyStyle.Bare, RequestFormat = WebMessageFormat.Xml)]
        //[System.Web.Services.Protocols.SoapDocumentMethodAttribute("/ValidateTransaction", RequestNamespace = "", ResponseNamespace = "", Use = System.Web.Services.Description.SoapBindingUse.Default, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Bare)]
        public string ValidateTransaction(ValidationRequest req)
        {           
            return "test";
            /*//var stream = streamContent.ReadAsStreamAsync().Result;
            ////conversion
            //StreamReader reader = new StreamReader(stream);
            //string text = reader.ReadToEnd();
            //text.FromXml(typeof(ValidationRequest));
            //var req = new ValidationRequest();

            var res = new ValidationResponse();
            //var paramList = new List<Param>();
            string errorCode = "";
            if (req == null)
            {
                throw new ArgumentNullException("Invalid Status Query Request");
            }
            string msg = string.Empty;
            string terminalCode = string.Empty;

            //Extract params key value pairs
            //var req = request.ExtractNotificationRequest();
            //var reqObject = request.FromXml(new ValidationRequest().GetType());
            //var req = reqObject as ValidationRequest;

            try
            {
                var isValid = _serviceHelper.ValidateEODTransaction(req, out msg, out List<Param> param, out errorCode);
                res.Param = param;
                if (isValid)
                {
                    res.ResponseCode = NIBSSResponseHelper.ApprovedOrCompleted;
                    res.NextStep = 0;
                    res.BillerID = req.BillerID;
                }
                else
                {
                    //Log Failed Upload Request(req);
                    //string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(req);

                    //====Log Failed status queries to File===
                    //Logger.logToFile(msgLog, DebugLogPath + "\\Failed_EOD_Validation\\", true, terminalCode, "xml", true);

                    //====Log Failed Upload to Database====
                    //ServiceHelper.UploadExceptionToDb(msg, req);

                    res.ResponseCode = errorCode;
                }
            }
            catch (Exception e)
            {
                //Logger.logToFile(e, ErrorLogPath);
                res.ResponseCode = NIBSSResponseHelper.SystemMalfunction;
            }
            res.ResponseMessage = NIBSSResponseHelper.GetResponseMessage(res.ResponseCode);
            return res.SerializeNIBSSResponse();*/
        }
    }
}
