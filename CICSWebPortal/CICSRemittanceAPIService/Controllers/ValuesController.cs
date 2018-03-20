using CICSRemittanceService.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace CICSRemittanceAPIService.Controllers
{
    public class ValuesController : ApiController
    {
        private RemittanceServiceHelper _serviceHelper = new RemittanceServiceHelper();
        // GET api/values
        public async Task<HttpResponseMessage> Get()
        {
            var stream = await Request.Content.ReadAsStreamAsync();

            var xmlDocument = new XmlDocument();
            xmlDocument.Load(stream);

            // Process XML document

            return new HttpResponseMessage();
        }

        // POST api/values
        [HttpPost]
        [Route("EODRemittance/ValidateTransaction")]
        public async Task<HttpResponseMessage> ValidateTransaction(ValidationRequest request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var stream = await Request.Content.ReadAsStreamAsync();
            var requestType = NIBSSRequestHelper.VALIDATIONREQUEST;
            var req = request.PrepRequest(stream, requestType) as ValidationRequest;

            string errorCode = "";
            string msg = string.Empty;
            string terminalCode = string.Empty;
            var res = new ValidationResponse();

            try
            {
                var isValid = _serviceHelper.PerformTransactionAction(req, requestType, out msg, out List<Param> param, out errorCode);
                res.Params = param;
                if (isValid)
                {
                    // Gabriel cross-check this line , this is meant to be a quick fix.. @teeCodez
                    res.PaymentDetail = new PaymentDetail { Amount = decimal.Parse(res.Params.SingleOrDefault(a => a.Key == "Amount").Value) };
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
            var xmlresponseString = res.SerializeNIBSSResponse();
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = new XmlContent(xmlresponseString, Encoding.UTF8);
            return responseMessage;
        }

        [HttpPost]
        [Route("EODRemittance/ValidateWithoutCode")]
        public async Task<HttpResponseMessage> ValidateWithoutCode(ValidationRequest request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var stream = await Request.Content.ReadAsStreamAsync();
            var requestType = NIBSSRequestHelper.VALIDATIONWITHOUTCODE;
            var req = request.PrepRequest(stream, requestType) as ValidationRequest;

            string errorCode = "";
            string msg = string.Empty;
            string terminalCode = string.Empty;
            var res = new ValidationResponse();

            try
            {
                var isValid = _serviceHelper.PerformTransactionAction(req, requestType, out msg, out List<Param> param, out errorCode);
                res.Params = param;
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
            var xmlresponseString = res.SerializeNIBSSResponse();
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = new XmlContent(xmlresponseString, Encoding.UTF8);
            return responseMessage;
        }

        [HttpPost]
        [Route("EODRemittance/SendNotification")]
        public async Task<HttpResponseMessage> SendNotification(NotificationRequest request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var stream = await Request.Content.ReadAsStreamAsync();
            var requestType = NIBSSRequestHelper.NOTIFICATIONREQUEST;
            var req = request.PrepRequest(stream, requestType) as NotificationRequest;

            var res = new NotificationResponse();
            string errorCode = "";

            string msg = string.Empty;
            string terminalCode = string.Empty;
            try
            {
                var isValid = _serviceHelper.SendEODNotification(req, out msg, out List<Param> param, out errorCode);
                res.Params = param;
                if (isValid)
                {
                    res.ResponseCode = NIBSSResponseHelper.ApprovedOrCompleted;
                    res.BillerID = req.BillerID;
                    res.SessionID = req.SessionID;
                }
                else
                {
                    //Log Failed Upload Request(req);
                    //string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(req);

                    //====Log Failed status queries to File===
                    //Logger.logToFile(msgLog, DebugLogPath + "\\Wrong_EOD_Notification\\", true, terminalCode, "xml", true);
                    res.ResponseCode = errorCode;
                }
            }
            catch (Exception e)
            {
                //Logger.logToFile(e, ErrorLogPath);
                res.ResponseCode = NIBSSResponseHelper.SystemMalfunction;
            }
            res.ResponseMessage = NIBSSResponseHelper.GetResponseMessage(res.ResponseCode);

            var xmlresponseString = res.SerializeNIBSSResponse();
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = new XmlContent(xmlresponseString, Encoding.UTF8);
            return responseMessage;
        }

        [HttpPost]
        [Route("EODRemittance/NotifyWithoutCode")]
        public async Task<HttpResponseMessage> NotifyWithoutCode(NotificationRequest request)
        {
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            var stream = await Request.Content.ReadAsStreamAsync();
            var requestType = NIBSSRequestHelper.NOTIFICATIONWITHOUTCODE;
            var req = request.PrepRequest(stream, requestType) as NotificationRequest;

            var res = new NotificationResponse();
            string errorCode = "";

            string msg = string.Empty;
            string terminalCode = string.Empty;
            try
            {
                var isValid = _serviceHelper.PerformTransactionAction(req, requestType, out msg, out List<Param> param, out errorCode);
                res.Params = param;
                if (isValid)
                {
                    res.ResponseCode = NIBSSResponseHelper.ApprovedOrCompleted;
                    res.BillerID = req.BillerID;
                    res.SessionID = req.SessionID;
                }
                else
                {
                    //Log Failed Upload Request(req);
                    //string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(req);

                    //====Log Failed status queries to File===
                    //Logger.logToFile(msgLog, DebugLogPath + "\\Wrong_EOD_Notification\\", true, terminalCode, "xml", true);
                    res.ResponseCode = errorCode;
                }
            }
            catch (Exception e)
            {
                //Logger.logToFile(e, ErrorLogPath);
                res.ResponseCode = NIBSSResponseHelper.SystemMalfunction;
            }
            res.ResponseMessage = NIBSSResponseHelper.GetResponseMessage(res.ResponseCode);

            var xmlresponseString = res.SerializeNIBSSResponse();
            responseMessage.StatusCode = HttpStatusCode.OK;
            responseMessage.Content = new XmlContent(xmlresponseString, Encoding.UTF8);
            return responseMessage;
        }
    }
}
