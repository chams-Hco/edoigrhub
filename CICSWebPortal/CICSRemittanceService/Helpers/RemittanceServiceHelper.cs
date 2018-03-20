using ChamsICSLib.Data;
using CICSRemittanceService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CICSRemittanceService.Helpers
{
    public class RemittanceServiceHelper : IDisposable
    {
        CICSEntities db = new CICSEntities();
        internal bool ValidateEODTransaction(ValidationRequest req, out string msg, out List<Param> param, out string errorCode)
        {
            bool isValid = false;
            msg = "";
            errorCode = "";
            var data = SortParams(req.Param, NIBSSRequestHelper.VALIDATIONREQUEST, out msg);
            //var data = SortParams(new List<Param> { req.Param }, NIBSSRequestHelper.VALIDATIONREQUEST, out msg);
            param = new List<Param>();
            //exit method if there is an error msg
            if (msg != "")
            {
                errorCode = NIBSSResponseHelper.FormatError;
                return isValid;
            }
            //assign sorted params to their respective variables
            string remittanceCode = "";
            if (data.Length == 1)
            {
                remittanceCode = data[0];
            }
            //fetch transaction from DB
            var eod = db.EODs.SingleOrDefault(x => x.TransactionReference == remittanceCode && x.Status == false);
            if (eod == null)
            {
                msg = "Invalid End Of Day Transaction";
                errorCode = NIBSSResponseHelper.UnableToLocateRecord;
                param.Add(new Param { Key = "Status", Value = "InValid" });
                param.Add(new Param { Key = "RemittanceCode", Value = remittanceCode });
                param.Add(new Param { Key = "Amount", Value = eod.Amount.ToString() });
            }
            else
            {
                param.Add(new Param { Key = "Amount", Value = eod.Amount.ToString() });
                param.Add(new Param { Key = "PhoneNumber", Value = eod.Terminal.Agent.Phone1 ?? eod.Terminal.Agent.Phone2 });
                param.Add(new Param { Key = "Name", Value = eod.Terminal.Agent.Name });
                param.Add(new Param { Key = "Email", Value = eod.Terminal.Agent.Email });
                param.Add(new Param { Key = "Status", Value = "Valid" });
                param.Add(new Param { Key = "RemittanceCode", Value = remittanceCode });
            }
            return isValid;
        }

        internal bool SendEODNotification(NotificationRequest req, out string msg, out List<Param> param, out string errorCode)
        {
            bool isValid = false;
            msg = "";
            errorCode = "";
            var data = SortParams(req.Param, NIBSSRequestHelper.NOTIFICATIONREQUEST, out msg);
            //var data = SortParams(new List<Param> { req.Param }, NIBSSRequestHelper.NOTIFICATIONREQUEST, out msg);
            param = new List<Param>();
            //exit method if there is an error msg
            if (msg != "")
            {
                errorCode = NIBSSResponseHelper.FormatError;
                return isValid;
            }
            //assign sorted params to their respective variables
            string remittanceCode = "", amount = "", phoneNumber = "", email = "", name = "";
            if (data.Length == 5)
            {
                remittanceCode = data[4]; amount = data[0]; phoneNumber = data[1]; email = data[2]; name = data[3];
            }

            //convert amount string to decimal
            msg = Decimal.TryParse(amount, out decimal decimalAmount) ? "" : "Failed to convert string amount to decimal";
            // exit if there is an error msg in converting from string to decimal
            if (msg != "")
            {
                errorCode = NIBSSResponseHelper.FormatError;
                return isValid;
            }
            //fetch EOD
            var eod = db.EODs.SingleOrDefault(x => x.TransactionReference == remittanceCode && x.Amount == decimalAmount);

            if (eod == null)
            {
                errorCode = NIBSSResponseHelper.UnableToLocateRecord;
                return isValid;
            }
            else if (eod.Status)
            {
                errorCode = NIBSSResponseHelper.DuplicateTransaction;
                return isValid;
            }
            else
            {
                eod.Status = true;
                db.Entry(eod).State = System.Data.Entity.EntityState.Modified;
            }
            //insert into EOD payment log
            db.EODPaymentNotificationLogs.Add(new EODPaymentNotificationLog
            {
                BillerId = req.BillerID,
                BillerName = req.BillerName,
                ChannelCode = req.ChannelCode,
                CustomerName = req.CustomerName,
                DestinationBankCode = req.DestinationBankCode,
                EODId = eod.Id,
                Fee = req.Fee,
                Narration = req.Narration,
                PaymentReference = req.PaymentReference,
                ProductId = req.ProductID,
                ProductName = req.ProductName,
                SessionId = req.SessionID,
                SourceBankCode = req.SourceBankCode,
                SplitType = req.SplitType,
                TotalAmount = req.TotalAmount,
                TransactionApprovalDate = new DateTime(req.TransactionApprovalDate),
                TransactionFeeBearer = req.TransactionFeeBearer,
                TransactionInitiatedDate = new DateTime(req.TransactionInitiatedDate)
            });
            //set isValid depending on whether EOD status and payment log is saved
            isValid = db.SaveChanges() > 0 ? true : false;
            if (isValid == false)
            {
                errorCode = NIBSSResponseHelper.SystemMalfunction;
                param.Add(new Param { Key = "Amount", Value = amount });
                param.Add(new Param { Key = "PhoneNumber", Value = phoneNumber });
                param.Add(new Param { Key = "Name", Value = name });
                param.Add(new Param { Key = "Email", Value = email });
                param.Add(new Param { Key = "Status", Value = "Unpaid" });
                param.Add(new Param { Key = "RemittanceCode", Value = remittanceCode });
            }
            else
            {
                param.Add(new Param { Key = "Amount", Value = amount });
                param.Add(new Param { Key = "PhoneNumber", Value = phoneNumber });
                param.Add(new Param { Key = "Name", Value = name });
                param.Add(new Param { Key = "Email", Value = email });
                param.Add(new Param { Key = "Status", Value = eod.Status ? "Paid" : "Unpaid" });
                param.Add(new Param { Key = "RemittanceCode", Value = remittanceCode });
            }
            return isValid;
        }

        private string[] SortParams(List<Param> reqParams, string requestType, out string msg)
        {
            string username = null, password = null, amount = null, phoneNumber = null, email = null, name = null, remittanceCode = null;
            msg = "";
            var data = new string[] { };

            foreach (var item in reqParams)
            {
                switch (item.Key.ToLower())
                {
                    case "amount":
                        //msg = Decimal.TryParse(item.Value, out amount) ? "" : "Failed to convert string amount to decimal";
                        amount = item.Value;
                        break;
                    case "phonenumber":
                        phoneNumber = item.Value;
                        break;
                    case "email":
                        email = item.Value;
                        break;
                    case "name":
                        name = item.Value;
                        break;
                    case "remittancecode":
                        remittanceCode = item.Value;
                        break;
                    default:
                        msg = "Keys: Phone Number, Email, Amount, Name do not exist in the parameter being passed. Check XML data";
                        break;
                }
            }
            // msg is set if none of the cases are met for a param
            if (msg != "")
            {
                return data;
            }

            //data = new string[] { amount, phoneNumber, email, name, remittanceCode, username, password };
            data = new string[] { amount, phoneNumber, email, name, remittanceCode };

            var prepData = ValidateParams(data, requestType, out msg);

            return msg == "" ? prepData : data;
        }

        private string[] ValidateParams(string[] data, string requestType, out string msg)
        {
            var prepData = new string[] { };
            msg = "";
            switch (requestType)
            {
                case NIBSSRequestHelper.VALIDATIONREQUEST:
                    msg += string.IsNullOrEmpty(data[4]) ? "Remmittance parameter not provided" : "";
                    prepData = new string[] { data[4] };
                    break;
                case NIBSSRequestHelper.NOTIFICATIONREQUEST:
                    msg = string.IsNullOrEmpty(data[0]) ? "Amount parameter not provided" : "";
                    msg += string.IsNullOrEmpty(data[1]) ? "Phone number parameter not provided" : "";
                    msg += string.IsNullOrEmpty(data[2]) ? "Email parameter not provided" : "";
                    msg += string.IsNullOrEmpty(data[3]) ? "Name parameter not provided" : "";
                    msg += string.IsNullOrEmpty(data[4]) ? "Remmittance parameter not provided" : "";
                    prepData = new string[] { data[0], data[1], data[2], data[3], data[4] };

                    break;
                default:
                    msg = "Invalid request type";
                    break;
            }

            return prepData;
        }

        public void Dispose()
        {
            ((IDisposable)db).Dispose();
        }
    }
}