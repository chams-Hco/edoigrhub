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

namespace ChamsICSWebService
{
    public partial class ServiceHelper : IDisposable
    {
        CICSEntities db = DbInstance.Db();
        public void Dispose()
        {
            //Assume Gabage Collection will happen
        }

        public String GetNewTerminalCode(AuthoriseTerminalReq req)
        {
            string result = string.Empty;
            var ac = db.Agents.FirstOrDefault(x => x.Code == req.AgentCode.Trim());

            if (ac != null)
            {
                ChamsICSLib.Data.Terminal lastTerminal = db.Terminals.OrderByDescending(x=> x.Code).FirstOrDefault();
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
                db.Terminals.Add(terminal);
                db.SaveChanges();

                result = terminal.Code;
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

            //Validate TerminalCode
            string TerminalCode = req.TransactionCode.Substring(4, 6);
            ChamsICSLib.Data.Terminal terminal;
            if (!ValidateTerminalCode(TerminalCode, out msg, out terminal))
            {
                return false;
            }

            //Validate AgentCode
            string AgentCode = req.TransactionCode.Substring(0, 4);
            ChamsICSLib.Data.Agent agent;
            if (!ValidateAgentCode(AgentCode, terminal.Id, out msg, out agent))
            {
                return false;
            }

            if (terminal.AgentId!=agent.Id)
            {
                msg = "Incompatible Agent or Terminal Code Supplied in TransactionCode";
                return false;
            }

            //Validate Date Of Birth
            DateTime testDob;
            if (!(DateTime.TryParse(req.DateOfBirth,out testDob)))
            {
                msg = "Invalid Date Of Birth. Correct format is: [yyyy-MM-dd]. e.g, " + DateTime.Now.ToString("yyyy-MM-dd");

                return false;
            }

            //Validate Transaction Date
            if (!(DateTime.TryParse(req.TransactionDate, out testDob)))
            {
                msg = "Invalid Transaction Date. Correct format is: [yyyy-MM-dd]. e.g, " + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

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
            if (!Utils.IsValidEmail(req.Email))
            {
                msg = "Invalid Email: " + req.Email;

                return false;
            }

            //Validate FirstName and 
            string lName = req.LastName!=null ? req.LastName.Trim() : "";
            string fName = req.FirstName != null ? req.FirstName.Trim() : "";
            if (lName.Length<2 || fName.Length<2)
            {
                msg = "Invalid FirtName/Lastname";

                return false;
            }

            //Validate ResidentId and Upload
            if (!ValidateUploadResidentId(agent, terminal, req, out msg, out tempResId))
            {
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

        internal bool ValidateAgentCode(string agentCode, int terminalId, out string msg, out ChamsICSLib.Data.Agent agent)
        {
            bool result = true;

            //Validate AgentCode Exists
            agent = db.Agents.FirstOrDefault(x => x.Code == agentCode.Trim());
            if (agent == null)
            {
                msg = "Invalid AgentCode: "+agentCode;
                return false;
            }
            else
            {
                msg = string.Empty;
            }

            return result;
        }

        private bool ValidateUploadResidentId(ChamsICSLib.Data.Agent agent, ChamsICSLib.Data.Terminal terminal, UploadTransactionReq req, out string msg, out string tempResId)
        {
            msg = string.Empty;
            tempResId = string.Empty;

            try
            {
                //Get Client Details
                var client = db.Clients.FirstOrDefault(x => x.Id == agent.ClientId);
                if (client == null)
                {
                    msg = "Client Configuration Error.";
                    return false;
                }
                var identityService = client.IdentityServices.FirstOrDefault();

                if (identityService == null)
                {
                    msg = "Identity Services Configuration Error.";
                    return false;
                }
                //If the Terminal Provided the ResidendID, Attempt to Validate
                if (req.ResidentId != null && req.ResidentId.Trim() != string.Empty && req.ResidentId.Length!=0)
                {
                    VerifyIdResponse resident = ValidateResidentID(identityService.URL, req.ResidentId);
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
                            UploadTransaction(client.Id, agent.Id, terminal.Id, req);
                            tempResId = "";
                        }
                    }
                    else
                    {
                        msg = "Application Error.";
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

        internal TransactionLog QueryTransaction(string transactionCode)
        {
            TransactionLog tLog = db.TransactionLogs.FirstOrDefault(x=> x.Code==transactionCode.Trim());
            return tLog;
        }

        internal VerifyIdResponse ValidateResidentID(string URL, string ResidentId)
        {
            String ResponseText = string.Empty;
            VerifyIdResponse resident = new VerifyIdResponse() ;
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(URL);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {

                    string json = "{\"ResidentId\":"+ResidentId+"}";

                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream stream = httpResponse.GetResponseStream();
                DataContractJsonSerializer jsonData = new DataContractJsonSerializer(typeof(VerifyIdResponse));
                resident = (VerifyIdResponse)jsonData.ReadObject(stream);

                return resident;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        internal string UploadTransaction(int clientId, int agentId, int terminalId, UploadTransactionReq req, int status = 1)
        {
            string result = string.Empty;
            TransactionLog tLog = new TransactionLog();

            tLog.ClientId = clientId;
            tLog.AgentId = agentId;
            tLog.TerminalId = terminalId;
            tLog.ResidentId = req.ResidentId;
            tLog.Address = req.Address;
            tLog.Amount = Convert.ToDecimal(req.Amount);
            tLog.Code = req.TransactionCode;
            tLog.DateOfBirth = DateTime.Parse(req.DateOfBirth);
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

            //Debug=== Remove before Release Build    
            string TransactionLog = String.Empty;
            TransactionLog += clientId + Environment.NewLine;
            TransactionLog += agentId + Environment.NewLine;
            TransactionLog += terminalId + Environment.NewLine;
            TransactionLog += req.ResidentId + Environment.NewLine;
            TransactionLog += req.Address + Environment.NewLine;
            TransactionLog += req.Amount + Environment.NewLine;
            TransactionLog += req.TransactionCode + Environment.NewLine;
            TransactionLog += req.DateOfBirth + Environment.NewLine;
            TransactionLog += req.Email + Environment.NewLine;
            TransactionLog += req.FirstName + Environment.NewLine;
            TransactionLog += clientId + Environment.NewLine;
            TransactionLog += req.LastName + Environment.NewLine;
            TransactionLog += req.MiddleName + Environment.NewLine;
            TransactionLog += req.PhoneNumber + Environment.NewLine;
            TransactionLog += req.RevenueCode + Environment.NewLine;
            TransactionLog += DateTime.Now + Environment.NewLine;
            TransactionLog += req.TransactionDate + Environment.NewLine;
            TransactionLog += tLog.PaymentReference + Environment.NewLine;
            TransactionLog += status + Environment.NewLine;
            Logger.logToFile("C:\\CICS\\Logs\\DebugTrace\\", "UploadTransaction-" + Guid.NewGuid() + ".xml", TransactionLog);
            //Debug End ================================

            result = tLog.ResidentId;

            return result;
        }

        internal string UploadTemporaryTransaction(int clientId, int agentId, int terminalId, UploadTransactionReq req, out string tempResId)
        {
            string result = string.Empty;
            int status = 1;

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

        public bool ValidateTerminalCode(string terminalCode, out string msg, out ChamsICSLib.Data.Terminal terminal)
        {
            bool result = true;
            msg = string.Empty;

            terminal = db.Terminals.FirstOrDefault(x => x.Code == terminalCode);
            if (terminal == null)
            {
                msg = "Invalid TerminalCode: " + terminalCode;
                return false;
            }

            return result;
        }

        #region Client Opperations

        internal string GetClientServiceURL(string terminalCode)
        {
            string result = null;

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
                            string res = IdService.URL;
                            return res;
                        }
                    }
                }
            }
            return result;
        }
        #endregion
    }
}