using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChamsICSWebService.Model;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;

namespace ChamsICSWebService
{
    public static class NIBSSResponseHelper
    {
        public const string ApprovedOrCompleted = "00";
        public const string StatusUnknown = "01";
        public const string InvalidSender = "03";
        public const string DoNotHonor = "05";
        public const string DormantAccount = "06";
        public const string InvalidAccount = "07";
        public const string AccountNameMismatch = "08";
        public const string RequestProcessingInProgress = "09";
        public const string InvalidTransaction = "12";
        public const string InvalidAmount = "13";
        public const string InvalidBatchNumber = "14";
        public const string InvalidSession = "15";
        public const string UnknownBankCode = "16";
        public const string InvalidChannel = "17";
        public const string WrongMethodCall = "18";
        public const string NoActionTaken = "21";
        public const string UnableToLocateRecord = "25";
        public const string DuplicateRecord = "26";
        public const string FormatError = "30";
        public const string SuspectedFraud = "34";
        public const string ContactSendingBank = "35";
        public const string NoSufficientFunds = "51";
        public const string TransactionNotPermittedToSender = "57";
        public const string TransactionNotPermittedOnChannel = "58";
        public const string TransferLimitExceeded = "61";
        public const string SecurityViolation = "63";
        public const string ExceedsWithdrawalFrequency = "65";
        public const string ResponseReceivedTooLate = "68";
        public const string AmountBlock = "69";
        public const string AmountUnblock = "70";
        public const string EmptyMandateReferenceNumber = "71";
        public const string BeneficiaryBankUnavailable = "91";
        public const string RoutingError = "92";
        public const string DuplicateTransaction = "94";
        public const string SystemMalfunction = "96";
        public const string TimeoutWaitingForResponse = "97";

        public static string getResponseMessage(string errorCode)
        {
            string responseMsg = "";
            switch (errorCode)
            {
                case ApprovedOrCompleted:
                    responseMsg = "Approved or completed successfully";
                    break;
                case InvalidAmount:
                    responseMsg = "Invalid Amount";
                    break;
                case InvalidTransaction:
                    responseMsg = "invalid Transaction";
                    break;
                case DuplicateTransaction:
                    responseMsg = "Duplicate Transaction";
                    break;
                case DuplicateRecord:
                    responseMsg = "Duplicate Record";
                    break;
                case SystemMalfunction:
                    responseMsg = "System Malfunction";
                    break;
                case FormatError:
                    responseMsg = "Format Error";
                    break;
                case InvalidSender:
                    responseMsg = "Invalid Sender. You are not authorized to make this request";
                    break;
                case UnableToLocateRecord:
                    responseMsg = "Unable to locate record";
                    break;
                default:
                    responseMsg = "Response code doesn't exist";
                    break;
            }
            return responseMsg;
        }

        public static string SerializeNIBSSResponse(this NIBSSEODServiceBaseRes res)
        {
            var stringwriter = new StringWriter();
            var serializer = new XmlSerializer(res.GetType());
            serializer.Serialize(stringwriter, res);
            var xmlString = stringwriter.ToString();

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(new StringReader(xmlString));

            var xmldecl = xmlDocument.CreateXmlDeclaration("1.0", "UTF-8", "yes");
            var text = xmlDocument.InnerXml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", "<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>")
                .Replace("<Params>", "").Replace("</Params>", "");

            stringwriter.Close();
            stringwriter.Dispose();

            return text;
        }
    }

    public static class NIBSSRequestHelper
    {
        public const string VALIDATIONREQUEST = "validation";
        public const string NOTIFICATIONREQUEST = "notification";

        //public static NotificationRequest ExtractNotificationRequest(this string req)
        //{
        //    XDocument xmldoc = XDocument.Parse(req);
        //    //xmlDoc.Element("Param").

        //    NotificationRequest request = new NotificationRequest();

        //    try
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(req);
        //        var xmlnode = xmlDoc.GetElementsByTagName("Param");
        //        for (int i = 0; i <= xmlnode.Count - 1; i++)
        //        {
        //            xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
        //            var key = xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
        //            var value = xmlnode[i].ChildNodes.Item(1).InnerText.Trim();
        //            request.Params.Add(new Param { Key = key, Value = value });
        //        }
        //        request.SessionID = XElement.Parse(req).Element("SessionID").Value;
        //    }
        //    catch (XmlException e)
        //    {
        //        throw e;
        //    }
        //    return request;
        //}

        //public static ValidationRequest ExtractValidationRequest(this string req)
        //{
        //    var requestString = req.Replace("<?xml version=\"1.0\" encoding=\"UTF - 8\"?>", "");

        //    var request = new ValidationRequest();
        //    try
        //    {
        //        XmlDocument xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(req);
        //        var xmlnode = xmlDoc.GetElementsByTagName("Param");
        //        for (int i = 0; i <= xmlnode.Count - 1; i++)
        //        {
        //            xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
        //            var key = xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
        //            var value = xmlnode[i].ChildNodes.Item(1).InnerText.Trim();
        //            request.Params.Add(new Param { Key = key, Value = value });
        //        }
        //        request.Amount = decimal.Parse(XElement.Parse(req).Element("Amount").Value);
        //        request.BillerID = int.Parse(XElement.Parse(req).Element("BillerID").Value);
        //    }
        //    catch (XmlException e)
        //    {
        //        throw e;
        //    }
        //    return request;
        //}

        public static object FromXml(this string Xml, System.Type ObjType)
        {
            var editedXml = Xml.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "").Replace("<?xml version=\"1.0\" encoding=\"UTF-8\"?>", "")
                .Replace("<?xml version=\"1.0\" encoding=\"utf-8\" standalone = \"yes\"?>", "");
            XmlSerializer ser;
            ser = new XmlSerializer(ObjType);
            StringReader stringReader;
            stringReader = new StringReader(editedXml);
            XmlTextReader xmlReader;
            xmlReader = new XmlTextReader(stringReader);
            object obj;
            obj = ser.Deserialize(xmlReader);
            xmlReader.Close();
            stringReader.Close();
            //object objectToReturn;
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(editedXml);
                var xmlnode = xmlDoc.GetElementsByTagName("Param");
                if (ObjType == new NotificationRequest().GetType())
                {
                    var notificationObj = obj as NotificationRequest;
                    for (int i = 0; i <= xmlnode.Count - 1; i++)
                    {
                        xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                        var key = xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                        var value = xmlnode[i].ChildNodes.Item(1).InnerText.Trim();
                        notificationObj.Param.Add(new Param { Key = key, Value = value });
                    }
                    return notificationObj;
                }
                if (ObjType == new ValidationRequest().GetType())
                {
                    var validationObj = obj as ValidationRequest;
                    for (int i = 0; i <= xmlnode.Count - 1; i++)
                    {
                        xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                        var key = xmlnode[i].ChildNodes.Item(0).InnerText.Trim();
                        var value = xmlnode[i].ChildNodes.Item(1).InnerText.Trim();
                        validationObj.Param.Add(new Param { Key = key, Value = value });
                    }
                    return validationObj;
                }
            }
            catch (XmlException)
            {
                throw;
            }
            return obj;
        }
    }
}