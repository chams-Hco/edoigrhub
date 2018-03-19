using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }

    public class NIBSSRequestHelper
    {
        public const string VALIDATIONREQUEST = "validation";
        public const string NOTIFICATIONREQUEST = "notification";
    }
}