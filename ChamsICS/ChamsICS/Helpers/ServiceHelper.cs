using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChamsICSWebService.Model;
using ChamsICSLib.Data;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using ChamsICSLib.Utilities;
using System.Xml;
using System.Runtime.Serialization;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using ChamsICSLib;
using System.Configuration;
using System.Text;
using ChamsICSLib.Model;

namespace ChamsICSWebService
{
    public partial class ServiceHelper : IDisposable
    {
        CICSEntities db = new CICSEntities();
        static string DebugLogPath = ConfigurationManager.AppSettings["DebugLoggingPath"];
        static string ErrorLogPath = ConfigurationManager.AppSettings["ErrorLoggingPath"];
        static bool sms_notification = ConfigurationManager.AppSettings["sms_notification_for_transactions"]=="1"? true : false;
        static bool email_notification = ConfigurationManager.AppSettings["email_notification_for_transactions"] == "1" ? true : false;

        public void Dispose()
        {
            //Assume Gabage Collection will happen
        }

        internal bool AuthoriseRequest(String AgentCode, string Username, string Password, out Response res)
        {
            UserLoginRes loginRes = new UserLoginRes();
            res = new Response();

            var result = UserLogin(new UserLoginReq
            {
                Email = Username,
                UserPassword = Password
            },out loginRes);

            if (!result)
            {
                res.ResponseCode = loginRes.ResponseCode;
                res.ResponseDescription = loginRes.ResponseDescription;
                return false;
            }
            if (loginRes.PasswordStatus == 0)
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Account is not activated";
                return false;
            }
            if (loginRes.AccountStatus == 0)
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Account is disabled";
                return false;
            }
            //Validate that the request is comming in with the right AgentCode
            var agentId = loginRes.UserDashBoardData.UserTypeParentId;
            var userAgent = db.Agents.FirstOrDefault(x => x.Id == agentId && x.Code==AgentCode);

            if (userAgent == null)
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Unauthorised Login Credentials";
                return false;
            }
            return true;
        }

        public String GetNewTerminalCode(AuthoriseTerminalReq req)
        {
            string result = string.Empty;
            var ac = db.Agents.FirstOrDefault(x => x.Code == req.AgentCode.Trim());

            if (ac != null)
            {
                ChamsICSLib.Data.Terminal lastTerminal = db.Terminals.OrderByDescending(x => x.Code).FirstOrDefault();
                String TerminalCode = String.Empty;

                if (lastTerminal != null)
                {
                    TerminalCode = GetNextTerminalCode(lastTerminal.Code);
                }
                else
                {
                    TerminalCode = "A00000";
                }

                ChamsICSLib.Data.Terminal terminal = new ChamsICSLib.Data.Terminal();
                terminal.AgentId = ac.Id;
                terminal.Code = TerminalCode;
                terminal.SerialNumber = req.TerminalSerialNumber;
                terminal.Name = req.TerminalName;
                terminal.Status = 1;
                db.Terminals.Add(terminal);
                db.SaveChanges();

                result = terminal.Code;


                //Log Audit Trail
                var user = db.Users.FirstOrDefault(x => x.Email == req.UserName);
                LogAuditData(user.ClientId.Value, user.Id, AuditTrailType.CREATE, "Terminal", "TerminalCode: " + TerminalCode, "");
            }

            return result;
        }

        public string GetNextTerminalCode(string lastTerminalCode)
        {
            char IdChar = lastTerminalCode.ToCharArray()[0];
            int IdNum = Convert.ToInt32(lastTerminalCode.Substring(1, lastTerminalCode.Length - 1));

            if (IdNum < 99999)
            {
                IdNum += 1;
                string IdNumString = IdNum.ToString().PadLeft(5, '0');
                return IdChar.ToString() + IdNumString;
            }
            else
            {
                return GetNextTerminalIDChar(IdChar) + "00001";
            }
        }

        private char GetNextTerminalIDChar(char merchantIdChar)
        {
            char[] merchantIDChars = {'A','B','C','D','E','F','G','H','I','J','K','L','M','N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
                'W', 'X', 'Y' ,'Z'};

            int inCharPos = merchantIDChars.ToList().FindIndex(x => x == merchantIdChar);

            //if the character is less than 'Z' i.e position 25, take the next one
            if (inCharPos < 25)
                return merchantIDChars[inCharPos + 1];
            else
                throw new Exception("Merchant ID Band Exhausted");
        }

        private string GetTemporaryResidentId()
        {
            string tempResId = string.Empty;

            //Generate 7 Digits Random Numbers + Time Stamp [yyyyMMddhhmmss]
            string leftSide = Utils.GenerateSecretCode(6);
            string rightSide = DateTime.Now.ToString("yyyyMMddhhmmss");
            tempResId = leftSide + rightSide;

            return tempResId;
        }

        internal bool ValidateAuthoriseTerminal(AuthoriseTerminalReq req, out string msg)
        {
            bool result = true;
            msg = string.Empty;

            //Validate AgentCode Exists
            var ac = db.Agents.FirstOrDefault(x => x.Code == req.AgentCode.Trim());
            if (ac==null)
            {
                msg = "Invalid Agent Code";
                return false;
            }
            //Validate Terminal Serial Does Not Exists for AgentTerminals
            var ts = db.Terminals.FirstOrDefault(x => x.SerialNumber == req.TerminalSerialNumber.Trim());

            if (ts!=null)
            {
                msg = String.Format("Terminal Serial Numeber {0} Exists: ",req.TerminalSerialNumber);
                return false;
            }

            return result;
        }

        internal bool ValidateUploadTransaction(UploadTransactionReq req, out string msg, out string tempResId)
        {
            bool result = true;
            msg = string.Empty;
            tempResId = string.Empty;
            string outMsg = String.Empty;

            string TerminalCode = req.TransactionCode.Substring(4, 6);
            string AgentCode = req.TransactionCode.Substring(0, 4);

            //Validate TransactionCode
            if (!ValidateTransactionCodeLength(req.TransactionCode, out msg))
            {
                return false;
            }

            //Validate TransactionCode EXIST
            if (!ValidateTransactionCodeExist(req.TransactionCode, out msg))
            {
                return false;
            }

            //Validate Revenue EXIST
            if (!ValidateRevenueCode(req.RevenueCode, req.Amount, out msg))
            {
                return false;
            }
            //Validate TerminalCode
            if (!ValidateTerminalCode(TerminalCode, out msg))
            {
                return false;
            }

            //Validate AgentCode
            if (!ValidateAgentCode(AgentCode, TerminalCode, out msg))
            {
                return false;
            }

            //Validate Date Of Birth
            DateTime testDob;
            if (!(DateTime.TryParse(req.DateOfBirth,out testDob)))
            {
                msg = string.Format("Invalid Date Of Birth: {0} Correct format is: [yyyy-MM-dd]. e.g, {1} ",req.DateOfBirth,DateTime.Now.ToString("yyyy-MM-dd"));

                return false;
            }

            //Validate Transaction Date
            if (!(DateTime.TryParse(req.TransactionDate, out testDob)))
            {
                msg = string.Format("Invalid Transaction Date: {0} Correct format is: [yyyy-MM-dd HH:mm:ss]. e.g, ",req.TransactionDate, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                return false;
            }

            //Validate Amount
            Decimal testAmount;
            if (!(Decimal.TryParse(req.Amount, out testAmount)))
            {
                msg = "Invalid Amount: " + req.Amount;

                return false;
            }

            //Validate Email
            //if (!Utils.IsValidEmail(req.Email))
            //{
            //    msg = "Invalid Email: " + req.Email;

            //    return false;
            //}

            //Validate FirstName and 
            string lName = req.LastName!=null ? req.LastName.Trim() : "";
            string fName = req.FirstName != null ? req.FirstName.Trim() : "";
            if (lName.Length<2 || fName.Length<2)
            {
                msg = "Invalid FirtName/Lastname";

                return false;
            }

            //Validate ResidentId and Upload
            if (!ValidateUploadResidentId(AgentCode, TerminalCode, req, out msg, out tempResId))
            {
                msg = msg.Trim()==string.Empty? "Failed to Upload transaction" : msg;
                return false;
            }

            return result;
        }

        private bool ValidateRevenueCode(string revenueCode, out string msg)
        {
            bool result = true;
            msg = string.Empty;

            //Validate Revenue Code Exists
            var revenue = db.RevenueItems.FirstOrDefault(x => x.Code == revenueCode.Trim());
            if (revenue == null)
            {
                msg = "Invalid Revenue";
                return false;
            }
            return result;
        }

        private bool ValidateRevenueCode(string revenueCode, string revenueAmount, out string msg)
        {
            bool result = true;
            msg = string.Empty;

            //Validate Revenue Code Exists
            var revenue = db.RevenueItems.FirstOrDefault(x => x.Code == revenueCode.Trim());
            if (revenue == null)
            {
                msg = "Invalid Revenue";
                return false;
            }

            if (revenue.Amount!=0 && revenue.Amount!=decimal.Parse(revenueAmount))
            {
                msg = "Invalid Amount for Revenue: "+revenue.Name;
                return false;
            }
            return result;
        }

        internal bool ValidateAgentCode(string agentCode, out string msg)
        {
            bool result = true;
            msg = string.Empty;

            //Validate AgentCode Exists
            var agent = db.Agents.FirstOrDefault(x => x.Code == agentCode.Trim());
            if (agent == null)
            {
                msg = "Invalid AgentCode";
                return false;
            }

            return result;
        }

        internal ChamsICSLib.Data.Terminal FindTerminalByCode(string terminalCode)
        {
            return db.Terminals.FirstOrDefault(x => x.Code == terminalCode);
        }

        public bool ValidateTerminalCode(string terminalCode, out string msg)
        {
            bool result = true;
            msg = string.Empty;

            var terminal = FindTerminalByCode(terminalCode);
            if (terminal == null)
            {
                msg = "Invalid TerminalCode: " + terminalCode;
                return false;
            }
            if (terminal.Status == 0)
            {
                msg = "Inactive/Disabled Terminal ";
                return false;
            }

            return result;
        }

        internal ChamsICSLib.Data.Agent FindAgentByCode(string agentCode)
        {
            return db.Agents.FirstOrDefault(x => x.Code == agentCode.Trim());
        }

        internal bool ValidateAgentCode(string agentCode, string terminalCode, out string msg)
        {
            bool result = true;

            //Validate AgentCode Exists
            var agent = FindAgentByCode(agentCode);
            if (agent == null)
            {
                msg = "Invalid AgentCode: "+agentCode;
                return false;
            }

            //Validate Terminal Against Agent
            var terminal = FindTerminalByCode(terminalCode);
            if (terminal.AgentId != agent.Id)
            {
                msg = String.Format("Incompatible AgentCode:{0} and TerminalCode:{1} Supplied in TransactionCode",agentCode,terminalCode);
                return false;
            }
            msg = string.Empty;
            return result;
        }

        private bool ValidateUploadResidentId(string AgentCode, string TerminalCode, UploadTransactionReq req, out string msg, out string tempResId)
        {
            msg = string.Empty;
            tempResId = string.Empty;
            ChamsICSLib.Data.Agent agent = FindAgentByCode(AgentCode);
            ChamsICSLib.Data.Terminal terminal = FindTerminalByCode(TerminalCode);

            try
            {
                //Get Client Details
                var client = db.Clients.FirstOrDefault(x => x.Id == agent.ClientId);
                if (client == null)
                {
                    msg = "Client Configuration Error.";
                    return false;
                }
                var identityService = db.IdentityServices.FirstOrDefault(x => x.ClientId == client.Id);

                if (identityService == null)
                {
                    msg = "Identity Services Configuration Error.";
                    return false;
                }
                //If the Terminal Provided the ResidendID, Attempt to Validate
                if (req.ResidentId != null && req.ResidentId.Trim() != string.Empty && req.ResidentId.Length!=0)
                {
                    int ResidentIDStatus = 0;
                    VerifyIdResponse resident = ValidateResidentID(identityService.URL, identityService.Username, identityService.Password, req.ResidentId, out ResidentIDStatus);
                    if (resident != null)
                    {
                        if (resident.ResponseCode != "00")
                        {
                            //No Record For the ResidentId of this Upload found by the ResidentValidation Service
                            //Attempt to see if there are existing records matching FirstName, LastName and DOB in the Transaction Table
                            //If there are Existing Records, Retreive the Temporary ID and Use to Save the record but set status as Existing Temporary {3}
                            //If No Existing Matching Record, Upload the record in the temporary transaction table with status set as NEW TemporaryID {2}

                            UploadTemporaryTransaction(client.Id, agent.Id, terminal.Id, req, out tempResId);
                        }
                        //The Service Found a Matching Record in the Residency Database
                        else
                        {
                            UploadTransaction(client.Id, agent.Id, terminal.Id, req, ResidentIDStatus);
                            tempResId = "";
                        }
                    }
                    else
                    {
                        //Log Transaction in Exception Table
                        msg = "Internal Service Error: Unable to Complete Resident ID Validation.";
                       
                        return false;
                    }
                }
                //No ResidencyId was provided
                //Attempt to see if there are existing records matching FirstName, LastName and DOB in the Transaction Table
                //If there are Existing Records, Retreive the Temporary ID and Use to Save the record but set status as Existing Temporary {3}
                //If No Existing Matching Record, Upload the record in the temporary transaction table with status set as NEW TemporaryID {2}
                else
                {
                    UploadTemporaryTransaction(client.Id, agent.Id, terminal.Id, req, out tempResId);
                }
                return true;       
            }
            catch (Exception e)
            {
                msg = "Application Error: "+e.Message;
                return false;
            }            
        }

        internal GetServiceRevenueRes GetServiceRevenue(string agentCode)
        {
            GetServiceRevenueRes res = new GetServiceRevenueRes();

            var agent = db.Agents.FirstOrDefault(x => x.Code == agentCode);

            if (agent==null)
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Invalid Agent Code";

                return res;
            }

            int clientId = agent.ClientId.Value;
            var revenueItems = from x in db.RevenueItems
                               join y in db.RevenueCategories on x.CategoryId equals y.Id
                               join z in db.RevenueHeads on x.RevenueHeadId equals z.Id
                               join a in db.Ministries on x.MinistryId equals a.Id

                               where x.ClientId == clientId
                               where x.Status ==1
                               select new Model.ServiceRevenue
            {
                RevenueCode = x.Code,
                Name = x.Name,
                Amount = x.Amount.Value,
                Ministry = a.Name,
                RevenueHead = z.Name,
                Category = y.Name,
                Status = y.Status
            };
            
            res.ResponseCode = ResponseHelper.SUCCESS;
            res.ResponseDescription = "Success";
            res.ServiceRevenues = revenueItems.ToList();            
            return res;
        }

        internal GetServiceRevenueRes FindServiceRevenue(ServiceFindRevenueReq req)
        {
            GetServiceRevenueRes res = new GetServiceRevenueRes();

            var agent = db.Agents.FirstOrDefault(x => x.Code == req.AgentCode);

            if (agent==null)
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Invalid Agent Code";

                return res;
            }

            int clientId = agent.ClientId.Value;
            var revenueItems = from x in db.RevenueItems
                               join y in db.RevenueCategories on x.CategoryId equals y.Id
                               join z in db.RevenueHeads on x.RevenueHeadId equals z.Id
                               join a in db.Ministries on x.MinistryId equals a.Id

                               where x.ClientId == clientId
                               where x.Code == req.RevenueCode

                               select new Model.ServiceRevenue
            {
                RevenueCode = x.Code,
                Name = x.Name,
                Amount = x.Amount.Value,
                Ministry = a.Name,
                RevenueHead = z.Name,
                Category = y.Name,
                Status = x.Status
            };
            
            res.ResponseCode = ResponseHelper.SUCCESS;
            res.ResponseDescription = "Success";
            res.ServiceRevenues = revenueItems.ToList();            
            return res;
        }

        internal GetTerminalsRes FindServiceTerminal(string agentCode, string terminalCode)
        {
            GetTerminalsRes res = new GetTerminalsRes();

            var agent = db.Agents.FirstOrDefault(x => x.Code == agentCode);
            int agentId = agent.Id;
            
            var terminal = db.Terminals.Where(x => x.AgentId == agentId && x.Code==terminalCode).Select(obj => new Model.ServiceTerminal
            {
                Code = obj.Code,
                Name = obj.Name,
                SerialNumber = obj.SerialNumber
            });

            if (terminal.Count() > 0)
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Success";
                res.Terminals = terminal.ToList();
            }
            return res;
        }

        internal GetTerminalsRes GetServiceTerminals(string agentCode)
        {
            GetTerminalsRes res = new GetTerminalsRes();

            var agent = db.Agents.FirstOrDefault(x => x.Code == agentCode);
            int agentId = agent.Id;

            var terminal = db.Terminals.Where(x => x.AgentId == agentId).Select(obj => new Model.ServiceTerminal
            {
                Code = obj.Code,
                Name = obj.Name,
                SerialNumber = obj.SerialNumber
            });

            if (terminal.Count() > 0)
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Success";
                res.Terminals = terminal.ToList();
            }
            return res;
        }

        internal GetAgentLocationsRes GetAgentLocations(string agentCode)
        {
            GetAgentLocationsRes res = new GetAgentLocationsRes();

            var agent = db.Agents.FirstOrDefault(x => x.Code == agentCode);
            int agentId = agent.Id;

            var locations = db.TerminalLocations.Where(x => x.AgentId == agentId).Select(obj => new Model.TerminalLocation
            {
                LocationCode = obj.LocationCode,
                LocationDescription = obj.LocationDescription
            });

            if (locations.Count() > 0)
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Success";
                res.Locations = locations.ToList();
            }
            return res;
        }

        internal TransactionLog QueryTransaction(string transactionCode)
        {
            TransactionLog tLog = db.TransactionLogs.FirstOrDefault(x=> x.Code==transactionCode.Trim());
            return tLog;
        }

        internal VerifyIdResponse ValidateResidentID(string URL, string Username, string Password, string ResidentId, out int status)
        {
            //First Attempt to Validate the residency ID from Records in IGRHub Transaction Database
            //If the Resident ID was Found In the Database, Out Status Parameter will be 3
            String ResponseText = string.Empty;
            VerifyIdResponse resident = new VerifyIdResponse();

            var ResidentData = from tlog in db.TransactionLogs
                               where tlog.ResidentId.Trim() == ResidentId.Trim()

                               select new VerifyIdResponse()
                               {
                                   ResponseCode = "00",
                                   ResponseDescription = "OK",
                                    FIRSTNAME = tlog.FirstName,
                                    MIDDLENAME = tlog.MiddleName,
                                    SURNAME = tlog.Lastname,
                                    RESIDENTIAL_ADDRESS = tlog.Address,
                                    EMAIL = tlog.Email,
                                    MOBILENUMBER = tlog.PhoneNumber,
                                    DOB = tlog.DateOfBirth.Value.ToString(),
                                    GENDER = tlog.Gender
                               };

            if(ResidentData.Count()>0)
            {
                status = 3;
                return ResidentData.FirstOrDefault();
            }

            //If No Record Was Returned from the attempt above
            //Try Verifying the data from the Client Residency Web Service
            //If the Resident ID was Found In the Database, Out Status Parameter will be 1
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json =
                        "{"+
                        "\"Password\":" + "\""+Password + "\"" + ","+
                        "\"ResidentId\":" + "\"" + ResidentId + "\"" + "," +
                        "\"Username\":" + "\"" + Username + "\"" + "," +
                        "}";

                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = httpResponse.GetResponseStream();
                    DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(VerifyIdResponse));
                    resident = (VerifyIdResponse)jsonData.ReadObject(stream);

                    status = 1;
                    return resident;
                }
                else {
                    status = 0;
                    return null;
                }
                
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                status = 0;
                return null;
            }

        }

        internal string UploadTransaction(int clientId, int agentId, int terminalId, UploadTransactionReq req, int status = 1)
        {
            string result = string.Empty;
            TransactionLog tLog = new TransactionLog();

            //Retreive LocationID from DB based on Provided LocationCOde
            if (req.LocationCode != null)
            {
                ChamsICSLib.Data.TerminalLocation location = db.TerminalLocations.FirstOrDefault(x => x.LocationCode == req.LocationCode.Trim());
                if (location != null)
                {
                    tLog.LocationId = location.Id;
                }
            }

            //If the attempt above does not set a LocationId for the transaction, set it to 0
            //This will map to the default entry in the TerminalLocation Table to enable Reporting
            //the Location mapped to 0 is called "Not Specified" 
            // Location ID 0 maps to all transactions regardless of the Client or Agent in the entire system
            if (tLog.LocationId == null)
                tLog.LocationId = 0;

            tLog.ClientId = clientId;
            tLog.AgentId = agentId;
            tLog.TerminalId = terminalId;
            tLog.ResidentId = req.ResidentId;
            tLog.Address = req.Address;
            tLog.Amount = Convert.ToDecimal(req.Amount);
            tLog.Code = req.TransactionCode;
            tLog.DateOfBirth = DateTime.Parse(req.DateOfBirth );
            tLog.Email = req.Email;
            tLog.FirstName = req.FirstName;
            tLog.Gender = req.Gender;
            tLog.Lastname = req.LastName;
            tLog.MiddleName = req.MiddleName;
            tLog.PhoneNumber = req.PhoneNumber;
            tLog.RevenueCode = req.RevenueCode;
            tLog.TransactionDate = DateTime.Parse(req.TransactionDate);
            tLog.UploadDate = DateTime.Now;
            tLog.PaymentReference = req.PaymentReference;
            tLog.Status = status;

            db.TransactionLogs.Add(tLog);
            db.SaveChanges();

            LogTransactionNotification(tLog);
            //Debug End ================================

            result = tLog.ResidentId;

            return result;
        }

        internal string UploadTemporaryTransaction(int clientId, int agentId, int terminalId, UploadTransactionReq req, out string tempResId)
        {
            string result = string.Empty;
            int status = 0;

            DateTime dob = DateTime.Parse(req.DateOfBirth);
            var tempTransaction = db.TransactionLogs.FirstOrDefault(x => x.FirstName == req.FirstName.Trim()
            && x.Lastname == req.LastName.Trim() && x.DateOfBirth == dob);
            if (tempTransaction != null)
            {
                //There are Existing Records, Retreive the ID and Use to Save the record but set status as Existing ResidencyId {3}
                req.ResidentId = tempTransaction.ResidentId;
                status = 3;
                tempResId = tempTransaction.ResidentId;
            }
            else
            {
                //No Existing Matching Record, Upload the record in the temporary transaction table with status set as NEW TemporaryID {2}
                //Genereate New ReidentID is Status 
                string newTempResId = GetTemporaryResidentId();
                req.ResidentId = newTempResId;
                status = 2;
                tempResId = newTempResId;
            }

            UploadTransaction(clientId, agentId, terminalId, req, status);

            result = req.ResidentId;

            return result;
        }

        internal void UploadExceptionToDb(string msg, UploadTransactionReq req)
        {
            string TerminalCode = req.TransactionCode.Substring(4, 6);
            string AgentCode = req.TransactionCode.Substring(0, 4);

            ChamsICSLib.Data.Agent agent = FindAgentByCode(AgentCode);
            ChamsICSLib.Data.Terminal terminal = FindTerminalByCode(TerminalCode);

            ChamsICSLib.Data.Client client = null;
            if (agent!=null)
            client = db.Clients.FirstOrDefault(x => x.Id == agent.ClientId);

            string result = string.Empty;
            TransactionUploadError tLog = new TransactionUploadError();
            DateTime dateParser = DateTime.Now;

            //Retreive LocationID from DB based on Provided LocationCOde
            if (req.LocationCode != null)
            {
                ChamsICSLib.Data.TerminalLocation location = db.TerminalLocations.FirstOrDefault(x => x.LocationCode == req.LocationCode.Trim());
                if (location != null)
                {
                    tLog.LocationId = location.Id;
                }
            }

            //If the attempt above does not set a LocationId for the transaction, set it to 0
            //This will map to the default entry in the TerminalLocation Table to enable Reporting
            //the Location mapped to 0 is called "Not Specified" 
            // Location ID 0 maps to all transactions regardless of the Client or Agent in the entire system
            if (tLog.LocationId == null)
                tLog.LocationId = 0;

            if (client != null)
            tLog.ClientId = client.Id;
            if(agent!=null)
            tLog.AgentId = agent.Id;
            if(terminal!=null)
            tLog.TerminalId = terminal.Id;

            tLog.ResidentId = req.ResidentId;
            tLog.Address = req.Address;

            tLog.Amount = Convert.ToDecimal(req.Amount);
            tLog.Code = req.TransactionCode;

            tLog.DateOfBirth = DateTime.TryParse(req.DateOfBirth, out dateParser) ? dateParser : DateTime.Now;

            tLog.Email = req.Email;
            tLog.FirstName = req.FirstName;
            tLog.Gender = req.Gender;
            tLog.Lastname = req.LastName;
            tLog.MiddleName = req.MiddleName;
            tLog.PhoneNumber = req.PhoneNumber;
            tLog.RevenueCode = req.RevenueCode;

            tLog.TransactionDate = DateTime.TryParse(req.TransactionDate,out dateParser) ? dateParser : DateTime.Now;

            tLog.UploadDate = DateTime.Now;
            tLog.PaymentReference = req.PaymentReference;
            tLog.Status = 0;
            tLog.UploadError = msg;

            db.TransactionUploadErrors.Add(tLog);
            db.SaveChanges();
        }

        private void LogTransactionNotification(TransactionLog tLog)
        {
            //Log Notification
            //Template: Your Payment of {0} for {1} was processed successfully on {2}. Your Transaction Code: {3}.
            string transactionNaration = getTransactionNaration(tLog.ClientId, tLog.RevenueCode);

            string Message = String.Format("{0}||{1}||{2}||{3}", tLog.Amount, transactionNaration, tLog.TransactionDate, tLog.Code);

            if (sms_notification)
            {
                ChamsICSLib.Data.Notification smsNotification = new ChamsICSLib.Data.Notification
                {
                    TypeId = ChamsICSLib.Model.NotificationType.RESIDENT_SMS,
                    Message = Message,
                    Recipient = tLog.PhoneNumber,
                    ReferenceId = tLog.Id,
                    Status = 0
                };
                db.Notifications.Add(smsNotification); 
            }
            if (email_notification)
            {
                ChamsICSLib.Data.Notification emailNotification = new ChamsICSLib.Data.Notification
                {
                    TypeId = ChamsICSLib.Model.NotificationType.RESIDENT_EMAIL,
                    Message = Message,
                    Recipient = tLog.Email,
                    ReferenceId = tLog.Id,
                    Status = 0
                };
                db.Notifications.Add(emailNotification);
            }

            db.SaveChanges();
        }

        private string getTransactionNaration(int? clientId, string revenueCode)
        {
            List<string> data = new List<string>();

            var result = (from x in db.RevenueItems
                          where x.Code == revenueCode
                          where x.ClientId == clientId
                          join z in db.RevenueHeads on x.RevenueHeadId equals z.Id
                          join y in db.Ministries on x.MinistryId equals y.Id
                          join a in db.Clients on y.ClientId equals a.Id
                         
                         select new TransactionNaration { RevenueItem = x.Name, RevenueHead = z.Name, Ministry = y.Name, Client = a.Name}).FirstOrDefault();
            
            return result.RevenueItem + " / " + result.RevenueHead + " / " + result.Ministry+ " / " + result.Client;
        }

        public bool ValidateTransactionCodeLength(string TransactionCode, out string msg)
        {
            bool result = true;

            msg = string.Empty;
            //Validate TransactionCodeLength
            int tCodeLength = TransactionCode.Length;
            if (tCodeLength != 20)
            {
                msg = "Invalid TransactionCode Length.";
                return false;
            }

            return result;
        }

        public bool ValidateTransactionCodeExist(string TransactionCode, out string msg)
        {
            msg = string.Empty;
            //Validate TransactionCode
            var TransactionLog = db.TransactionLogs.FirstOrDefault(x => x.Code == TransactionCode.Trim());
            if (TransactionLog != null)
            {
                msg = "Transaction Code Already Exist: " + TransactionCode;
                return false;

            }
            else
            {
                return true;
            }
        }

        internal void LogAuditData(int? clientId, int? userId, string LogType, string TableAffected, string AuditData, string UndoCommand)
        {
            try
            {
                if (userId != null)
                {
                    if (clientId == null)
                    {
                        clientId = 0;
                    }
                    ChamsICSLib.Data.AuditTrail auditTrail = new ChamsICSLib.Data.AuditTrail();
                    auditTrail.ClientId = clientId;
                    auditTrail.UserId = userId;
                    auditTrail.LogType = LogType;
                    auditTrail.TableAffected = TableAffected;
                    auditTrail.AuditLog = AuditData;
                    auditTrail.UndoCommand = UndoCommand;
                    auditTrail.LogDate = DateTime.Now;

                    db.AuditTrails.Add(auditTrail);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                //Whatever Happens, Log it and move on...
                Logger.logToFile(ex, ErrorLogPath);
            }
        }

        #region Client Opperations
        internal IdentityService GetClientIdentityService(string terminalCode)
        {
            IdentityService result = null;

            var terminal = db.Terminals.FirstOrDefault(x => x.Code == terminalCode.Trim());

            if (terminal != null)
            {
                var agent = db.Agents.FirstOrDefault(x => x.Id == terminal.AgentId);

                if (agent != null)
                {
                    var client = db.Clients.FirstOrDefault(x => x.Id == agent.ClientId);
                    if (client != null)
                    {
                        var IdService = db.IdentityServices.FirstOrDefault(x => x.ClientId == client.Id);

                        if (IdService != null)
                        {
                            result = IdService;
                        }
                    }
                }
            }
            return result;
        }
        #endregion
    }
}