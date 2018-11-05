using ChamsICSLib.Data;
using ChamsICSLib.Utilities;
using ChamsICSWebService.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Net;
using System.Xml.Linq;
using ChamsICSWebService.HttpAccessors;

namespace ChamsICSWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public partial class ChamsICSService : iChamsICSService, iChamsICSPortalService
    {
        ServiceHelper ServiceHelper = new ServiceHelper();

        static string DebugLogPath = ConfigurationManager.AppSettings["DebugLoggingPath"];
        static string ErrorLogPath = ConfigurationManager.AppSettings["ErrorLoggingPath"];
        static string AllowBulkEodPush = ConfigurationManager.AppSettings["AllowBulkEODPush"];

        public AuthoriseTerminalRes AuthoriseTerminal(AuthoriseTerminalReq req)
        {
            AuthoriseTerminalRes res = new AuthoriseTerminalRes();
            if (req == null || req.AgentCode == null || req.TerminalName == null || req.TerminalSerialNumber == null)
            {
                throw new ArgumentNullException("Invalid AuthoriseTerminal Request");
            }

            try
            {
                Response response;
                var authorise = ServiceHelper.AuthoriseRequest(req.AgentCode, req.UserName, req.Password, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }

                //Validate AgentCode
                string msg = string.Empty;
                bool ValidateRes = ServiceHelper.ValidateAuthoriseTerminal(req, out msg);
                if (!ValidateRes)
                {
                    string err = msg;
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = err;
                    return res;
                }
                //Generate New TerminalCode
                String TerminalCode = ServiceHelper.GetNewTerminalCode(req);
                if (TerminalCode == null || TerminalCode == String.Empty)
                {
                    res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                    res.ResponseDescription = "Unable To Authorise Terminal";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                    res.TerminalCode = TerminalCode;
                    res.TerminalSerialNumber = req.TerminalSerialNumber;
                    return res;
                }
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Application Error";
                return res;
            }

        }

        public VerifyResidentIdRes VerifyResidentId(VerifyResidentIdReq req)
        {
            VerifyResidentIdRes res = new VerifyResidentIdRes();

            try
            {
                if (req == null || req.AgentCode == null || req.TerminalCode == null || req.ResidentId == null)
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Invalid Requests";
                    return res;
                }
                Response response;
                var authorise = ServiceHelper.AuthoriseRequest(req.AgentCode, req.UserName, req.Password, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }

                //Validate AgentCode
                string msg = string.Empty;
                bool ValidateRes = ServiceHelper.ValidateAgentCode(req.AgentCode, out msg);
                if (!ValidateRes)
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = msg;
                    return res;
                }

                IdentityService serviceUrl = ServiceHelper.GetClientIdentityService(req.TerminalCode);
                //Validate serviceURL Before Proceeding...
                //Debug=== Remove before live release build
                Logger.logToFile(DebugLogPath, "ServiceUrl-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", serviceUrl.URL);

                if (serviceUrl == null || serviceUrl.URL.Trim() == "" || serviceUrl.URL == string.Empty)
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Invalid Terminal Code";
                    return res;
                }
                try
                {
                    int ResidencyIdStatus = 0;
                    VerifyIdResponse resident = ServiceHelper.ValidateResidentID(serviceUrl.URL, serviceUrl.Username, serviceUrl.Password, req.ResidentId, out ResidencyIdStatus);

                    if (resident == null)
                    {
                        res.ResponseCode = ResponseHelper.FAILED;
                        res.ResponseDescription = "Inner Service Error. Unable to Complete Resident ID Validation";
                        return res;
                    }
                    else
                    {
                        res.ResponseCode = resident.ResponseCode;
                        res.ResponseDescription = resident.ResponseDescription;
                        res.ResidentId = req.ResidentId;
                        res.FirstName = resident.FIRSTNAME;
                        res.MiddleName = resident.MIDDLENAME;
                        res.LastName = resident.SURNAME;
                        res.Address = resident.RESIDENTIAL_ADDRESS;
                        res.Email = resident.EMAIL;
                        res.PhoneNumber = resident.MOBILENUMBER;
                        res.DateOfBirth = resident.DOB;
                        res.Gender = resident.GENDER;
                    }
                    return res;
                }
                catch (Exception e)
                {
                    Logger.logToFile(e, ErrorLogPath);
                    res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                    res.ResponseDescription = "Application Error";
                    return res;
                }
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Application Error";
                return res;
            }
        }

        public async Task<VerifyResidentRes> VerifyAnambraResidentID(VerifyResidentIdReq req)
        {
            VerifyResidentRes res = new VerifyResidentRes();

            try
            {
                if (req == null || req.AgentCode == null || req.TerminalCode == null || req.ResidentId == null)
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Invalid Requests";
                    return res;
                }
                Response response;
                var authorise = ServiceHelper.AuthoriseRequest(req.AgentCode, req.UserName, req.Password, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }

                //Validate AgentCode
                string msg = string.Empty;
                bool ValidateRes = ServiceHelper.ValidateAgentCode(req.AgentCode, out msg);
                if (!ValidateRes)
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = msg;
                    return res;
                }

                IdentityService serviceUrl = ServiceHelper.GetClientIdentityService(req.TerminalCode);
                //Validate serviceURL Before Proceeding...
                //Debug=== Remove before live release build
                Logger.logToFile(DebugLogPath, "ServiceUrl-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt", serviceUrl.URL);

                if (serviceUrl == null || serviceUrl.URL.Trim() == "" || serviceUrl.URL == string.Empty)
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Invalid Terminal Code";
                    return res;
                }
                try
                {
                    MemberRequest request = new MemberRequest { OtherSearch = req.ResidentId };
                   
                    if (int.TryParse(req.ResidentId, out int result) == true)
                    {
                        request.Id = result;
                        request.SearchType = SearchType.Id;
                    }
                    else
                    {
                        request.SearchType = SearchType.OtherSearch;
                    }

                    // VerifyIdResponse resident;
                    List<MemberResponse> residents = null;
                    var httpresp = await HttpRequestFactory.Post(serviceUrl.URL.Trim(), request);

                    if (httpresp.IsSuccessStatusCode)
                    {
                        residents = await httpresp.ContentAsTypeAsync<List<MemberResponse>>();
                        res.ResponseCode = httpresp.StatusCode.ToString();
                        res.ResponseDescription = httpresp.ReasonPhrase;
                        foreach(var resident in residents )
                        {
                            Resident currentresident = new Resident();
                            currentresident.ResidentId = resident.RESIDENCYID;
                            currentresident.FirstName = resident.FIRSTNAME;
                            currentresident.MiddleName = resident.MIDDLENAME;
                            currentresident.LastName = resident.SURNAME;
                            currentresident.Address = resident.RESIDENTIAL_ADDRESS;
                            currentresident.Email = resident.EMAIL;
                            currentresident.PhoneNumber = resident.MOBILE;
                            currentresident.DateOfBirth = resident.DOB;
                            currentresident.Gender = resident.GENDER;
                            currentresident.WebAccessPin = resident.WEBACCESSPIN;
                            res.Residents.Add(currentresident);
                        }
                        return res;
                    }
                    else
                    {
                        res.ResponseCode = httpresp.StatusCode.ToString();
                        res.ResponseDescription = httpresp.ReasonPhrase;
                        return res;
                    }
                   
                }
                catch (Exception e)
                {
                    Logger.logToFile(e, ErrorLogPath);
                    res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                    res.ResponseDescription = "Application Error";
                    return res;
                }
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Application Error";
                return res;
            }
        }



        public UploadTransactionRes UploadTransaction(UploadTransactionReq req)
        {
            UploadTransactionRes res = new UploadTransactionRes();
            if (req == null)
            {
                throw new ArgumentNullException("Invalid UploadTransaction Request");
            }

            string msg = string.Empty;
            string tempResId = string.Empty;

            //Validate Upload Data
            try
            {
                bool ValidateRes = ServiceHelper.ValidateUploadTransaction(req, out msg, out tempResId);
                if (!ValidateRes)
                {
                    //Log Failed Upload Request
                    string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(req);

                    //====Log Failed Upload to File===
                    Logger.logToFile(msgLog, DebugLogPath + "\\Failed_Upload\\", true, req.TransactionCode, "xml", true);

                    //====Log Failed Upload to Database====
                    ServiceHelper.UploadExceptionToDb(msg, req);

                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                    res.ResidentId = req.ResidentId;
                    res.TempResidentId = tempResId;
                    res.TransactionCode = req.TransactionCode;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Application Error";
                return res;
            }
        }

        public WebTransactionRes ProcessWebTrancation(WebTransactionReq webTransactionReq)
        {
            WebTransactionRes res = new WebTransactionRes();
            if (webTransactionReq == null)
            {
                throw new ArgumentNullException("Invalid UploadTransaction Request");
            }

            string msg = string.Empty;
            string tempResId = string.Empty;

            //Validate Upload Data
            try
            {
                bool ValidateRes = ServiceHelper.ValidateAndSaveWebTransaction(webTransactionReq, out msg, out tempResId, out string transactioncode, out string remittanceCode, out ChamsICSLib.Data.Terminal terminal);
                if (!ValidateRes)
                {
                    //Log Failed Upload Request
                    string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(webTransactionReq);

                    //====Log Failed Upload to File===
                    Logger.logToFile(msgLog, DebugLogPath + "\\Failed_Upload\\", true, transactioncode, "xml", true);

                    //====Log Failed Upload to Database====
                    UploadTransactionReq req = new UploadTransactionReq
                    {
                        TransactionCode = transactioncode,
                        Amount = webTransactionReq.Amount.ToString(),
                        TerminalCode = terminal.Code,
                        FirstName = (terminal.UserDetails != null) ? terminal.UserDetails.ElementAt(0).Name : "",
                        PaymentReference = remittanceCode,
                        TransactionDate = DateTime.Now.ToString(),
                        Address = webTransactionReq.Address,
                        DateOfBirth = webTransactionReq.DateOfBirth,
                        Email = webTransactionReq.Email,
                        Gender = webTransactionReq.Gender,
                        LastName = webTransactionReq.LastName,
                        MiddleName = webTransactionReq.MiddleName,
                        PhoneNumber = webTransactionReq.PhoneNumber,
                        ResidentId = webTransactionReq.ResidentId,
                        RevenueCode = webTransactionReq.RevenueCode
                    };
                    ServiceHelper.UploadExceptionToDb(msg, req);

                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                    res.RemittanceCode = remittanceCode;
                    res.TransactionCode = transactioncode;
                    res.TerminalCode = terminal.Code;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Application Error";
                return res;
            }
        }

        public WebTransactionRes ProcessWebMultiTrancation(List<WebTransactionReq> webTransactionReq)
        {
            WebTransactionRes res = new WebTransactionRes();
            if (webTransactionReq == null)
            {
                throw new ArgumentNullException("Invalid UploadTransaction Request");
            }

            string msg = string.Empty;
            string tempResId = string.Empty;

            //Validate Upload Data
            try
            {
                bool ValidateRes = ServiceHelper.ValidateAndSaveMultiWebTransaction(webTransactionReq, out msg, out tempResId, out string transactioncode, out string remittanceCode, out ChamsICSLib.Data.Terminal terminal);
                if (!ValidateRes)
                {
                    //Log Failed Upload Request
                    string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(webTransactionReq);

                    //====Log Failed Upload to File===
                    Logger.logToFile(msgLog, DebugLogPath + "\\Failed_Upload\\", true, transactioncode, "xml", true);

                    //====Log Failed Upload to Database====
                    //UploadTransactionReq req = new UploadTransactionReq
                    //{
                    //    TransactionCode = transactioncode,
                    //    Amount = webTransactionReq.Amount.ToString(),
                    //    TerminalCode = terminal.Code,
                    //    FirstName = (terminal.UserDetails != null) ? terminal.UserDetails.ElementAt(0).Name : "",
                    //    PaymentReference = remittanceCode,
                    //    TransactionDate = DateTime.Now.ToString(),
                    //    Address = webTransactionReq.Address,
                    //    DateOfBirth = webTransactionReq.DateOfBirth,
                    //    Email = webTransactionReq.Email,
                    //    Gender = webTransactionReq.Gender,
                    //    LastName = webTransactionReq.LastName,
                    //    MiddleName = webTransactionReq.MiddleName,
                    //    PhoneNumber = webTransactionReq.PhoneNumber,
                    //    ResidentId = webTransactionReq.ResidentId,
                    //    RevenueCode = webTransactionReq.RevenueCode
                    //};
                    //ServiceHelper.UploadExceptionToDb(msg, req);

                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                    res.RemittanceCode = remittanceCode;
                    res.TransactionCode = transactioncode;
                    res.TerminalCode = terminal.Code;
                }
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Application Error";
                return res;
            }
        }

        public async Task<CashWorkIntegrationRes> SubmitInterswitchInvoiceAsync(CashWorxIntegration cashWorxIntegration)
        {
            CashWorkIntegrationRes res = new CashWorkIntegrationRes();
            if (cashWorxIntegration == null)
            {
                return null;
            }

            try
            {
               
                var serviceUrl = "https://handshake.cashworxportal.com/integration/index.php?r=mda/sendinvoice";
                var httpresp = await HttpRequestFactory.Post(serviceUrl.Trim(), cashWorxIntegration);

                if (httpresp!= null)
                {
                    res = await httpresp.ContentAsTypeAsync<CashWorkIntegrationRes>();                   
                }
                else
                {
                    res = null;
                }
            }
            catch (Exception ex)
            {
                res = null;
            }

            return res;
        }

        public QueryTransactionRes QueryUploadTransaction(QueryTransactionReq req)
        {
            string msg = string.Empty;
            QueryTransactionRes res = new QueryTransactionRes();
            if (req == null)
            {
                throw new ArgumentNullException("Invalid QueryUploadTransaction Request");
            }

            string TerminalCode = req.TransactionCode.Substring(4, 6);
            string AgentCode = req.TransactionCode.Substring(0, 4);

            //Validate TransactionCode IS Right Length
            if (!ServiceHelper.ValidateTransactionCodeLength(req.TransactionCode, out msg))
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = msg;
                return res;
            }

            //Validate TerminalCode Section of Transaction Code is Valid

            if (!ServiceHelper.ValidateTerminalCode(TerminalCode, out msg))
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = msg;
                return res;
            }

            //Validate AgentCode Section of Transaction Code is Valid and Terminal is Assigned to It
            if (!ServiceHelper.ValidateAgentCode(AgentCode, TerminalCode, out msg))
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = msg;
                return res;
            }

            //Get Transaction Details;
            TransactionLog tLog = ServiceHelper.QueryTransaction(req.TransactionCode);
            if (tLog == null)
            {
                res.ResponseCode = ResponseHelper.UNKNOWN_ERROR;
                res.ResponseDescription = "Unable to retrieve Transaction Details";
                return res;
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

                res.TerminalCode = req.TerminalCode;
                res.TransactionCode = tLog.Code;
                res.ResidentId = tLog.ResidentId;
                res.Address = tLog.Address;
                res.Amount = tLog.Amount.ToString();
                res.DateOfBirth = tLog.DateOfBirth.ToString();
                res.Email = tLog.Email;
                res.FirstName = tLog.FirstName;
                res.Gender = tLog.Gender;
                res.LastName = tLog.Lastname;
                res.MiddleName = tLog.MiddleName;
                res.PhoneNumber = tLog.PhoneNumber;
                res.RevenueCode = tLog.RevenueCode;
                res.TransactionDate = tLog.TransactionDate.ToString();
                res.UploadDate = tLog.UploadDate.ToString();
                res.PaymentReference = tLog.PaymentReference;

            }
            return res;
        }

        public GetTerminalsRes GetTerminal(FindTerminalReq req)
        {
            GetTerminalsRes res = new GetTerminalsRes();

            return res;
        }

        public GetTerminalDetailsRes GetTerminalDetails(GetTerminalsReq req)
        {
            GetTerminalDetailsRes res = new GetTerminalDetailsRes();

            return res;
        }

        public GetTerminalsRes FindTerminal(FindTerminalReq req)
        {
            GetTerminalsRes res = new GetTerminalsRes();
            try
            {
                Response response;
                var authorise = ServiceHelper.AuthoriseRequest(req.AgentCode, req.UserName, req.Password, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }

                res = ServiceHelper.FindServiceTerminal(req.AgentCode, req.TerminalCode);

                return res;
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public GetTerminalsRes GetTerminals(GetTerminalsReq req)
        {

            GetTerminalsRes res = new GetTerminalsRes();
            try
            {
                Response response;
                var authorise = ServiceHelper.AuthoriseRequest(req.AgentCode, req.UserName, req.Password, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }

                res = ServiceHelper.GetServiceTerminals(req.AgentCode);
                return res;
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public GetServiceRevenueRes GetRevenue(ServiceRevenueReq req)
        {
            GetServiceRevenueRes res = new GetServiceRevenueRes();
            try
            {
                Response response;
                var authorise = ServiceHelper.AuthoriseRequest(req.AgentCode, req.UserName, req.Password, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }
                res = ServiceHelper.GetServiceRevenue(req.AgentCode);

                return res;
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public GetServiceRevenueRes FindRevenue(ServiceFindRevenueReq req)
        {
            GetServiceRevenueRes res = new GetServiceRevenueRes();
            try
            {
                Response response;
                var authorise = ServiceHelper.AuthoriseRequest(req.AgentCode, req.UserName, req.Password, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }
                res = ServiceHelper.FindServiceRevenue(req);

                return res;
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public GetServiceRevenueRes GetLattestRevenue(ServiceRevenueReq req)
        {
            GetServiceRevenueRes res = new GetServiceRevenueRes();
            try
            {
                Response response;
                var authorise = ServiceHelper.AuthoriseRequest(req.AgentCode, req.UserName, req.Password, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }
                res = ServiceHelper.GetServiceRevenue(req.AgentCode);
                return res;
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public GetAgentLocationsRes GetAgentLocations(GetAgentLocationsReq req)
        {
            GetAgentLocationsRes res = new GetAgentLocationsRes();
            try
            {
                Response response;
                var authorise = ServiceHelper.AuthoriseRequest(req.AgentCode, req.UserName, req.Password, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }

                res = ServiceHelper.GetAgentLocations(req.AgentCode);
                return res;
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        bool iChamsICSPortalService.FundWallet(Model.Payment payment)
        {
            return ServiceHelper.FundWallet(payment);
        }

        /// <summary>
        /// method called by terminal vendors toCreate new EOD transaction
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public CreateEndOfDayRes CreateEODTransaction(CreateEndOfDayReq req)
        {
            var res = new CreateEndOfDayRes();
            if (req == null)
            {
                throw new ArgumentNullException("Invalid CreateEODTransaction Request");
            }
            string msg = string.Empty;
            string terminalCode = string.Empty;
            try
            {
                res = ServiceHelper.CreateEODTransaction(req, out msg, out bool isCreated);
                if (isCreated == false)
                {
                    //Log Failed Upload Request
                    string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(req);

                    //====Log Failed Upload to File===
                    Logger.logToFile(msgLog, DebugLogPath + "\\Failed_EOD_Transactions\\", true, terminalCode, "xml", true);

                    //====Log Failed Upload to Database====
                    //ServiceHelper.UploadExceptionToDb(msg, req);

                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                }
            }
            catch (Exception e)
            {
                Logger.logToFile(e, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = $"Application Error...\n{msg}...\n{e.Message}";
            }
            return res;
        }

        public QueryEndOfDayStatusRes QueryEODStatus(QueryEndOfDayStatusReq req)
        {
            var res = new QueryEndOfDayStatusRes();
            if (req == null)
            {
                throw new ArgumentNullException("Invalid Status Query Request");
            }
            string msg = string.Empty;
            string terminalCode = string.Empty;
            try
            {
                res = ServiceHelper.GetEODStatus(req, out msg);
                if (string.IsNullOrEmpty(res.Status))
                {
                    //Log Failed Upload Request
                    string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(req);

                    //====Log Failed status queries to File===
                    Logger.logToFile(msgLog, DebugLogPath + "\\Failed_EOD_Status_Queries\\", true, terminalCode, "xml", true);

                    //====Log Failed Upload to Database====
                    //ServiceHelper.UploadExceptionToDb(msg, req);

                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                }
            }
            catch (Exception e)
            {
                Logger.logToFile(e, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Application Error";
            }
            return res;
        }
        /// <summary>
        /// Method Called by NIBSS to validate transaction
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        //public string ValidateTransaction(CustomStreamContent streamContent)
        //{
        //    var stream = streamContent.ReadAsStreamAsync().Result;
        //    //conversion
        //    StreamReader reader = new StreamReader(stream);
        //    string text = reader.ReadToEnd();
        //    text.FromXml(typeof(ValidationRequest));
        //    var req = new ValidationRequest() ;

        //    var res = new ValidationResponse();
        //    //var paramList = new List<Param>();
        //    string errorCode = "";
        //    if (req == null)
        //    {
        //        throw new ArgumentNullException("Invalid Status Query Request");
        //    }
        //    string msg = string.Empty;
        //    string terminalCode = string.Empty;

        //    //Extract params key value pairs
        //    //var req = request.ExtractNotificationRequest();
        //    //var reqObject = request.FromXml(new ValidationRequest().GetType());
        //    //var req = reqObject as ValidationRequest;

        //    try
        //    {
        //        var isValid = ServiceHelper.ValidateEODTransaction(req, out msg, out List<Param> param, out errorCode);
        //        res.Param = param;
        //        if (isValid)
        //        {
        //            res.ResponseCode = NIBSSResponseHelper.ApprovedOrCompleted;
        //            res.NextStep = 0;
        //            res.BillerID = req.BillerID;
        //        }
        //        else
        //        {
        //            //Log Failed Upload Request(req);
        //            string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(req);

        //            //====Log Failed status queries to File===
        //            Logger.logToFile(msgLog, DebugLogPath + "\\Failed_EOD_Validation\\", true, terminalCode, "xml", true);

        //            //====Log Failed Upload to Database====
        //            //ServiceHelper.UploadExceptionToDb(msg, req);

        //            res.ResponseCode = errorCode;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.logToFile(e, ErrorLogPath);
        //        res.ResponseCode = NIBSSResponseHelper.SystemMalfunction;
        //    }
        //    res.ResponseMessage = NIBSSResponseHelper.getResponseMessage(res.ResponseCode);
        //    return res.SerializeNIBSSResponse();
        //}



        //public string SendNotification(NotificationRequest req)
        //{
        //    var res = new NotificationResponse();
        //    string errorCode = "";
        //    if (req == null)
        //    {
        //        throw new ArgumentNullException("Invalid Status Query Request");
        //    }
        //    string msg = string.Empty;
        //    string terminalCode = string.Empty;
        //    //Extract params key value pairs
        //    //var req = request.ExtractNotificationRequest();
        //    //var reqObject = request.FromXml(new NotificationRequest().GetType());
        //    //var req = reqObject as NotificationRequest;
        //    try
        //    {
        //        var isValid = ServiceHelper.SendEODNotification(req, out msg, out List<Param> param, out errorCode);
        //        res.Param = param;
        //        if (isValid)
        //        {
        //            res.ResponseCode = NIBSSResponseHelper.ApprovedOrCompleted;
        //            res.BillerID = req.BillerID;
        //            res.SessionID = req.SessionID;
        //        }
        //        else
        //        {
        //            //Log Failed Upload Request(req);
        //            string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(req);

        //            //====Log Failed status queries to File===
        //            Logger.logToFile(msgLog, DebugLogPath + "\\Wrong_EOD_Notification\\", true, terminalCode, "xml", true);
        //            res.ResponseCode = errorCode;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Logger.logToFile(e, ErrorLogPath);
        //        res.ResponseCode = NIBSSResponseHelper.SystemMalfunction;
        //    }
        //    res.ResponseMessage = NIBSSResponseHelper.getResponseMessage(res.ResponseCode);

        //    return res.SerializeNIBSSResponse();
        //}

        /// <summary>
        /// method called by itex vendors to push all exiting EOD in a single push
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public List<CreateEndOfDayRes> BulkPushEodTransactions(List<EodRequest> request)
        {
            var result = new List<CreateEndOfDayRes>();
            if (int.TryParse(AllowBulkEodPush, out int x))
            {
                if (x != 1)
                {
                    result.Add(new CreateEndOfDayRes { ResponseCode = ResponseHelper.APPLICATION_ERROR, ResponseDescription = $"Application Error...Service Disabled" });
                    return result;
                }
            }
            else
            {
                result.Add(new CreateEndOfDayRes { ResponseCode = ResponseHelper.APPLICATION_ERROR, ResponseDescription = $"Application Error...Service Disabled : Bad format contact administrators.. *conf*" });
                return result;
            }
            if (request == null)
            {
                throw new ArgumentNullException("Invalid CreateEODTransaction Request");
            }
            string msg = string.Empty;
            string terminalCode = string.Empty;
            foreach (var requestmodel in request)
            {

                var res = new CreateEndOfDayRes();
                try
                {


                    var req = new CreateEndOfDayReq { Amount = requestmodel.TOTAL, EODCount = requestmodel.EODCOUNT, Username = "test_user", Password = "_ch@m5123", TerminalCode = requestmodel.TERMINALCODE };
                    res = ServiceHelper.CreateEODTransaction(req, out msg, out bool isCreated);
                    if (isCreated == false)
                    {
                        //Log Failed Upload Request
                        string msgLog = msg + Environment.NewLine + XMLHelper.serializeObjectToXMLString(req);

                        //====Log Failed Upload to File===
                        Logger.logToFile(msgLog, DebugLogPath + "\\Failed_EOD_Transactions\\", true, terminalCode, "xml", true);


                        res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                        res.ResponseDescription = msg;
                    }
                    else
                    {
                        res.ResponseCode = ResponseHelper.SUCCESS;
                        res.ResponseDescription = "Successful";
                    }




                }
                catch (Exception e)
                {
                    Logger.logToFile(e, ErrorLogPath);
                    res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                    res.ResponseDescription = $"Application Error...\n{msg}...\n{e.Message}";
                }
                result.Add(res);
            }
            return result;
        }

        public AuthoriseTerminalRes AuthoriseWebUser(AuthoriseWebUserReq req)
        {
            AuthoriseTerminalRes res = new AuthoriseTerminalRes();

            //GetAgentLocations the terminal request part of the request
            AuthoriseTerminalReq terminalReq = new AuthoriseTerminalReq
            {
                AgentCode = req.AgentCode,
                Password = req.Password,
                TerminalName = req.TerminalName,
                UserName = req.AgentUserName,
                TerminalSerialNumber = req.TerminalSerialNumber

            };
            //Get UserRequest section of the request
            Model.User userReq = new Model.User
            {
                ClientId = req.ClientId,
                Email = req.Email,
                Mobile = req.Mobile,
                Password = req.Password,
                RoleId = req.RoleId,
                RoleCode = req.RoleCode,
                PasswordStatus = req.PasswordStatus,
                Status = req.Status,
                UserId = req.UserId,
                UserTypeParentId = req.UserTypeParentId,
                AuditTrailData = req.AuditTrailData,
                userDetail = new UserDetailModel
                {
                    Address = req.userDetail.Address,
                    Name = req.userDetail.Name,
                    Email = req.userDetail.Email,
                    Firstname = req.userDetail.Firstname,
                    Lastname = req.userDetail.Lastname,
                    Sex = req.userDetail.Sex,
                    Middlename = req.userDetail.Middlename,
                    Website = req.userDetail.Website,
                    Zoneid = req.userDetail.Zoneid,
                    RegistrationNumber = req.userDetail.RegistrationNumber
                }
            };
            //Model.UserDetailReq userdetailReq = req.UserDetail;

            if (req == null || req.AgentCode == null)
            {
                throw new ArgumentNullException("Invalid AuthoriseTerminal Request");
            }

            if (res == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Request";

                return res;
            }
            if (!Utils.IsValidEmail(req.Email))
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Email";
                return res;

            }
            if (req.Mobile.Trim().Length < 11)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Mobile";
                return res;

            }
            if (ServiceHelper.EmailExists(req.Email))
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Email Already Exists";
                return res;
            }

            if (ServiceHelper.MobileExists(req.Mobile))
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Mobile Already Exists";
                return res;
            }



            try
            {
                //Authorise Agent Trying toCreateWeb USer
                Response response;
                var authorise = ServiceHelper.AuthoriseRequestForWebUSers(req.AgentCode, req.AgentUserName, out response);
                if (!authorise)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }

                //Validate Terminal
                string msg = string.Empty;
                bool ValidateRes = ServiceHelper.ValidateAuthoriseTerminal(terminalReq, out msg);
                if (!ValidateRes)
                {
                    string err = msg;
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = err;
                    return res;
                }


                int? userId = null;
                bool result = ServiceHelper.CreateWebUser(userReq, userReq.userDetail, terminalReq, out userId, out string TerminalCode);

                if (result == true)
                {
                    // String TerminalCode = ServiceHelper.GetNewTerminalCode(terminalReq);
                    if (TerminalCode == null || TerminalCode == String.Empty)
                    {
                        res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                        res.ResponseDescription = "Unable To Authorise Terminal";
                        return res;
                    }
                    else
                    {
                        res.ResponseCode = ResponseHelper.SUCCESS;
                        res.ResponseDescription = "Successful";
                        res.TerminalCode = TerminalCode;
                        res.TerminalSerialNumber = req.TerminalSerialNumber;
                        return res;
                    }
                }
                else
                {

                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "System cannot create User";
                    return res;
                }

                // res.ResponseCode = ResponseHelper.SUCCESS;
                //res.ResponseDescription = "User Created Succesfully";
                //Generate New TerminalCode

            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Application Error";
                return res;
            }
        }

        /// <summary>
        /// This Service method is Implemented to satisfy the needs of Expresspay
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public GetAllTransactionRes GetAllTransactions(GetAllTransactionRequest req)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            DateTime? startDate = null;
            DateTime? endDate = null;

            try
            {
                //convert date string to datetime
                startDate = Convert.ToDateTime(req.StartDate);
                endDate = Convert.ToDateTime(req.EndDate);
                //prep start date and end date
                startDate = new DateTime(startDate.Value.Year, startDate.Value.Month, startDate.Value.Day, 0, 0, 0);
                endDate = new DateTime(endDate.Value.Year, endDate.Value.Month, endDate.Value.Day, 23, 59, 59);
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Pls make sure dates are in required format";
                return res;
            }

            try
            {
                var isAuthenticated = ServiceHelper.AuthenticateUser(req.Email, req.Password, out Response response);
                if (!isAuthenticated)
                {
                    res.ResponseCode = response.ResponseCode;
                    res.ResponseDescription = response.ResponseDescription;
                    return res;
                }
                var userRes = ServiceHelper.GetUserByEmail(req.Email);
                if(userRes.ResponseCode == ResponseHelper.SUCCESS)
                {
                    //Get Transactions by the user's client id
                    res = ServiceHelper.GetAllTransactionByClientPerPeriod((int)userRes.User.ClientId, startDate, endDate);
                }
            }
            catch (Exception ex)
            {

                Logger.logToFile(ex, ErrorLogPath);
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = "Application Error";
            }
            return res;
        }
    }
}
