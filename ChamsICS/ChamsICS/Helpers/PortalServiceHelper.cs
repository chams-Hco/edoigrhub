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
using System.Data.Entity;
using ChamsICSLib.Model;
using System.Configuration;
using ChamsICSLib.Global;

namespace ChamsICSWebService
{
    public partial class ServiceHelper
    {
        Messaging Messaging = new Messaging();
        public string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
        public string emailPassword = ConfigurationManager.AppSettings["emailPassword"];
        public string mail_server = ConfigurationManager.AppSettings["mail_server"];
        public int mail_port = Convert.ToInt32(ConfigurationManager.AppSettings["mail_port"]);
        public string mail_from = ConfigurationManager.AppSettings["mail_from"];
        public string mail_sender = ConfigurationManager.AppSettings["mail_sender"];
        public string mail_name = ConfigurationManager.AppSettings["mail_name"];
        public string mail_pwd = ConfigurationManager.AppSettings["mail_pwd"];

        #region Client

        internal bool CreateClient(CreateClientReq req, out string ClientId, out string ClientCode, out string msg)
        {
            msg = string.Empty;
            ClientCode = string.Empty;
            ClientId = string.Empty;

            //Validate Form Data 
            string name = req.Name != null ? req.Name.Trim() : "";
            string address = req.Addres != null ? req.Addres.Trim() : "";

            if (name.Length < 2 || address.Length < 2)
            {
                msg = "Invalid Name/Address";

                return false;
            }

            //Validate Email
            if (!Utils.IsValidEmail(req.Email))
            {
                msg = "Invalid Email: " + req.Email;

                return false;
            }
            ChamsICSLib.Data.Client client = new ChamsICSLib.Data.Client();
            client.Code = GetNewClientCode();
            client.Name = req.Name;
            client.Email = req.Email;
            client.Address = req.Addres;
            client.Phone1 = req.Phone1;
            client.Phone2 = req.Phone2;
            client.Status = req.Status;
            db.Clients.Add(client);
            int res = db.SaveChanges();

            ClientCode = client.Code;
            ClientId = client.Id.ToString();

            //Log Audit Trail
            LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.CREATE, "Client", "ClientId:" + ClientId, "");

            return res > 0;
        }

        internal bool UpdateClient(UpdateClientReq req)
        {

            var client = db.Clients.FirstOrDefault(x => x.Id == req.ClientId);

            if (client != null)
            {
                client.Name = req.Name;
                client.Email = req.Email;
                client.Address = req.Address;
                client.Phone1 = req.Phone1;
                client.Phone2 = req.Phone2;
                client.Status = req.Status;
                db.Clients.Attach(client);
                db.Entry(client).State = EntityState.Modified;
                int res = db.SaveChanges();

                //Log Audit Trail
                if(res > 0)
                LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.EDIT, "Client", "ClientId:" + req.ClientId, "");

                return res > 0;
            }
            else { return false; }
        }

        internal string GetNewClientCode()
        {
            var lastClientClode = db.Clients.OrderByDescending(x => x.Code).FirstOrDefault().Code;
            if (lastClientClode != null)
            {
                int LCC;
                Int32.TryParse(lastClientClode, out LCC);
                int newlcc = LCC += 1;
                return newlcc.ToString();
            }
            else
            {
                return "10";
            }
        }

        internal FindClientRes FindClient(int clientId)
        {
            FindClientRes res = new FindClientRes();
            var client = db.Clients.FirstOrDefault(x => x.Id == clientId);

            if (client == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

                res.ClientId = client.Id.ToString();
                res.Code = client.Code;
                res.Name = client.Name;
                res.Email = client.Email;
                res.Addres = client.Address;
                res.Phone1 = client.Phone1;
                res.Phone2 = client.Phone2;
                res.status = client.Status.Value;
            }
            return res;
        }

        internal GetAllClientsRes GetAllClients()
        {
            GetAllClientsRes res = new GetAllClientsRes();

            var clients = db.Clients.Select(x => new Model.Client
            {
                ClientId = x.Id.ToString(),
                Code = x.Code,
                Name = x.Name,
                Addres = x.Address,
                Email = x.Email,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                status = x.Status.Value
            });


            res.Clients = clients;

            return res;
        }
        #endregion

        #region Terminal
        internal bool UpdateTerminalStatus(TerminalStatus req)
        {
            var user = db.Terminals.FirstOrDefault(x => x.Id == req.TerminalId);
            if (user != null)
            {
                user.Status = req.status;
                db.SaveChanges();

                //Log Audit Trail
                LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.EDIT, "Terminal", "Change Status for TerminalId:" + req.TerminalId, "");
                return true;
            }
            else
            {
                return false;
            }
        }

        internal FindTerminalRes FindTerminal(int id)
        {
            FindTerminalRes res = new FindTerminalRes();
            var terminal = db.Terminals.FirstOrDefault(x => x.Id == id);
            
            var terminalObj= new Model.Terminal
             {
                 TerminalId = terminal.Id,
                 AgentName = db.Agents.FirstOrDefault(y => y.Id == terminal.AgentId).Name,
                 AgentId = terminal.AgentId.Value,
                 Code = terminal.Code,
                 Name=terminal.Name,
                 SerialNumber = terminal.SerialNumber,
                 status = terminal.Status.Value
                
             };

            if (terminalObj == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Terminal = (Model.Terminal)terminalObj;

            }
            return res;
        }

        internal GetAllTerminalRes GetAllTerminals()
        {
            GetAllTerminalRes res = new GetAllTerminalRes();

            
            var terminal = db.Terminals.Where(x=> x.Status==1).Select(x => new Model.Terminal
            {
                TerminalId = x.Id,
                AgentName = db.Agents.FirstOrDefault(y => y.Id == x.AgentId).Name,
                AgentId = x.AgentId.Value,
                Name=x.Name,
                Code = x.Code,
                SerialNumber = x.SerialNumber,
                status = x.Status.Value
            });

            res.Terminals = terminal;
            
            return res;
        }

        internal GetAllTerminalRes GetAllTerminalsByAgentId(int agentId)
        {
            GetAllTerminalRes res = new GetAllTerminalRes();

            var terminal = db.Terminals.Where(x => x.Status == 1).Where(x => x.AgentId == agentId).Select(y => new Model.Terminal
            {
                TerminalId = y.Id,
                AgentName = db.Agents.FirstOrDefault(z => z.Id == y.AgentId).Name,
                AgentId = y.AgentId.Value,
                Code = y.Code,
                Name = y.Name,
                SerialNumber = y.SerialNumber,
                status = y.Status.Value
            });


            res.Terminals = terminal;

            return res;
        }

        internal GetAllTerminalRes GetAllTerminalsByClientId(int ClientId)
        {
            GetAllTerminalRes res = new GetAllTerminalRes();
            IList<int> ClientAgents = db.Agents.Where(x => x.Status == 1).Where(x => x.ClientId == ClientId).Select(x => x.Id).ToList();

            var Terminal = db.Terminals.Where(y => ClientAgents.Contains(y.AgentId.Value))
               .Select(t => new Model.Terminal
               {
                   TerminalId = t.Id,
                   AgentName = db.Agents.FirstOrDefault(z => z.Id == t.AgentId).Name,
                   AgentId = t.AgentId.Value,
                   Code = t.Code,
                   Name = t.Name,
                   SerialNumber = t.SerialNumber,
                   status = t.Status.Value
               });

            res.Terminals = Terminal;



            //res.Terminals = terminal.ToList();

            return res;
        }
        #endregion

        #region Agent
        internal bool CreateAgent(Model.Agent req, out string agentId, out string agentCode, out string msg)
        {
            bool result = true;
            msg = string.Empty;
            agentCode = string.Empty;
            agentId = string.Empty;

            //Validate Form Data 
            string name = req.Name != null ? req.Name.Trim() : "";
            string address = req.Address != null ? req.Address.Trim() : "";
            string comp = req.Company != null ? req.Company.Trim() : "";

            if (name.Length < 2 || address.Length < 2 || comp.Length < 2)
            {
                msg = "Invalid Name/Company/Address";

                return false;
            }

            //Validate Email
            if (!Utils.IsValidEmail(req.Email))
            {
                msg = "Invalid Email: " + req.Email;

                return false;
            }

            //Validate Client ID
            string newAgentCode = GetNewAgentCode(Convert.ToInt32(req.ClientId));
            if (newAgentCode == null)
            {
                msg = "Invalid Client Code";
                return false;
            }
            else
            {
                if (newAgentCode.Length>4)
                {
                    msg = "Agent Limit reached for this client.";
                    return false;

                }
                ChamsICSLib.Data.Agent agent = new ChamsICSLib.Data.Agent();
                agent.ClientId = req.ClientId;
                agent.Code = newAgentCode;
                agent.Name = req.Name;
                agent.Email = req.Email;
                agent.Address = req.Address;
                agent.Company = req.Company;
                agent.Phone1 = req.Phone1;
                agent.Phone2 = req.Phone2;
                agent.Status = req.status;

                db.Agents.Add(agent);
                db.SaveChanges();

                //Push out for Returned Result
                agentCode = agent.Code;
                agentId = agent.Id.ToString();

                //Log Audit Trail
                LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.CREATE, "Agent", "AgentId:" + agentId, "");
                result = true;
            }
            return result;
        }

        internal bool UpdateAgent(Model.Agent req)
        {
            var agent = db.Agents.FirstOrDefault(x => x.Id == req.Id);
            if (agent != null)
            {
                agent.Name = req.Name;
                agent.Email = req.Email;
                agent.Address = req.Address;
                agent.Company = req.Company;
                agent.Phone1 = req.Phone1;
                agent.Phone2 = req.Phone2;
                agent.Status = req.status;
                db.Agents.Attach(agent);
                db.Entry(agent).State = EntityState.Modified;

                int result = db.SaveChanges();

                //Log Audit Trail
                if(result > 0)
                LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.EDIT, "Agent", "AgentId:" + req.Id, "");

                return result > 0;
            }
            else { return false; }
        }

        private string GetNewAgentCode(int clientId)
        {
            //Get Last AgentCode from the Agents of the Same Client
            var lastAgentCode = db.Agents.Where(x => x.ClientId == clientId).OrderByDescending(x=> x.Code).FirstOrDefault();
            if (lastAgentCode != null)
            {
                //Get the Right Part of the Last Agent Code
                int LastAgentCodeRight = Convert.ToInt32(lastAgentCode.Code.Substring(2, 2));

                //Get the Laft Part of the Last Agent Code
                string LastAgentCodeLeft = lastAgentCode.Code.Substring(0,2);

                //Increment the Right Pat of the Last Agent Code by 1
                int newlac = LastAgentCodeRight += 1;
                //Pad it with zero in the left to ensure its two Characters
                string newAgentCodeString = newlac.ToString().PadLeft(2, '0');

                //Add the ClientCode to The Right and Return
                return LastAgentCodeLeft + newAgentCodeString;
            }
            else
            {
                string clientCode = string.Empty;
                //Get Last Client Code and Generate New One
                var client = db.Clients.Where(x => x.Id == clientId).FirstOrDefault();
                if (client != null)
                {
                    clientCode = client.Code;
                    return clientCode.Trim() + "01";
                }
                else
                {
                    return null;
                }
            }
        }

        internal FindAgentRes FindAgent(int id)
        {
            FindAgentRes res = new FindAgentRes();
            var agentObj= db.Agents.FirstOrDefault(x => x.Id == id);
            
           var agent= new Model.Agent
             {
                 Id = agentObj.Id,
                 ClientId = agentObj.ClientId.Value,
                 Code = agentObj.Code,
                 Company =agentObj.Company,
                 Name =agentObj.Name,
                 Address = agentObj.Address,
                 Email = agentObj.Email,
                 Phone1 = agentObj.Phone1,
                 Phone2 = agentObj.Phone2,
                 status = agentObj.Status.Value
             };

            if (agent == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Agent = (Model.Agent)agent;

            }
            return res;
        }

        internal GetAllAgentRes GetAllAgent()
        {
            GetAllAgentRes res = new GetAllAgentRes();
            var agent = db.Agents.Select(x => new Model.Agent
            {
                Id = x.Id,
                ClientId = x.ClientId.Value,
                Code = x.Code,
                Company = x.Company,
                Name = x.Name,
                Address = x.Address,
                Email = x.Email,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                status = x.Status.Value
            });


            res.Agents = agent;

            return res;
        }

        internal GetAllAgentRes GetAllAgentByClientId(int clientId)
        {
            GetAllAgentRes res = new GetAllAgentRes();
            var agent = db.Agents.Where(x => x.ClientId == clientId).Select(x => new Model.Agent
            {
                Id = x.Id,
                ClientId = x.ClientId.Value,
                Code = x.Code,
                Company = x.Company,
                Name = x.Name,
                Address = x.Address,
                Email = x.Email,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                status = x.Status.Value
            });


            res.Agents = agent;

            return res;
        }
        #endregion

        #region Transaction

        internal FindTransactionRes FindTransaction(int id)
        {
            FindTransactionRes res = new FindTransactionRes();
            var tLogsObj = db.TransactionLogs.FirstOrDefault(x => x.Id == id);
            var terminal = db.Terminals.FirstOrDefault(x => x.Id == tLogsObj.TerminalId);
            var revenue = db.RevenueItems.FirstOrDefault(x => x.Code == tLogsObj.RevenueCode.Trim());
            var agent = db.Agents.FirstOrDefault(x => x.Id == tLogsObj.AgentId);
            var revenueHead = db.RevenueHeads.FirstOrDefault(x => x.Id == revenue.RevenueHeadId);
            var ministry = db.Ministries.FirstOrDefault(x => x.Id == revenueHead.MinistryId);

            var tLogs = new Transaction
            {
                Id = tLogsObj.Id,
                AgentId = tLogsObj.AgentId.Value,
                ClientId = tLogsObj.ClientId.Value,
                TerminalId = tLogsObj.TerminalId.Value,
                TransactionCode = tLogsObj.Code,
                ResidentId = tLogsObj.ResidentId,
                FirstName = tLogsObj.FirstName,
                MiddleName = tLogsObj.MiddleName,
                LastName = tLogsObj.Lastname,
                Address = tLogsObj.Address,
                Email = tLogsObj.Email,
                PhoneNumber = tLogsObj.PhoneNumber,
                DateOfBirth = tLogsObj.DateOfBirth.Value,
                Gender = tLogsObj.Gender,
                RevenueCode = tLogsObj.RevenueCode,
                TerminalCode = terminal.Code,
                RevenueName = revenue != null ? revenue.Name : "",
                AgentName = agent != null ? agent.Name : "",
                Ministry = ministry != null ? ministry.Name : "",
                 RevenueHead = revenueHead!=null? revenueHead.Name:"",
                Amount = tLogsObj.Amount.Value,
                PaymentReference = tLogsObj.PaymentReference,
                TransactionDate = tLogsObj.TransactionDate.Value,
                UploadDate = tLogsObj.UploadDate.Value,
                status = tLogsObj.Status.Value
            };

            if (tLogs == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Transaction = (Model.Transaction)tLogs;

            }
            return res;
        }

        internal FindTransactionRes FindTransactionByCode(string transactionCode)
        {
            FindTransactionRes res = new FindTransactionRes();

            var tLogsObj = db.TransactionLogs.FirstOrDefault(x => x.Code == transactionCode);
           

            var terminal = db.Terminals.FirstOrDefault(x => x.Id == tLogsObj.TerminalId);
            var revenue = db.RevenueItems.FirstOrDefault(x => x.Code == tLogsObj.RevenueCode.Trim());
            var revenueHead = db.RevenueHeads.FirstOrDefault(x => x.Id == revenue.RevenueHeadId);
            var ministry = db.Ministries.FirstOrDefault(x => x.Id == revenueHead.MinistryId);
            var agent = db.Agents.FirstOrDefault(x => x.Id == tLogsObj.AgentId);

            var tLogs = new Model.Transaction
            {
                Id = tLogsObj.Id,
                AgentId = tLogsObj.AgentId.Value,
                ClientId = tLogsObj.ClientId.Value,
                TerminalId = tLogsObj.TerminalId.Value,
                TransactionCode = tLogsObj.Code,
                ResidentId = tLogsObj.ResidentId,
                FirstName = tLogsObj.FirstName,
                MiddleName = tLogsObj.MiddleName,
                LastName = tLogsObj.Lastname,
                Address = tLogsObj.Address,
                Email = tLogsObj.Email,
                PhoneNumber = tLogsObj.PhoneNumber,
                DateOfBirth = tLogsObj.DateOfBirth.Value,
                Gender = tLogsObj.Gender,
                RevenueCode = tLogsObj.RevenueCode,
                TerminalCode = terminal.Code,
                RevenueName = revenue != null ? revenue.Name : "",
                AgentName = agent != null ? agent.Name : "",
                Ministry = ministry != null ? ministry.Name : "",
                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                Amount = tLogsObj.Amount.Value,
                PaymentReference = tLogsObj.PaymentReference,
                TransactionDate = tLogsObj.TransactionDate.Value,
                UploadDate = tLogsObj.UploadDate.Value,
                status = tLogsObj.Status.Value
            };
            if (tLogs == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Transaction = (Model.Transaction)tLogs;

            }
            return res;
        }

        internal GetAllTransactionRes GetTransactions(GetTransactionRequest req)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();

            var tLogs = from tlogs in db.TransactionLogs
                        join z in db.Agents on tlogs.AgentId equals z.Id
                        join y in db.RevenueItems on tlogs.RevenueCode equals y.Code
                        join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                        join ministry in db.Ministries on y.MinistryId equals ministry.Id

                        join location in db.TerminalLocations on tlogs.LocationId equals location.Id

                        select new Transaction
                        {
                            Id = tlogs.Id,
                            ClientId = tlogs.ClientId.Value,
                            AgentId = tlogs.AgentId.Value,
                            AgentName = z.Name,
                            LocationName = location.LocationDescription,
                            TerminalId = tlogs.TerminalId.Value,
                            LocationId = tlogs.LocationId,
                            TransactionCode = tlogs.Code,
                            ResidentId = tlogs.ResidentId,
                            FirstName = tlogs.FirstName,
                            MiddleName = tlogs.MiddleName,
                            LastName = tlogs.Lastname,
                            Address = tlogs.Address,
                            Email = tlogs.Email,
                            PhoneNumber = tlogs.PhoneNumber,
                            DateOfBirth = tlogs.DateOfBirth.Value,
                            Gender = tlogs.Gender,
                            RevenueCode = tlogs.RevenueCode,
                            RevenueName = y.Name,
                            Ministry = ministry != null ? ministry.Name : "",
                            RevenueHead = revenueHead != null ? revenueHead.Name : "",
                            Amount = tlogs.Amount.Value,
                            PaymentReference = tlogs.PaymentReference,
                            TransactionDate = tlogs.TransactionDate.Value,
                            UploadDate = tlogs.UploadDate.Value,
                            status = tlogs.Status.Value
                        };

            switch (req.UserType)
            {
                case UserType.Client:
                    tLogs = tLogs.Where(x => x.ClientId == req.UserTypeId);
                    break;
                case UserType.Agent:
                    tLogs = tLogs.Where(x => x.AgentId == req.UserTypeId);
                    break;
                case UserType.Terminal:
                    tLogs = tLogs.Where(x => x.TerminalId == req.UserTypeId);
                    break;
                case UserType.Location:
                    tLogs = tLogs.Where(x => x.LocationId == req.UserTypeId);
                    break;
                default:
                    break;
            }

            if (req.RequireDateFilter && req.StartDate != null && req.EndtDate != null)
            {
                tLogs = tLogs.Where(x => x.TransactionDate >= req.StartDate && x.TransactionDate <= req.EndtDate);
            }

            if (req.RequireLimit && req.Limit!=null)
            {
                tLogs = tLogs.Take(req.Limit ?? 0);
            }

            tLogs = tLogs.OrderByDescending(x => x.TransactionDate);

            res.Transactions = tLogs.ToList();
            return res;
        }

        #region ===Depricated.. Now Replaced by GetAllTransactionRes GetTransactions(GetTransactionRequest req)
            internal GetAllTransactionRes GetAllTransactionsByLocationId(int locationId)
            {
                throw new NotImplementedException();
            }

            internal GetAllTransactionRes GetAllTransactions()
            {
                GetAllTransactionRes res = new GetAllTransactionRes();

                var tLogs = from x in db.TransactionLogs
                            join z in db.Agents on x.AgentId equals z.Id
                            join y in db.RevenueItems on x.RevenueCode equals y.Code
                            join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                            join ministry in db.Ministries on y.MinistryId equals ministry.Id
                            select new Transaction
                            {
                                Id = x.Id,
                                AgentId = x.AgentId.Value,
                                AgentName = z.Name,
                                TerminalId = x.TerminalId.Value,
                                TransactionCode = x.Code,
                                ResidentId = x.ResidentId,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.Lastname,
                                Address = x.Address,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                DateOfBirth = x.DateOfBirth.Value,
                                Gender = x.Gender,
                                RevenueCode = x.RevenueCode,
                                RevenueName = y.Name,
                                Ministry = ministry != null ? ministry.Name : "",
                                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                                Amount = x.Amount.Value,
                                PaymentReference = x.PaymentReference,
                                TransactionDate = x.TransactionDate.Value,
                                UploadDate = x.UploadDate.Value,
                                status = x.Status.Value
                            };
                                
                  res.Transactions = tLogs.ToList().OrderByDescending(x => x.Id);

                return res;
            }

            internal GetAllTransactionRes GetLast1000Transactions()
            {
                GetAllTransactionRes res = new GetAllTransactionRes();

                var tLogs = from x in db.TransactionLogs
                            join z in db.Agents on x.AgentId equals z.Id
                            join y in db.RevenueItems on x.RevenueCode equals y.Code
                            join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                            join ministry in db.Ministries on y.MinistryId equals ministry.Id
                            select new Transaction
                            {
                                Id = x.Id,
                                AgentId = x.AgentId.Value,
                                AgentName = z.Name,
                                TerminalId = x.TerminalId.Value,
                                TransactionCode = x.Code,
                                ResidentId = x.ResidentId,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.Lastname,
                                Address = x.Address,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                DateOfBirth = x.DateOfBirth.Value,
                                Gender = x.Gender,
                                RevenueCode = x.RevenueCode,
                                RevenueName = y.Name,
                                Ministry = ministry != null ? ministry.Name : "",
                                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                                Amount = x.Amount.Value,
                                PaymentReference = x.PaymentReference,
                                TransactionDate = x.TransactionDate.Value,
                                UploadDate = x.UploadDate.Value,
                                status = x.Status.Value
                            };

                res.Transactions = tLogs.OrderByDescending(x => x.TransactionDate).Take(1000).ToList();

                return res;
            }

            internal GetAllTransactionRes GetAllTransactionsByClientId(int clientId)
            {
                GetAllTransactionRes res = new GetAllTransactionRes();
                var tLogs = from x in db.TransactionLogs
                            join z in db.Agents on x.AgentId equals z.Id
                            join y in db.RevenueItems on x.RevenueCode equals y.Code
                            join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                            join ministry in db.Ministries on y.MinistryId equals ministry.Id

                            where x.ClientId == clientId
                            select new Transaction
                            {
                                Id = x.Id,
                                AgentId = x.AgentId.Value,
                                AgentName = z.Name,
                                TerminalId = x.TerminalId.Value,
                                TransactionCode = x.Code,
                                ResidentId = x.ResidentId,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.Lastname,
                                Address = x.Address,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                DateOfBirth = x.DateOfBirth.Value,
                                Gender = x.Gender,
                                RevenueCode = x.RevenueCode,
                                RevenueName = y.Name,
                                Ministry = ministry != null ? ministry.Name : "",
                                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                                Amount = x.Amount.Value,
                                PaymentReference = x.PaymentReference,
                                TransactionDate = x.TransactionDate.Value,
                                UploadDate = x.UploadDate.Value,
                                status = x.Status.Value
                            };

                res.Transactions = tLogs.ToList().OrderByDescending(x => x.TransactionDate);

                return res;
            }

            internal GetAllTransactionRes GetLast1000TransactionsByClientId(int clientId)
            {
                GetAllTransactionRes res = new GetAllTransactionRes();
                var tLogs = from x in db.TransactionLogs
                            join z in db.Agents on x.AgentId equals z.Id
                            join y in db.RevenueItems on x.RevenueCode equals y.Code
                            join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                            join ministry in db.Ministries on y.MinistryId equals ministry.Id

                            where x.ClientId == clientId
                            select new Transaction
                            {
                                Id = x.Id,
                                AgentId = x.AgentId.Value,
                                AgentName = z.Name,
                                TerminalId = x.TerminalId.Value,
                                TransactionCode = x.Code,
                                ResidentId = x.ResidentId,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.Lastname,
                                Address = x.Address,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                DateOfBirth = x.DateOfBirth.Value,
                                Gender = x.Gender,
                                RevenueCode = x.RevenueCode,
                                RevenueName = y.Name,
                                Ministry = ministry != null ? ministry.Name : "",
                                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                                Amount = x.Amount.Value,
                                PaymentReference = x.PaymentReference,
                                TransactionDate = x.TransactionDate.Value,
                                UploadDate = x.UploadDate.Value,
                                status = x.Status.Value
                            };

                res.Transactions = tLogs.OrderByDescending(x => x.TransactionDate).Take(1000).ToList();

                return res;
            }

            internal GetAllTransactionRes GetAllTransactionsByAgentId(int agentId)
            {
                GetAllTransactionRes res = new GetAllTransactionRes();
                var tLogs = from x in db.TransactionLogs
                            join z in db.Agents on x.AgentId equals z.Id
                            join y in db.RevenueItems on x.RevenueCode equals y.Code
                            join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                            join ministry in db.Ministries on y.MinistryId equals ministry.Id

                            where x.AgentId  == agentId
                            select new Transaction
                            {
                                Id = x.Id,
                                AgentId = x.AgentId.Value,
                                AgentName = z.Name,
                                TerminalId = x.TerminalId.Value,
                                TransactionCode = x.Code,
                                ResidentId = x.ResidentId,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.Lastname,
                                Address = x.Address,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                DateOfBirth = x.DateOfBirth.Value,
                                Gender = x.Gender,
                                RevenueCode = x.RevenueCode,
                                RevenueName = y.Name,
                                Ministry = ministry != null ? ministry.Name : "",
                                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                                Amount = x.Amount.Value,
                                PaymentReference = x.PaymentReference,
                                TransactionDate = x.TransactionDate.Value,
                                UploadDate = x.UploadDate.Value,
                                status = x.Status.Value
                            };

                res.Transactions = tLogs.ToList().OrderByDescending(x => x.Id); ;

                return res;
            }

            internal GetAllTransactionRes GetLast1000TransactionsByAgentId(int agentId)
            {
                GetAllTransactionRes res = new GetAllTransactionRes();
                var tLogs = from x in db.TransactionLogs
                            join z in db.Agents on x.AgentId equals z.Id
                            join y in db.RevenueItems on x.RevenueCode equals y.Code
                            join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                            join ministry in db.Ministries on y.MinistryId equals ministry.Id

                            where x.AgentId  == agentId
                            select new Transaction
                            {
                                Id = x.Id,
                                AgentId = x.AgentId.Value,
                                AgentName = z.Name,
                                TerminalId = x.TerminalId.Value,
                                TransactionCode = x.Code,
                                ResidentId = x.ResidentId,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.Lastname,
                                Address = x.Address,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                DateOfBirth = x.DateOfBirth.Value,
                                Gender = x.Gender,
                                RevenueCode = x.RevenueCode,
                                RevenueName = y.Name,
                                Ministry = ministry != null ? ministry.Name : "",
                                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                                Amount = x.Amount.Value,
                                PaymentReference = x.PaymentReference,
                                TransactionDate = x.TransactionDate.Value,
                                UploadDate = x.UploadDate.Value,
                                status = x.Status.Value
                            };

                res.Transactions = tLogs.OrderByDescending(x => x.TransactionDate).Take(1000).ToList();

                return res;
            }

            internal GetAllTransactionRes GetAllTransactionsByTerminalId(int terminalId)
            {
                GetAllTransactionRes res = new GetAllTransactionRes();
                var tLogs = from x in db.TransactionLogs
                            join z in db.Agents on x.AgentId equals z.Id
                            join y in db.RevenueItems on x.RevenueCode equals y.Code
                            join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                            join ministry in db.Ministries on y.MinistryId equals ministry.Id

                            where x.TerminalId == terminalId
                            select new Transaction
                            {
                                Id = x.Id,
                                AgentId = x.AgentId.Value,
                                AgentName = z.Name,
                                TerminalId = x.TerminalId.Value,
                                TransactionCode = x.Code,
                                ResidentId = x.ResidentId,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.Lastname,
                                Address = x.Address,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                DateOfBirth = x.DateOfBirth.Value,
                                Gender = x.Gender,
                                RevenueCode = x.RevenueCode,
                                RevenueName = y.Name,
                                Ministry = ministry != null ? ministry.Name : "",
                                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                                Amount = x.Amount.Value,
                                PaymentReference = x.PaymentReference,
                                TransactionDate = x.TransactionDate.Value,
                                UploadDate = x.UploadDate.Value,
                                status = x.Status.Value
                            };

                var test = tLogs.ToList().OrderByDescending(x => x.Id); ;
                res.Transactions = tLogs;

                return res;
            }

            internal GetAllTransactionRes GetLast1000TransactionsByTerminalId(int terminalId)
            {
                GetAllTransactionRes res = new GetAllTransactionRes();
                var tLogs = from x in db.TransactionLogs
                            join z in db.Agents on x.AgentId equals z.Id
                            join y in db.RevenueItems on x.RevenueCode equals y.Code
                            join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                            join ministry in db.Ministries on y.MinistryId equals ministry.Id

                            where x.TerminalId == terminalId
                            select new Transaction
                            {
                                Id = x.Id,
                                AgentId = x.AgentId.Value,
                                AgentName = z.Name,
                                TerminalId = x.TerminalId.Value,
                                TransactionCode = x.Code,
                                ResidentId = x.ResidentId,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.Lastname,
                                Address = x.Address,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                DateOfBirth = x.DateOfBirth.Value,
                                Gender = x.Gender,
                                RevenueCode = x.RevenueCode,
                                RevenueName = y.Name,
                                Ministry = ministry != null ? ministry.Name : "",
                                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                                Amount = x.Amount.Value,
                                PaymentReference = x.PaymentReference,
                                TransactionDate = x.TransactionDate.Value,
                                UploadDate = x.UploadDate.Value,
                                status = x.Status.Value
                            };

                res.Transactions = tLogs.OrderByDescending(x => x.TransactionDate).Take(1000).ToList();
                res.Transactions = tLogs;

                return res;
            }

            internal GetAllTransactionRes GetAllTransactionsByResidentId(string residentId)
            {
                GetAllTransactionRes res = new GetAllTransactionRes();
                var tLogs = from x in db.TransactionLogs
                            join z in db.Agents on x.AgentId equals z.Id
                            join y in db.RevenueItems on x.RevenueCode equals y.Code
                            join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                            join ministry in db.Ministries on y.MinistryId equals ministry.Id

                            where x.ResidentId == residentId
                            select new Transaction
                            {
                                Id = x.Id,
                                AgentId = x.AgentId.Value,
                                AgentName = z.Name,
                                TerminalId = x.TerminalId.Value,
                                TransactionCode = x.Code,
                                ResidentId = x.ResidentId,
                                FirstName = x.FirstName,
                                MiddleName = x.MiddleName,
                                LastName = x.Lastname,
                                Address = x.Address,
                                Email = x.Email,
                                PhoneNumber = x.PhoneNumber,
                                DateOfBirth = x.DateOfBirth.Value,
                                Gender = x.Gender,
                                RevenueCode = x.RevenueCode,
                                RevenueName = y.Name,
                                Ministry = ministry != null ? ministry.Name : "",
                                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                                Amount = x.Amount.Value,
                                PaymentReference = x.PaymentReference,
                                TransactionDate = x.TransactionDate.Value,
                                UploadDate = x.UploadDate.Value,
                                status = x.Status.Value
                            };

                var test = tLogs.ToList().OrderByDescending(x => x.Id); ;
                res.Transactions = tLogs;

                return res;
            }
            #endregion
        #endregion

        #region Upload Error
        internal Model.FindErrorTransactionRes FindErrorTransaction(int id)
        {
            Model.FindErrorTransactionRes res = new Model.FindErrorTransactionRes();
            var tLogsObj = db.TransactionUploadErrors.FirstOrDefault(x => x.Id == id);
            var terminal = db.Terminals.FirstOrDefault(x => x.Id == tLogsObj.TerminalId);
            var revenue = db.RevenueItems.FirstOrDefault(x => x.Code == tLogsObj.RevenueCode.Trim());
            var agent = db.Agents.FirstOrDefault(x => x.Id == tLogsObj.AgentId);
            var revenueHead = db.RevenueHeads.FirstOrDefault(x => x.Id == revenue.RevenueHeadId);
            var ministry = db.Ministries.FirstOrDefault(x => x.Id == revenueHead.MinistryId);

            var tLogs = new ErrorTransaction();
            tLogs.Id = tLogsObj.Id;
            tLogs.AgentId = tLogsObj.AgentId.Value;
            tLogs.ClientId = tLogsObj.ClientId.Value;
            tLogs.TerminalId = tLogsObj.TerminalId.Value;
            tLogs.TransactionCode = tLogsObj.Code;
            tLogs.ResidentId = tLogsObj.ResidentId;
            tLogs.FirstName = tLogsObj.FirstName;
            tLogs.MiddleName = tLogsObj.MiddleName;
            tLogs.LastName = tLogsObj.Lastname;
            tLogs.Address = tLogsObj.Address;
            tLogs.Email = tLogsObj.Email;
            tLogs.PhoneNumber = tLogsObj.PhoneNumber;
            tLogs.DateOfBirth = tLogsObj.DateOfBirth.Value;
            tLogs.Gender = tLogsObj.Gender;
            tLogs.RevenueCode = tLogsObj.RevenueCode;
            tLogs.TerminalCode = terminal.Code;
            tLogs.RevenueName = revenue != null ? revenue.Name : "";
            tLogs.AgentName = agent != null ? agent.Name : "";
            tLogs.Ministry = ministry != null ? ministry.Name : "";
            tLogs.RevenueHead = revenueHead != null ? revenueHead.Name : "";
            tLogs.Amount = tLogsObj.Amount.Value;
            tLogs.PaymentReference = tLogsObj.PaymentReference;
            tLogs.TransactionDate = tLogsObj.TransactionDate.Value;
            tLogs.UploadDate = tLogsObj.UploadDate.Value;
            tLogs.status = tLogsObj.Status.Value;

            tLogs.UploadError = tLogsObj.UploadError;

            tLogs.ResolutionDate = tLogsObj.ResolutionDate ?? null;
            tLogs.ResolutionError = tLogsObj.ResolutionError;
            tLogs.ResolutionStatus = tLogsObj.ResolutionStatus != null ? tLogsObj.ResolutionStatus.Value : 0;

            if (tLogs == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Transaction = tLogs;

            }
            return res;
        }

        internal FindErrorTransactionRes FindErrorTransactionByCode(string transactionCode)
        {
            FindErrorTransactionRes res = new FindErrorTransactionRes();
            var tLogsObj = db.TransactionUploadErrors.FirstOrDefault(x => x.Code == transactionCode);
            var terminal = db.Terminals.FirstOrDefault(x => x.Id == tLogsObj.TerminalId);
            var revenue = db.RevenueItems.FirstOrDefault(x => x.Code == tLogsObj.RevenueCode.Trim());
            var revenueHead = db.RevenueHeads.FirstOrDefault(x => x.Id == revenue.RevenueHeadId);
            var ministry = db.Ministries.FirstOrDefault(x => x.Id == revenueHead.MinistryId);
            var agent = db.Agents.FirstOrDefault(x => x.Id == tLogsObj.AgentId);

            var tLogs = new ErrorTransaction
            {
                Id = tLogsObj.Id,
                AgentId = tLogsObj.AgentId.Value,
                ClientId = tLogsObj.ClientId.Value,
                TerminalId = tLogsObj.TerminalId.Value,
                TransactionCode = tLogsObj.Code,
                ResidentId = tLogsObj.ResidentId,
                FirstName = tLogsObj.FirstName,
                MiddleName = tLogsObj.MiddleName,
                LastName = tLogsObj.Lastname,
                Address = tLogsObj.Address,
                Email = tLogsObj.Email,
                PhoneNumber = tLogsObj.PhoneNumber,
                DateOfBirth = tLogsObj.DateOfBirth.Value,
                Gender = tLogsObj.Gender,
                RevenueCode = tLogsObj.RevenueCode,
                TerminalCode = terminal.Code,
                RevenueName = revenue != null ? revenue.Name : "",
                Ministry = ministry != null ? ministry.Name : "",
                RevenueHead = revenueHead != null ? revenueHead.Name : "",
                AgentName = agent != null ? agent.Name : "",
                Amount = tLogsObj.Amount.Value,
                PaymentReference = tLogsObj.PaymentReference,
                TransactionDate = tLogsObj.TransactionDate.Value,
                UploadDate = tLogsObj.UploadDate.Value,
                status = tLogsObj.Status.Value,

                UploadError = tLogsObj.UploadError,
                ResolutionDate = tLogsObj.ResolutionDate.Value,
                ResolutionError = tLogsObj.ResolutionError,
                ResolutionStatus = tLogsObj.ResolutionStatus.Value
            };
            if (tLogs == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Transaction = (ErrorTransaction)tLogs;

            }
            return res;
        }

        internal GetAllErrorTransactionRes GetErrorTransactions(GetTransactionRequest req)
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();

            var tLogs = from x in db.TransactionUploadErrors
                        join z in db.Agents on x.AgentId equals z.Id
                        join y in db.RevenueItems on x.RevenueCode equals y.Code
                        join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                        join ministry in db.Ministries on y.MinistryId equals ministry.Id

                        select new ErrorTransaction
                        {
                            Id = x.Id,
                            AgentId = x.AgentId ?? null,
                            ClientId = x.ClientId ?? null,
                            AgentName = z.Name,
                            TerminalId = x.TerminalId ?? null,
                            TransactionCode = x.Code,
                            ResidentId = x.ResidentId,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.Lastname,
                            Address = x.Address,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            DateOfBirth = x.DateOfBirth,
                            Gender = x.Gender,
                            RevenueCode = x.RevenueCode,
                            RevenueName = y.Name,
                            Ministry = ministry != null ? ministry.Name : "",
                            RevenueHead = revenueHead != null ? revenueHead.Name : "",
                            Amount = x.Amount ?? null,
                            PaymentReference = x.PaymentReference,
                            TransactionDate = x.TransactionDate,
                            UploadDate = x.UploadDate,
                            status = x.Status ?? null,

                            UploadError = x.UploadError,
                            ResolutionDate = x.ResolutionDate,
                            ResolutionError = x.ResolutionError,
                            ResolutionStatus = x.ResolutionStatus ?? null
                        };

            switch (req.UserType)
            {
                case 1:
                    tLogs = tLogs.Where(x => x.ClientId == req.UserTypeId);
                    break;
                case 2:
                    tLogs = tLogs.Where(x => x.AgentId == req.UserTypeId);
                    break;
                case 3:
                    tLogs = tLogs.Where(x => x.TerminalId == req.UserTypeId);
                    break;
                default:
                    break;
            }

            if (req.RequireDateFilter)
            {
                tLogs = tLogs.Where(x => x.TransactionDate >= req.StartDate && x.TransactionDate <= req.EndtDate);
            }

            if (req.RequireLimit && req.Limit != null)
            {
                tLogs = tLogs.Take(req.Limit ?? 0);
            }

            tLogs = tLogs.OrderByDescending(x => x.TransactionDate);
            res.Transactions = tLogs.ToList();
            return res;
        }

        internal GetAllErrorTransactionRes GetAllErrorTransactions()
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();

            var tLogs = from x in db.TransactionUploadErrors
                        join z in db.Agents on x.AgentId equals z.Id
                        join y in db.RevenueItems on x.RevenueCode equals y.Code
                        join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                        join ministry in db.Ministries on y.MinistryId equals ministry.Id
                        select new ErrorTransaction
                        {
                            Id = x.Id,
                            AgentId = x.AgentId.Value,
                            AgentName = z.Name,
                            TerminalId = x.TerminalId.Value,
                            TransactionCode = x.Code,
                            ResidentId = x.ResidentId,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.Lastname,
                            Address = x.Address,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            DateOfBirth = x.DateOfBirth.Value,
                            Gender = x.Gender,
                            RevenueCode = x.RevenueCode,
                            RevenueName = y.Name,
                            Ministry = ministry != null ? ministry.Name : "",
                            RevenueHead = revenueHead != null ? revenueHead.Name : "",
                            Amount = x.Amount.Value,
                            PaymentReference = x.PaymentReference,
                            TransactionDate = x.TransactionDate.Value,
                            UploadDate = x.UploadDate.Value,
                            status = x.Status.Value,

                            UploadError = x.UploadError,
                            ResolutionDate = x.ResolutionDate.Value,
                            ResolutionError = x.ResolutionError,
                            ResolutionStatus = x.ResolutionStatus.Value
                        };

            res.Transactions = tLogs.ToList();

            return res;
        }

        internal GetAllErrorTransactionRes GetAllErrorTransactionsByClientId(int clientId)
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();
            var tLogs = from x in db.TransactionUploadErrors
                        join z in db.Agents on x.AgentId equals z.Id
                        join y in db.RevenueItems on x.RevenueCode equals y.Code
                        join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                        join ministry in db.Ministries on y.MinistryId equals ministry.Id

                        where x.ClientId == clientId
                        select new ErrorTransaction
                        {
                            Id = x.Id,
                            AgentId = x.AgentId.Value,
                            AgentName = z.Name,
                            TerminalId = x.TerminalId.Value,
                            TransactionCode = x.Code,
                            ResidentId = x.ResidentId,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.Lastname,
                            Address = x.Address,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            DateOfBirth = x.DateOfBirth.Value,
                            Gender = x.Gender,
                            RevenueCode = x.RevenueCode,
                            RevenueName = y.Name,
                            Ministry = ministry != null ? ministry.Name : "",
                            RevenueHead = revenueHead != null ? revenueHead.Name : "",
                            Amount = x.Amount.Value,
                            PaymentReference = x.PaymentReference,
                            TransactionDate = x.TransactionDate.Value,
                            UploadDate = x.UploadDate.Value,
                            status = x.Status.Value,

                            UploadError = x.UploadError,
                            ResolutionDate = x.ResolutionDate.Value,
                            ResolutionError = x.ResolutionError,
                            ResolutionStatus = x.ResolutionStatus.Value
                        };

            var xxx = tLogs.ToList();
            res.Transactions = tLogs.ToList().OrderByDescending(x => x.TransactionDate);

            return res;
        }

        internal GetAllErrorTransactionRes GetAllErrorTransactionsByAgentId(int agentId)
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();
            var tLogs = from x in db.TransactionUploadErrors
                        join z in db.Agents on x.AgentId equals z.Id
                        join y in db.RevenueItems on x.RevenueCode equals y.Code
                        join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                        join ministry in db.Ministries on y.MinistryId equals ministry.Id

                        where x.AgentId == agentId
                        select new ErrorTransaction
                        {
                            Id = x.Id,
                            AgentId = x.AgentId.Value,
                            AgentName = z.Name,
                            TerminalId = x.TerminalId.Value,
                            TransactionCode = x.Code,
                            ResidentId = x.ResidentId,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.Lastname,
                            Address = x.Address,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            DateOfBirth = x.DateOfBirth.Value,
                            Gender = x.Gender,
                            RevenueCode = x.RevenueCode,
                            RevenueName = y.Name,
                            Ministry = ministry != null ? ministry.Name : "",
                            RevenueHead = revenueHead != null ? revenueHead.Name : "",
                            Amount = x.Amount.Value,
                            PaymentReference = x.PaymentReference,
                            TransactionDate = x.TransactionDate.Value,
                            UploadDate = x.UploadDate.Value,
                            status = x.Status.Value,

                            UploadError = x.UploadError,
                            ResolutionDate = x.ResolutionDate.Value,
                            ResolutionError = x.ResolutionError,
                            ResolutionStatus = x.ResolutionStatus.Value
                        };

            res.Transactions = tLogs.ToList().OrderByDescending(x => x.Id); ;

            return res;
        }

        internal GetAllErrorTransactionRes GetAllErrorTransactionsByTerminalId(int terminalId)
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();
            var tLogs = from x in db.TransactionUploadErrors
                        join z in db.Agents on x.AgentId equals z.Id
                        join y in db.RevenueItems on x.RevenueCode equals y.Code
                        join revenueHead in db.RevenueHeads on y.RevenueHeadId equals revenueHead.Id
                        join ministry in db.Ministries on y.MinistryId equals ministry.Id

                        where x.TerminalId == terminalId
                        select new Model.ErrorTransaction
                        {
                            Id = x.Id,
                            AgentId = x.AgentId.Value,
                            AgentName = z.Name,
                            TerminalId = x.TerminalId.Value,
                            TransactionCode = x.Code,
                            ResidentId = x.ResidentId,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.Lastname,
                            Address = x.Address,
                            Email = x.Email,
                            PhoneNumber = x.PhoneNumber,
                            DateOfBirth = x.DateOfBirth.Value,
                            Gender = x.Gender,
                            RevenueCode = x.RevenueCode,
                            RevenueName = y.Name,
                            Ministry = ministry != null ? ministry.Name : "",
                            RevenueHead = revenueHead != null ? revenueHead.Name : "",
                            Amount = x.Amount.Value,
                            PaymentReference = x.PaymentReference,
                            TransactionDate = x.TransactionDate.Value,
                            UploadDate = x.UploadDate.Value,
                            status = x.Status.Value,

                            UploadError = x.UploadError,
                            ResolutionDate = x.ResolutionDate.Value,
                            ResolutionError = x.ResolutionError,
                            ResolutionStatus = x.ResolutionStatus.Value
                        };

            var test = tLogs.ToList().OrderByDescending(x => x.Id); ;
            res.Transactions = tLogs;

            return res;
        }


        #endregion

        #region Revenue

        internal bool CreateRevenue(Model.Revenue req, out int? revenueId, out string msg){
            bool result = true;
            msg = string.Empty;            
            revenueId = null;

            ChamsICSLib.Data.Revenue revenue = new ChamsICSLib.Data.Revenue(){
              Code = req.RevenueCode,
              ClientId = req.ClientId,
              Name = req.Name,
              MDA = req.MDA,
              Status = req.Status,
              Amount = req.Amount
            };

            db.Revenues.Add(revenue);
            db.Entry(revenue).State = EntityState.Added;
            db.SaveChanges();

            revenueId = revenue.Id;

            return result;

        }

        internal bool UpdateRevenue(Model.Revenue req, out string msg)
        {
            msg = string.Empty;

            var revenue = db.Revenues.FirstOrDefault(x => x.Id == req.RevenueId);
            if (revenue != null)
            {
                revenue.Code = req.RevenueCode;
                revenue.ClientId = req.ClientId;
                revenue.Name = req.Name;
                revenue.MDA = req.MDA;
                revenue.Status = req.Status;
                revenue.Amount = req.Amount;
                revenue.Status = req.Status;

                db.Revenues.Attach(revenue);
                db.Entry(revenue).State = EntityState.Modified;
                int result = db.SaveChanges();

                return result > 0;
            }
            else { return false; }
        }

        internal FindRevenueRes GetRevenue(int Id)
        {
            FindRevenueRes res = new FindRevenueRes();
            var revenueObj = db.Revenues.FirstOrDefault(x => x.Id == Id);

            var revenue = new Model.Revenue
            {
                RevenueId = revenueObj.Id,
                ClientId = revenueObj.ClientId.Value,
                RevenueCode = revenueObj.Code,
                Name = revenueObj.Name,
                Amount = revenueObj.Amount.Value,
                MDA = revenueObj.MDA,
                Status = revenueObj.Status.Value
            };

            if (revenue == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Revenue = (Model.Revenue)revenue;

            }
            return res;
        }

        internal GetAllRevenueRes GetAllRevenue()
        {
            GetAllRevenueRes res = new GetAllRevenueRes();
            var revenue = db.Revenues.Select(revenueObj => new Model.Revenue
            {
                RevenueId = revenueObj.Id,
                ClientId = revenueObj.ClientId.Value,
                RevenueCode = revenueObj.Code,
                Name = revenueObj.Name,
                Amount = revenueObj.Amount.Value,
                MDA = revenueObj.MDA,
                Status = revenueObj.Status.Value
            });


            res.Revenues = revenue;

            return res;
        }

        internal GetAllRevenueRes GetAllRevenueByClientId(int id)
        {
            GetAllRevenueRes res = new GetAllRevenueRes();
            var revenue = db.Revenues.Where(x=> x.ClientId==id).Select(revenueObj => new Model.Revenue
            {
                RevenueId = revenueObj.Id,
                ClientId = revenueObj.ClientId.Value,
                RevenueCode = revenueObj.Code,
                Name = revenueObj.Name,
                Amount = revenueObj.Amount.Value,
                MDA = revenueObj.MDA,
                Status = revenueObj.Status.Value
            });


            res.Revenues = revenue;
            return res;
        }

        internal List<Model.Ministry> GetAllMinistry()
        {
            var ministry = db.Ministries.Select(x => new Model.Ministry
            {
                Id = x.Id,
                ClientId = x.ClientId.Value,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status.Value
            });
            return ministry.ToList();
        }

        internal List<Model.Ministry> GetAllMinistryByClientId(int id)
        {
            var ministry = db.Ministries.Where(x => x.ClientId == id).Select(x => new Model.Ministry
            {
                Id = x.Id,
                ClientId = x.ClientId.Value,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status.Value
            });
            return ministry.ToList();
        }

        internal List<Model.RevenueHead> GetAllRevenueHead()
        {
            var revenueHeads = from x in db.RevenueHeads 
            select  new Model.RevenueHead {
                Id = x.Id,
                ClientId = x.ClientId.Value,
                MinistryId = x.MinistryId.Value,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status.Value
            };

            return revenueHeads.ToList();
        }

        internal List<Model.RevenueHead> GetAllRevenueHeadByClientId(int id)
        {
            var revenueHeads = from x in db.RevenueHeads join y in db.Ministries
            on x.MinistryId equals y.Id
            where  x.ClientId == id            
            select  new Model.RevenueHead {
                Id = x.Id,
                ClientId = x.ClientId.Value,
                MinistryId = x.MinistryId.Value,
                Ministry = y.Name,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status.Value
            };

            return revenueHeads.ToList();
        }

        internal List<Model.RevenueHead> GetAllRevenueHeadByMinistryId(int id)
        {
            var revenueHeads = from x in db.RevenueHeads
                               join y in db.Ministries on x.MinistryId equals y.Id
                               where x.MinistryId == id
                               select new Model.RevenueHead
                               {
                                   Id = x.Id,
                                   ClientId = x.ClientId.Value,
                                   MinistryId = x.MinistryId.Value,
                                   Ministry = y.Name,
                                   Code = x.Code,
                                   Name = x.Name,
                                   Status = x.Status.Value
                               };

            return revenueHeads.ToList();
        }

        internal List<Model.RevenueCategory> GetAllRevenueCategoryByClientId(int id)
        {
            var revenueCategory = from x in db.RevenueCategories
                               join y in db.RevenueHeads on x.RevenueHeadId equals y.Id
                               join z in db.Ministries on y.MinistryId equals z.Id
                               where x.ClientId == id
                               select new Model.RevenueCategory
                               {
                                   Id = x.Id,
                                   ClientId = x.ClientId.Value,
                                    MinistryId = z.Id,
                                    Ministry = z.Name,
                                   RevenueHeadId = x.RevenueHeadId.Value,
                                   RevenueHead = y.Name,
                                   Code = x.Code,
                                   Name = x.Name,
                                   Status = x.Status.Value
                               };

            return revenueCategory.ToList();
        }

        internal List<Model.RevenueCategory> GetAllRevenueCategoryByRevenueHeadId(int id)
        {
            var revenueCategory = from x in db.RevenueCategories
                                  join y in db.RevenueHeads on x.RevenueHeadId equals y.Id
                                  join z in db.Ministries on y.MinistryId equals z.Id
                                  where x.RevenueHeadId == id
                                  select new Model.RevenueCategory
                                  {
                                      Id = x.Id,
                                      ClientId = x.ClientId.Value,
                                      MinistryId = z.Id,
                                      Ministry = z.Name,
                                      RevenueHeadId = x.RevenueHeadId.Value,
                                      RevenueHead = y.Name,
                                      Code = x.Code,
                                      Name = x.Name,
                                      Status = x.Status.Value
                                  };

            return revenueCategory.ToList();
        }

        internal List<Model.RevenueItem> GetAllRevenueItemByClientId(int id)
        {
            var revenueItems = from x in db.RevenueItems
                                  join y in db.RevenueCategories on x.CategoryId equals y.Id
                                  join z in db.RevenueHeads on x.RevenueHeadId equals z.Id
                                  join a in db.Ministries on x.MinistryId equals a.Id

                                  where x.ClientId == id
                                  orderby a.Id, z.Id,y.Id
                                  select new Model.RevenueItem
                                  {
                                      Id = x.Id,
                                      ClientId = x.ClientId.Value,
                                      MinistryId = x.MinistryId.Value,
                                      Ministry = a.Name,
                                      CategoryId = x.CategoryId.Value,
                                      Category = y.Name,
                                      RevenueHeadId = x.RevenueHeadId.Value,
                                      RevenueHead = z.Name,
                                      Code = x.Code,
                                      Name = x.Name,
                                       Amount = x.Amount.Value,
                                      Status = x.Status.Value
                                  };

            return revenueItems.ToList();
        }

        internal List<Model.RevenueItem> GetAllRevenueItemByMinistryId(int id)
        {
            var revenueItems = from x in db.RevenueItems
                               join y in db.RevenueCategories on x.CategoryId equals y.Id
                               join z in db.RevenueHeads on x.RevenueHeadId equals z.Id
                               join a in db.Ministries on x.MinistryId equals a.Id

                               where x.MinistryId == id
                               select new Model.RevenueItem
                               {
                                   Id = x.Id,
                                   ClientId = x.ClientId.Value,
                                   MinistryId = x.MinistryId.Value,
                                   Ministry = a.Name,
                                   CategoryId = x.CategoryId.Value,
                                   Category = y.Name,
                                   RevenueHeadId = x.RevenueHeadId.Value,
                                   RevenueHead = z.Name,
                                   Code = x.Code,
                                   Name = x.Name,
                                   Amount = x.Amount.Value,
                                   Status = x.Status.Value
                               };

            return revenueItems.ToList();
        }

        internal List<Model.RevenueItem> GetAllRevenueItemByRevenueHeadId(int id)
        {
            var revenueItems = from x in db.RevenueItems
                               join y in db.RevenueCategories on x.CategoryId equals y.Id
                               join z in db.RevenueHeads on x.RevenueHeadId equals z.Id
                               join a in db.Ministries on x.MinistryId equals a.Id

                               where x.RevenueHeadId == id
                               select new Model.RevenueItem
                               {
                                   Id = x.Id,
                                   ClientId = x.ClientId.Value,
                                   MinistryId = x.MinistryId.Value,
                                   Ministry = a.Name,
                                   CategoryId = x.CategoryId.Value,
                                   Category = y.Name,
                                   RevenueHeadId = x.RevenueHeadId.Value,
                                   RevenueHead = z.Name,
                                   Code = x.Code,
                                   Name = x.Name,
                                   Amount = x.Amount.Value,
                                   Status = x.Status.Value
                               };

            return revenueItems.ToList();
        }

        internal List<Model.RevenueItem> GetAllRevenueItemByCategoryId(int id)
        {
            var revenueItems = from x in db.RevenueItems
                               join y in db.RevenueCategories on x.CategoryId equals y.Id
                               join z in db.RevenueHeads on x.RevenueHeadId equals z.Id
                               join a in db.Ministries on x.MinistryId equals a.Id

                               where x.CategoryId == id
                               select new Model.RevenueItem
                               {
                                   Id = x.Id,
                                   ClientId = x.ClientId.Value,
                                   MinistryId = x.MinistryId.Value,
                                   Ministry = a.Name,
                                   CategoryId = x.CategoryId.Value,
                                   Category = y.Name,
                                   RevenueHeadId = x.RevenueHeadId.Value,
                                   RevenueHead = z.Name,
                                   Code = x.Code,
                                   Name = x.Name,
                                   Amount = x.Amount.Value,
                                   Status = x.Status.Value
                               };

            return revenueItems.ToList();
        }

        #endregion

        #region Identity
        internal bool CreateIdentity(Model.Identity req, out int? identityId, out string msg)
        {
            msg = string.Empty;
            identityId = null;

            ChamsICSLib.Data.IdentityService identity = new ChamsICSLib.Data.IdentityService()
            {
                URL = req.URL,
                ClientId = req.ClientId,
                Username = req.UserName,
                Password = req.Password,
                Status = req.Status,
            };

            db.IdentityServices.Add(identity);
            int result = db.SaveChanges();

            identityId = identity.Id;

            try
            {
                if (req.AuditTrailData.userId != null)
                {
                    LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.CREATE, "Identity", "IdentityId:" + identityId, "");
                }
            }
            catch (Exception ex)
            {
                //Whatever Happens, Log it and move on...
                Logger.logToFile(ex, ErrorLogPath);
            }
            return result > 0;

        }

        internal bool UpdateIdentity(Model.Identity req, out string msg)
        {
            msg = string.Empty;

            var identity = db.IdentityServices.FirstOrDefault(x => x.Id == req.IdentityId);
            if (identity != null)
            {
                identity.URL = req.URL;
                identity.ClientId = req.ClientId;
                identity.Username = req.UserName;
                identity.Password = req.Password;
                identity.Status = req.Status;
                db.IdentityServices.Attach(identity);
                db.Entry(identity).State = EntityState.Modified;

                int result = db.SaveChanges();
                //Log Audit Trail
                LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.EDIT, "Identity", "IdentityId:" + identity.Id, "");

                return result > 0;
            }
            else { return false; }
        }

        internal FindIdentityRes GetIdentity(int Id)
        {
            FindIdentityRes res = new FindIdentityRes();
            var req = db.IdentityServices.FirstOrDefault(x => x.Id == Id);

            var identity = new Model.Identity
            {
                IdentityId = req.Id,
                URL = req.URL,
                ClientId = req.ClientId.Value,
                UserName = req.Username,
                Password = req.Password,
                Status = req.Status.Value
            };

            if (identity == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Identity = (Model.Identity)identity;

            }
            return res;
        }

        internal GetAllIdentityRes GetAllIdentity()
        {
            GetAllIdentityRes res = new GetAllIdentityRes();
            var identities = db.IdentityServices.Select(req => new Model.Identity
            {
                IdentityId = req.Id,
                URL = req.URL,
                ClientId = req.ClientId.Value,
                UserName = req.Username,
                Password = req.Password,
                Status = req.Status.Value
            });


            res.Identities = identities;

            return res;
        }

        internal GetAllIdentityRes GetAllIdentityByClientId(int clientId)
        {
            GetAllIdentityRes res = new GetAllIdentityRes();
            var identities = db.IdentityServices.Where(x => x.ClientId == clientId).Select(req => new Model.Identity
            {
                IdentityId = req.Id,
                URL = req.URL,
                ClientId = req.ClientId.Value,
                UserName = req.Username,
                Password = req.Password,
                Status = req.Status.Value,
            });


            res.Identities = identities;
            return res;
        }
        #endregion

        #region User
        internal bool UpdateUser(Model.User req, out string msg)
        {
            msg = string.Empty;
            var user = db.Users.FirstOrDefault(x => x.Id == req.UserId);
            if (user != null)
            {
                user.Mobile = req.Mobile;
                user.PasswordStatus = req.PasswordStatus;
                user.Status = req.Status;
                db.Users.Attach(user);
                db.Entry(user).State = EntityState.Modified;

                int result = db.SaveChanges();
                try
                {
                    if (req.AuditTrailData.userId != null)
                    {
                        LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.EDIT, "User", "UserId:" + req.UserId, "");
                    }
                }
                catch (Exception ex)
                {
                    //Whatever Happens, Log it and move on...
                    Logger.logToFile(ex, ErrorLogPath);
                }
                return result > 0;
            }
            else { return false; }
        }

        internal bool CreateUser(Model.User req, out int? UserId)
        {
            bool result = false;
            UserId = null;

            System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
            Random r = new CryptoRandom();
            string passcode = req.Email.Split(new char[] { '@' })[0] + DateTime.Now.Year.ToString();
            string password = Utils.GetMd5Hash(md5Hash, passcode);

            if (req.ClientId == 0)
            {
                req.ClientId = null;
            }

            ChamsICSLib.Data.User user = new ChamsICSLib.Data.User();
            user.Email = req.Email;
            user.Mobile = req.Mobile;
            user.Password = password;
            user.RoleId = req.RoleId;
            user.UserTypeParentId = req.UserTypeParentId;
            user.ClientId = req.ClientId;
            user.PasswordStatus = 0;
            user.Status = req.Status;

            db.Users.Add(user);
            db.SaveChanges();
            UserId = user.Id;//User ID Going back to Portal

            //Add Agent Code to Agent Notification Message
            string Message = string.Empty;
            if (req.RoleId > 4)
            {
                var agent = db.Agents.FirstOrDefault(x => x.Id == req.UserTypeParentId);
                Message = String.Format("LoginID: {0} || Password: {1} ||Agent Code: {2}", user.Email, passcode, agent != null ? agent.Code : "");
            }
            else
            {
                Message = String.Format("LoginID: {0} || Password: {1}", user.Email, passcode);

            }

            ChamsICSLib.Data.Notification smsNotification = new ChamsICSLib.Data.Notification
            {
                TypeId = ChamsICSLib.Model.NotificationType.USER_SMS,
                Message = Message,
                Recipient = user.Mobile,
                ReferenceId = user.Id,
                Status = 0
            };

            ChamsICSLib.Data.Notification emailNotification = new ChamsICSLib.Data.Notification
            {
                TypeId = ChamsICSLib.Model.NotificationType.USER_EMAIL,
                Message = Message,
                Recipient = user.Email,
                ReferenceId = user.Id,
                Status = 0
            };

            db.Notifications.Add(smsNotification);
            db.Notifications.Add(emailNotification);
            db.SaveChanges();

            //Log Audit Trail
            LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.CREATE, "User", "UserId:" + user.Id, "");

            return result;
        }
        
        internal bool EmailExists(string email)
        {
            var User = db.Users.FirstOrDefault(x => x.Email == email.Trim());
            return User != null;
        }

        internal bool MobileExists(string mobile)
        {
            var User = db.Users.FirstOrDefault(x => x.Mobile == mobile.Trim());
            return User != null;
        }

        internal bool UserLogin(UserLoginReq req, out UserLoginRes res)
        {
            res = new UserLoginRes();

            System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
            string password = Utils.GetMd5Hash(md5Hash, req.UserPassword);

            ChamsICSLib.Data.User user = db.Users.FirstOrDefault(x => x.Email == req.Email && x.Password == password);


            if (user != null)
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Email = user.Email;
                res.Mobile = user.Mobile;
                res.RoleId = user.RoleId.Value;
                res.UserId = user.Id;
                res.PasswordStatus = user.PasswordStatus == null ? 0 : user.PasswordStatus.Value;
                res.AccountStatus = user.Status == null ? 0 : user.Status.Value; ;
                res.UserDashBoardData = getUserDashBoardData(user);

                //Log Audit Trail
                LogAuditData(user.ClientId==null? 0:user.ClientId.Value, user.Id, AuditTrailType.LOGIN, "User","UserId:"+user.Id, "");

                return true;
            }
            else
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Invalid Login Details";
                return false;
            }
        }

        private UserDashBoardData getUserDashBoardData(ChamsICSLib.Data.User user)
        {
            UserDashBoardData UserDashBoardData = new UserDashBoardData();
            
            if(user.RoleId==1|| user.RoleId == 2) {
                UserDashBoardData.RoleName = db.UserRoles.FirstOrDefault(x=> x.id==user.RoleId).Description;
                UserDashBoardData.ClientName = "Chams ICS Administrator";
                UserDashBoardData.ClientId = 0;
            }
            else if (user.RoleId == 3 || user.RoleId == 4) {
                UserDashBoardData.RoleName = db.UserRoles.FirstOrDefault(x => x.id == user.RoleId).Description;
                UserDashBoardData.ClientName = db.Clients.FirstOrDefault(x=> x.Id==user.UserTypeParentId).Name;
                UserDashBoardData.UserTypeParentId = user.UserTypeParentId.Value;
                UserDashBoardData.ClientId = user.UserTypeParentId.Value;
            }
            else if (user.RoleId == 5 || user.RoleId == 6) {
                UserDashBoardData.RoleName = db.UserRoles.FirstOrDefault(x => x.id == user.RoleId).Description;
                UserDashBoardData.ClientName = db.Agents.FirstOrDefault(x => x.Id == user.UserTypeParentId).Name;
                UserDashBoardData.UserTypeParentId = user.UserTypeParentId.Value;
                UserDashBoardData.ClientId = db.Agents.FirstOrDefault(x => x.Id == UserDashBoardData.UserTypeParentId).ClientId.Value;
            }

            return UserDashBoardData;
        }

        internal RoleRes GetAllRoles()
        {
            RoleRes res = new RoleRes();
            var role = db.UserRoles.Select(x => new Model.Role
            {
                RoleId = x.id,
                Description = x.Description
            });

            res.Role = role;

            return res;
        }

        internal GetAllUserRes GetAllUsers(int roleId, int userTypeParentId)
        {
            GetAllUserRes res = new GetAllUserRes();

            if (roleId == 1 || roleId == 2)
            {
                var users = db.Users.Select(req => new Model.User
                {
                    UserId = req.Id,
                    Mobile = req.Mobile,
                    RoleId = req.RoleId.Value,
                    Email = req.Email,
                    Password = req.Password,
                    PasswordStatus = req.PasswordStatus.Value,
                    Status = req.Status.Value,
                });

                res.Users = users;
            }

            else if (roleId == 3 || roleId == 4)
            {
                //Join User Table to UserClients Table
                //Select Users where UserClients.ClientId = to the parameter userTypeParentId
            }
            else if (roleId == 5 || roleId == 6)
            {
                //Join User Table to UserClients Table
                //Select Users where UserClients.ClientId = to the parameter userTypeParentId
            }

            return res;
        }

        internal GetAllUserRes GetAllUsers()
        {
            GetAllUserRes res = new GetAllUserRes();
            var users = db.Users.Select(user => new Model.User
            {
                UserId = user.Id,
                RoleId = user.RoleId,
                Email = user.Email,
                Mobile = user.Mobile,
                Password = user.Password,
                PasswordStatus = user.PasswordStatus,
                Status = user.Status
            });

            res.Users = users;

            return res;
        }

        internal GetAllUserRes GetUserAssesibleUsers(int roleId, int clientId)
        {
            GetAllUserRes res = new GetAllUserRes();
            var users = db.Users.Where(x=> x.ClientId==clientId && x.RoleId > roleId).Select(user => new Model.User
            {
                UserId = user.Id,
                RoleId = user.RoleId,
                Email = user.Email,
                Mobile = user.Mobile,
                Password = user.Password,
                PasswordStatus = user.PasswordStatus,
                Status = user.Status
            });

            res.Users = users;

            return res;

        }

        internal FindUserRes GetUser(int Id)
        {
            FindUserRes res = new FindUserRes();
            var req = db.Users.FirstOrDefault(x => x.Id == Id);

            var user = new Model.User
            {
                UserId = req.Id,
                Mobile = req.Mobile,
                RoleId = req.RoleId.Value,
                Email = req.Email,
                Password = req.Password,
                PasswordStatus = req.PasswordStatus.Value,
                Status = req.Status.Value,
            };

            if (user == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.User = (Model.User)user;

            }
            return res;

        }

        internal FindUserRes GetUserByEmail(string email)
        {
            FindUserRes res = new FindUserRes();
            var req = db.Users.FirstOrDefault(x => x.Email == email);

            var user = new Model.User
            {
                UserId = req.Id,
                Mobile = req.Mobile,
                RoleId = req.RoleId.Value,
                Email = req.Email,
                Password = req.Password,
                PasswordStatus = req.PasswordStatus.Value,
                Status = req.Status.Value,
            };

            if (user == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Email";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.User = (Model.User)user;

            }
            return res;

        }

        private int GetUSerTypeParentId(int Id, int? roleId)
        {
            if (roleId <=2)
            {
                return 0;
            }
            else if (roleId >2 )
            {
                return db.Users.FirstOrDefault(x => x.Id == Id).UserTypeParentId.Value;

            }
            else { return 0; }
        }

        internal bool UpdateUserStatus(UserStatus req)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == req.UserId);
            if (user != null)
            {
                user.Status = req.status;
                db.SaveChanges();

                //Log Audit Trail
                LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.EDIT, "User", "Change Status for UserId:" + req.UserId, "");

                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool ChangeUserPassword(ChangeUserPassword req)
        {
            System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
            string old_password = Utils.GetMd5Hash(md5Hash, req.OldPassword);

            var user = db.Users.FirstOrDefault(x => x.Id == req.UserId && x.Password==old_password);
            if (user != null)
            {
                string password = Utils.GetMd5Hash(md5Hash, req.NewPassword);

                user.Password = password;
                user.PasswordStatus = 1;

                db.SaveChanges();
                try
                {
                    if (req.AuditTrailData.userId != null)
                    {
                        LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.EDIT, "User", "Change Password for UserId:" + req.UserId, "");
                    }
                }
                catch (Exception ex)
                {
                    //Whatever Happens, Log it and move on...
                    Logger.logToFile(ex, ErrorLogPath);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool ResetUserPassword(ResetUserPasswordReq req)
        {
            var user = db.Users.FirstOrDefault(x => x.Id == req.Id);
            if (user != null)
            {
                System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
                Random r = new CryptoRandom();
                string passcode = user.Email.Split(new char[] { '@' })[0] + DateTime.Now.Year.ToString();
                string password = Utils.GetMd5Hash(md5Hash, passcode);

                user.Password = password;
                user.PasswordStatus = 0;

                db.SaveChanges();

                try
                {
                    if (req.AuditTrailData.userId != null)
                    {
                        LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.EDIT, "User", "Reset Password for UserId:" + user.Id, "");
                    }
                }
                catch (Exception ex)
                {
                    //Whatever Happens, Log it and move on...
                    Logger.logToFile(ex, ErrorLogPath);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool ForgetUserPassword(string email)
        {
            var user = db.Users.FirstOrDefault(x => x.Email == email);
            if (user != null)
            {
                System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();
                Random r = new CryptoRandom();
                string passcode = user.Email.Split(new char[] { '@' })[0] + DateTime.Now.Year.ToString();
                string password = Utils.GetMd5Hash(md5Hash, passcode);

                user.Password = password;
                user.PasswordStatus = 0;

                db.SaveChanges();

                try
                {
                    LogAuditData(user.ClientId, user.Id, AuditTrailType.EDIT, "User", "Forget Password Reset for UserId:" + user.Id, "");
                }
                catch (Exception ex)
                {
                    //Whatever Happens, Log it and move on...
                    Logger.logToFile(ex, ErrorLogPath);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Executive Dashboard
        internal DashboardRes GetDashBoardData(DashboardReq req)
        {
            DashboardRes res = new DashboardRes();
            if (req.RoleId == 1 || req.RoleId == 2)
            {
                //This is Sytem User.. 
                res.TotalClient = db.Clients.Count();
                res.TotalAgent = db.Agents.Count();
                res.TotalTerminal = db.Terminals.Count();
                res.TotalTransaction = db.TransactionLogs.Count();
                res.TransctionValue = Convert.ToDecimal(db.TransactionLogs.Sum(x => x.Amount));
                res.TotalNotifications = db.Notifications.Count();

                return res;
            }
            else if (req.RoleId == 3 || req.RoleId == 4)
            {
                //This is Client User
                int UserClientId = db.Users.FirstOrDefault(x => x.Id == req.UserId).UserTypeParentId.Value;

                res.TotalAgent = db.Agents.Where(x => x.ClientId == UserClientId).Count();
                IList<int> ClientAgents = db.Agents.Where(x => x.ClientId == UserClientId).Select(x => x.Id).ToList();
                res.TotalTerminal = db.Terminals.Where(x => x.Status == 1).Where(x => ClientAgents.Contains(x.AgentId.Value)).Count();
                res.TotalTransaction = db.TransactionLogs.Where(x => x.ClientId == UserClientId).Count();
                res.TransctionValue = Convert.ToDecimal(db.TransactionLogs.Where(x => x.ClientId == UserClientId).Sum(x => x.Amount));

                return res;
            }
            else if (req.RoleId == 5 || req.RoleId == 6)
            {
                //This is Agent USer
                int UserAgentId = db.Users.FirstOrDefault(x => x.Id == req.UserId).UserTypeParentId.Value;
                res.TotalTerminal = db.Terminals.Where(x => x.Status == 1).Where(x => x.AgentId == UserAgentId).Count();
                res.TotalTransaction = db.TransactionLogs.Where(x => x.AgentId == UserAgentId).Count();
                res.TransctionValue =  Convert.ToDecimal(db.TransactionLogs.Where(x => x.AgentId == UserAgentId).Sum(x => x.Amount));
                return res;
            }
            else if (req.RoleId == 7)
            {
                //This is tax payer
                string userEmail = db.Users.FirstOrDefault(x => x.Id == req.UserId).Email;
                //string residentId = db.TransactionLogs.FirstOrDefault(x => x.Email == userEmail).ResidentId;
                //res.TotalTerminal = db.Terminals.Where(x => x.Status == 1).Where(x => x.AgentId == UserAgentId).Count();
                res.TotalTransaction = db.TransactionLogs.Where(x => x.Email == userEmail).Count();
                res.TransctionValue = Convert.ToDecimal(db.TransactionLogs.Where(x => x.Email == userEmail).Sum(x => x.Amount));
                res.InvoiceCount = db.Invoices.Where(x => x.Email == userEmail).Count();
                return res;
            }

            return null;
        }

        internal ExecutiveDashboardRes GetExecutiveDashboardData(ExecutiveDashboardReq req)
        {
            ExecutiveDashboardRes res = new ExecutiveDashboardRes();
            if (req.RoleId == 1 || req.RoleId == 2)
            {
                //This is Sytem User.. 
                res.TotalClient = db.Clients.Count();
                res.TotalAgent = db.Agents.Count();
                res.TotalTerminal = db.Terminals.Count();
                res.TotalTransaction = db.TransactionLogs.Count();
                res.TotalTransctionValue = Convert.ToDecimal(db.TransactionLogs.Sum(x => x.Amount));
                res.TotalNotification = db.Notifications.Count();

                return res;
            }
            else if (req.RoleId == 3 || req.RoleId == 4)
            {
                //This is Client User
                int UserClientId = db.Users.FirstOrDefault(x => x.Id == req.UserId).UserTypeParentId.Value;

                res.TotalAgent = db.Agents.Where(x => x.ClientId == UserClientId).Count();

                IList<int> ClientAgents = db.Agents.Where(x => x.ClientId == UserClientId).Select(x => x.Id).ToList();
                res.TotalTerminal = db.Terminals.Where(x => x.Status == 1).Where(x => ClientAgents.Contains(x.AgentId.Value)).Count();

                res.TotalTransaction = db.TransactionLogs.Where(x => x.ClientId == UserClientId).Count();
                res.TotalTransctionValue = Convert.ToDecimal(db.TransactionLogs.Where(x => x.ClientId == UserClientId).Sum(x => x.Amount));

                res.AgentLeaderStats = GetLeadingAgentStats(UserClientId);
                res.RevenueLeaderStats = GetLeadingRevenueStats(UserClientId);
                res.AgentStats = GetClientAgentStats(UserClientId);
                return res;
            }
            else if (req.RoleId == 5 || req.RoleId == 6)
            {
                //This is Agent USer
                int UserAgentId = db.Users.FirstOrDefault(x => x.Id == req.UserId).UserTypeParentId.Value;
                res.TotalTerminal = db.Terminals.Where(x => x.Status == 1).Where(x => x.AgentId == UserAgentId).Count();
                res.TotalTransaction = db.TransactionLogs.Where(x => x.AgentId == UserAgentId).Count();
                res.TotalTransctionValue = Convert.ToDecimal(db.TransactionLogs.Where(x => x.AgentId == UserAgentId).Sum(x => x.Amount));
                return res;
            }
            return res;
        }

        internal TaxpayerDashboardRes GetTaxpayerDashBoardData(DashboardReq req)
        {
            TaxpayerDashboardRes res = new TaxpayerDashboardRes();
            //This is Sytem User.. 
            res.Wallet = db.Wallets.Where(x => x.Email == req.Email).Select(x => x.Amount).FirstOrDefault();
            res.Invoices = db.Invoices.Where(x => x.Email == req.Email).Count();
            res.Notification = 0;

            return res;
        }

        private IEnumerable<AgentStats> GetClientAgentStats(int userClientId)
        {
            List<AgentStats> AgentStats = new List<AgentStats>();
            var agents = db.Agents.Where(x => x.ClientId == userClientId).ToList();

            foreach(var agent in agents)
            {
                AgentStats agentStat = new AgentStats();
                agentStat.AgentId = agent.Id;
                agentStat.AgentCode = agent.Code;
                agentStat.AgentName = agent.Name;
                agentStat.TerminalStats = GetAgentTerminalStats(agent.Id);

                AgentStats.Add(agentStat);
            }

            return AgentStats;
        }

        private AgentTerminalStats GetAgentTerminalStats(int AgentId)
        {
            AgentTerminalStats stats = new AgentTerminalStats();

            var AgentTerminals = db.Terminals.Where(x => x.AgentId == AgentId && x.Status ==1).ToList();
            var AgentTransactions = db.TransactionLogs.Where(x => x.AgentId == AgentId).ToList();


            DateTime Today = DateTime.Today;
            DateTime SevenDaysAgo = DateTime.Now.AddDays(-7);
            DateTime ThirtyDaysAgo = DateTime.Now.AddDays(-30);
            DateTime NinetyDaysAgo = DateTime.Now.AddDays(-90);
            DateTime OneEightyDaysAgo = DateTime.Now.AddDays(-180);

            stats.TotalTerminals = AgentTerminals.Count();
            stats.TotalActiveTerminals = AgentTransactions.GroupBy(x => x.TerminalId).Count();
            stats.TotalTodayActiveTerminals = AgentTransactions.Where(x => x.TransactionDate.Value.Date == Today). GroupBy(x => x.TerminalId).Count();
            stats.Total7DaysActiveTerminals = AgentTransactions.Where(x => x.TransactionDate >= SevenDaysAgo && x.TransactionDate <= DateTime.Now).GroupBy(x => x.TerminalId).Count();
            stats.Total30DaysActiveTerminals = AgentTransactions.Where(x => x.TransactionDate >= ThirtyDaysAgo && x.TransactionDate <= DateTime.Now).GroupBy(x => x.TerminalId).Count();
            stats.Total3MonthsActiveTerminals = AgentTransactions.Where(x => x.TransactionDate >= NinetyDaysAgo && x.TransactionDate <= DateTime.Now).GroupBy(x => x.TerminalId).Count();       
            stats.Total6MonthsActiveTerminals = AgentTransactions.Where(x => x.TransactionDate >= OneEightyDaysAgo && x.TransactionDate <= DateTime.Now).GroupBy(x => x.TerminalId).Count();

            stats.TotalInActiveTerminals = stats.TotalTerminals - stats.TotalActiveTerminals;
            stats.TotalTodayInActiveTerminals = stats.TotalTerminals - stats.TotalTodayActiveTerminals;
            stats.Total7DaysInActiveTerminals = stats.TotalTerminals - stats.Total7DaysActiveTerminals;
            stats.Total30DaysInActiveTerminals = stats.TotalTerminals - stats.Total30DaysActiveTerminals;
            stats.Total3MonthsInActiveTerminals = stats.TotalTerminals - stats.Total3MonthsActiveTerminals;
            stats.Total6MonthsInActiveTerminals = stats.TotalTerminals - stats.Total6MonthsActiveTerminals;

            stats.TotalTransactions = AgentTransactions.Count();
            stats.TotalTodayTransactions = AgentTransactions.Where(x => x.TransactionDate.Value.Date == Today).Count();
            stats.Total7DaysTransactions = AgentTransactions.Where(x => x.TransactionDate >= SevenDaysAgo && x.TransactionDate <= DateTime.Now).Count();
            stats.Total30DaysTransactions = AgentTransactions.Where(x => x.TransactionDate >= ThirtyDaysAgo && x.TransactionDate <= DateTime.Now).Count();
            stats.Total3MonthsTransactions = AgentTransactions.Where(x => x.TransactionDate >= NinetyDaysAgo && x.TransactionDate <= DateTime.Now).Count();
            stats.Total6MonthsTransactions = AgentTransactions.Where(x => x.TransactionDate >= OneEightyDaysAgo && x.TransactionDate <= DateTime.Now).Count();

            stats.TotalTransactionVal = AgentTransactions.Sum(x => x.Amount).Value;
            stats.TotalTodayTransactionVal = AgentTransactions.Where(x => x.TransactionDate.Value.Date == Today).Sum(x=> x.Amount).Value;
            stats.Total7DaysTransactionVal = AgentTransactions.Where(x => x.TransactionDate >= SevenDaysAgo && x.TransactionDate <= DateTime.Now).Sum(x => x.Amount).Value;
            stats.Total30DaysTransactionVal = AgentTransactions.Where(x => x.TransactionDate >= ThirtyDaysAgo && x.TransactionDate <= DateTime.Now).Sum(x => x.Amount).Value;
            stats.Total3MonthsTransactionVal = AgentTransactions.Where(x => x.TransactionDate >= NinetyDaysAgo && x.TransactionDate <= DateTime.Now).Sum(x => x.Amount).Value;
            stats.Total6MonthsTransactionVal = AgentTransactions.Where(x => x.TransactionDate >= OneEightyDaysAgo && x.TransactionDate <= DateTime.Now).Sum(x => x.Amount).Value;

            return stats;
        }

        private RevenueLeaderStats GetLeadingRevenueStats(int userClientId)
        {
            RevenueLeaderStats stats = new RevenueLeaderStats();
            var revenueRecordsList = from renenueTransactions in db.TransactionLogs
                                 where renenueTransactions.ClientId == userClientId
                                 group renenueTransactions by renenueTransactions.RevenueCode into result
                                 join revenues in db.RevenueItems
                                   on result.FirstOrDefault().RevenueCode equals revenues.Code
                                 join revenueHeads in db.RevenueHeads
                                 on revenues.RevenueHeadId equals revenueHeads.Id

                                 orderby result.Sum(x => x.Amount)
                                 select new
                                 {
                                     ID = result.Key,
                                     SUM = result.Sum(x => x.Amount),
                                     NAME = revenues.Name,
                                     HEAD = revenueHeads.Name
                                 };

            var revenueRecords = revenueRecordsList.ToList();

            stats.LeadingRevenue = revenueRecords.OrderByDescending(x => x.SUM).Select(x => x.NAME).FirstOrDefault() +
                " / " +
                 revenueRecords.OrderByDescending(x => x.SUM).Select(x => x.HEAD).FirstOrDefault();

            stats.TrailingRevenue = revenueRecords.OrderBy(x => x.SUM).Select(x => x.NAME).FirstOrDefault() +
                " / " +
                 revenueRecords.OrderByDescending(x => x.SUM).Select(x => x.HEAD).FirstOrDefault();

            stats.LeadingRevenueVal = revenueRecords.OrderByDescending(x => x.SUM).Select(x => x.SUM).FirstOrDefault().Value;
            stats.TrailingRevenueVal = revenueRecords.OrderBy(x => x.SUM).Select(x => x.SUM).FirstOrDefault().Value;

            return stats;
        }

        private AgentLeaderStats GetLeadingAgentStats(int userClientId)
        {
            AgentLeaderStats stats = new AgentLeaderStats();
            var agentRecords = from agentTransactions in db.TransactionLogs
                               where agentTransactions.ClientId == userClientId
                               group agentTransactions by agentTransactions.AgentId into result
                               join agents in db.Agents
                                 on result.FirstOrDefault().AgentId equals agents.Id

                               orderby result.Sum(x => x.Amount)
                               select new
                               {
                                   ID = result.Key,
                                   SUM = result.Sum(x => x.Amount),
                                   NAME = agents.Name
                               };


            stats.LeadingAgent = agentRecords.OrderByDescending(x => x.SUM).Select(x => x.NAME).FirstOrDefault();
            stats.TrailingAgent = agentRecords.OrderBy(x => x.SUM).Select(x => x.NAME).FirstOrDefault();
            stats.LeadingAgentVal = agentRecords.OrderByDescending(x => x.SUM).Select(x => x.SUM).FirstOrDefault().Value;
            stats.TrailingAgentVal = agentRecords.OrderBy(x => x.SUM).Select(x => x.SUM).FirstOrDefault().Value;
            return stats;
        }
        #endregion

        #region Periodic Dashboard
        internal ExecutiveDashboardRes GetPeriodicDashboardData(ExecutiveDashboardReq req)
        {
            ExecutiveDashboardRes res = new ExecutiveDashboardRes();
            if (req.RoleId == 1 || req.RoleId == 2)
            {
                //This is Sytem User.. 
                res.TotalClient = db.Clients.Count();
                res.TotalAgent = db.Agents.Count();
                res.TotalTerminal = db.Terminals.Count();
                res.TotalTransaction = db.TransactionLogs.Where(x=> x.TransactionDate>= req.StartDate && x.TransactionDate<=req.EndDate).Count();
                res.TotalTransctionValue = Convert.ToDecimal(db.TransactionLogs.Where(x=> x.TransactionDate>= req.StartDate && x.TransactionDate<=req.EndDate).Sum(x => x.Amount));
                res.TotalNotification = db.Notifications.Count();

                return res;
            }
            else if (req.RoleId == 3 || req.RoleId == 4)
            {
                //This is Client User
                int UserClientId = db.Users.FirstOrDefault(x => x.Id == req.UserId).UserTypeParentId.Value;

                IList<int> ClientAgents = db.Agents.Where(x => x.ClientId == UserClientId).Select(x => x.Id).ToList();
                res.TotalAgent = ClientAgents.Count();

                res.TotalTerminal = db.Terminals.Where(x => x.Status == 1).Where(x => ClientAgents.Contains(x.AgentId.Value)).Count();

                res.TotalTransaction = db.TransactionLogs.Where(x => x.ClientId == UserClientId && x.TransactionDate >= req.StartDate && x.TransactionDate <= req.EndDate).Count();
                res.TotalTransctionValue = Convert.ToDecimal(db.TransactionLogs.Where(x => x.ClientId == UserClientId && x.TransactionDate >= req.StartDate && x.TransactionDate <= req.EndDate).Sum(x => x.Amount));

                res.AgentLeaderStats = GetLeadingAgentStats(UserClientId,req.StartDate, req.EndDate);
                res.RevenueLeaderStats = GetLeadingRevenueStats(UserClientId,req.StartDate, req.EndDate);
                res.AgentStats = GetClientAgentStats(UserClientId,req.StartDate, req.EndDate);
                return res;
            }
            else if (req.RoleId == 5 || req.RoleId == 6)
            {
                //This is Agent USer
                int UserAgentId = db.Users.FirstOrDefault(x => x.Id == req.UserId).UserTypeParentId.Value;
                res.TotalTerminal = db.Terminals.Where(x => x.Status == 1).Where(x => x.AgentId == UserAgentId).Count();
                res.TotalTransaction = db.TransactionLogs.Where(x => x.AgentId == UserAgentId).Count();
                res.TotalTransctionValue = Convert.ToDecimal(db.TransactionLogs.Where(x => x.AgentId == UserAgentId).Sum(x => x.Amount));
                return res;
            }
            return res;
        }

        private IEnumerable<AgentStats> GetClientAgentStats(int userClientId, DateTime StartDate, DateTime EndDate)
        {
            List<AgentStats> AgentStats = new List<AgentStats>();
            var agents = db.Agents.Where(x => x.ClientId == userClientId).ToList();

            foreach (var agent in agents)
            {
                AgentStats agentStat = new AgentStats();
                agentStat.AgentId = agent.Id;
                agentStat.AgentCode = agent.Code;
                agentStat.AgentName = agent.Name;

                //To Avoid going to the database twice in Agent terminal stats and Revenue stats
                // reuse the TransactionLogs use for Terminal Stats in Revenue Stats
                IList<AgentTransactionsModel> AgentTransactions; 

                agentStat.TerminalStats = GetAgentTerminalStats(agent.Id, StartDate,EndDate, out AgentTransactions);
                agentStat.RevenueStats = GetAgentRevenueStats(agent.Id, AgentTransactions);
                agentStat.PeriodicRevenueStats = GetAgentPeriodicRevenueStats(agent.Id, StartDate, EndDate, AgentTransactions);

                AgentStats.Add(agentStat);
            }

            return AgentStats;
        }

        private IEnumerable<RevenueStats> GetAgentPeriodicRevenueStats(int agentId, DateTime startDate, DateTime endDate, IList<AgentTransactionsModel> AgentTransactions)
        {
            List<RevenueStats> RevenueStat = new List<RevenueStats>();

            //Group the Agent transactions by RevenueCode and Apply Date Filter
            var agentTransactions = from result in AgentTransactions
                                    where result.Date>= startDate && result.Date <=endDate
                                    group result by result.RevenueCode into results
                                    select new
                                    {
                                        RevenueName = results.FirstOrDefault().RevenueCode + ": " + results.FirstOrDefault().RevenueItemName + " / " + results.FirstOrDefault().RevenueHeadName,
                                        TotalTransactionVol = results.Count(),
                                        TotalTransactionVal = results.Sum(x => x.Amount)
                                    };


            foreach (var x in agentTransactions.ToList())
            {
                RevenueStats stat = new RevenueStats();
                stat.RevenueName = x.RevenueName;
                stat.TotalTransactionVol = x.TotalTransactionVol;
                stat.TotalTransactionVal = x.TotalTransactionVal;

                RevenueStat.Add(stat);
            }

            return RevenueStat;
        }

        private IEnumerable<RevenueStats> GetAgentRevenueStats(int agentId, IList<AgentTransactionsModel> AgentTransactions)
        {
            List<RevenueStats> RevenueStat = new List<RevenueStats>();

            //Group the Agent transactions by RevenueCode
            var agentTransactions = from result in AgentTransactions
                                    group result by result.RevenueCode into results
                                    select new {
                                        RevenueName = results.FirstOrDefault().RevenueCode+": "+ results.FirstOrDefault().RevenueItemName + " / "+ results.FirstOrDefault().RevenueHeadName,
                                        TotalTransactionVol = results.Count(),
                                        TotalTransactionVal = results.Sum(x=> x.Amount)
                                    };


            foreach (var x in agentTransactions.ToList())
            {
                RevenueStats stat = new RevenueStats();
                stat.RevenueName = x.RevenueName;
                stat.TotalTransactionVol = x.TotalTransactionVol;
                stat.TotalTransactionVal = x.TotalTransactionVal;

                RevenueStat.Add(stat);
            }

            return RevenueStat;
        }

        private AgentTerminalStats GetAgentTerminalStats(int AgentId, DateTime StartDate, DateTime EndDate, out IList<AgentTransactionsModel> AgentTransactions)
        {
            AgentTerminalStats stats = new AgentTerminalStats();

            var AgentTerminals = db.Terminals.Where(x => x.AgentId == AgentId && x.Status == 1).ToList();
            //AgentTransactions = db.TransactionLogs.Where(x => x.AgentId == AgentId).ToList();

            var result = from transactions in db.TransactionLogs
                                                          where transactions.AgentId == AgentId
                                                          join revItem in db.RevenueItems on transactions.RevenueCode equals revItem.Code
                                                          join revHead in db.RevenueHeads on revItem.RevenueHeadId equals revHead.Id
                                                          select new AgentTransactionsModel
                                                          {
                                                              AgentId = transactions.AgentId,
                                                              TerminalId = transactions.TerminalId,
                                                              RevenueCode = transactions.RevenueCode,
                                                              Amount = transactions.Amount,
                                                              Date = transactions.TransactionDate,
                                                              RevenueItemName = revItem.Name,
                                                              RevenueHeadName = revHead.Name
                                                          };
            AgentTransactions = result.ToList();

            stats.TotalTerminals = AgentTerminals.Count();
            stats.TotalActiveTerminals = AgentTransactions.GroupBy(x => x.TerminalId).Count();
            stats.TotalInActiveTerminals = stats.TotalTerminals - stats.TotalActiveTerminals;

            stats.TotalTransactions = AgentTransactions.Count();
            stats.TotalTransactionVal = AgentTransactions.Sum(x => x.Amount).Value;

            stats.TotalPeriodicTransactions = AgentTransactions.Where(x => x.Date >= StartDate && x.Date <= EndDate).Count();
            stats.TotalPeriodicTransactionVal = AgentTransactions.Where(x => x.Date >= StartDate && x.Date <= EndDate).Sum(x => x.Amount).Value;

            stats.TotalPeriodicActiveTerminals = AgentTransactions.Where(x=> x.Date>=StartDate && x.Date <=EndDate).GroupBy(x => x.TerminalId).Count();
            stats.TotalPeriodicInActiveTerminals = stats.TotalTerminals - stats.TotalPeriodicActiveTerminals;

            return stats;
        }

        private RevenueLeaderStats GetLeadingRevenueStats(int userClientId, DateTime StartDate, DateTime EndDate)
        {
            RevenueLeaderStats stats = new RevenueLeaderStats();
            var revenueRecordsList = from renenueTransactions in db.TransactionLogs
                                     where renenueTransactions.ClientId == userClientId
                                     where renenueTransactions.TransactionDate >= StartDate
                                     where renenueTransactions.TransactionDate <=EndDate
                                     group renenueTransactions by renenueTransactions.RevenueCode into result
                                     join revenues in db.RevenueItems
                                       on result.FirstOrDefault().RevenueCode equals revenues.Code
                                     join revenueHeads in db.RevenueHeads
                                     on revenues.RevenueHeadId equals revenueHeads.Id

                                     orderby result.Sum(x => x.Amount)
                                     select new
                                     {
                                         ID = result.Key,
                                         SUM = result.Sum(x => x.Amount),
                                         NAME = revenues.Name,
                                         HEAD = revenueHeads.Name
                                     };

            var revenueRecords = revenueRecordsList.ToList();

            stats.LeadingRevenue = revenueRecords.OrderByDescending(x => x.SUM).Select(x => x.NAME).FirstOrDefault() +
                " / " +
                 revenueRecords.OrderByDescending(x => x.SUM).Select(x => x.HEAD).FirstOrDefault();

            stats.TrailingRevenue = revenueRecords.OrderBy(x => x.SUM).Select(x => x.NAME).FirstOrDefault() +
                " / " +
                 revenueRecords.OrderByDescending(x => x.SUM).Select(x => x.HEAD).FirstOrDefault();

            stats.LeadingRevenueVal = revenueRecords.OrderByDescending(x => x.SUM).Select(x => x.SUM).FirstOrDefault();
            stats.TrailingRevenueVal = revenueRecords.OrderBy(x => x.SUM).Select(x => x.SUM).FirstOrDefault();

            return stats;
        }

        private AgentLeaderStats GetLeadingAgentStats(int userClientId, DateTime StartDate, DateTime EndDate)
        {
            AgentLeaderStats stats = new AgentLeaderStats();
            var agentRecords = from agentTransactions in db.TransactionLogs
                               where agentTransactions.ClientId == userClientId
                               where agentTransactions.TransactionDate >= StartDate
                               where agentTransactions.TransactionDate <= EndDate
                               group agentTransactions by agentTransactions.AgentId into result
                               join agents in db.Agents
                                 on result.FirstOrDefault().AgentId equals agents.Id

                               orderby result.Sum(x => x.Amount)
                               select new
                               {
                                   ID = result.Key,
                                   SUM = result.Sum(x => x.Amount),
                                   NAME = agents.Name
                               };

            var agentRecordLoaded = agentRecords.ToList();
            stats.LeadingAgent = agentRecords.OrderByDescending(x => x.SUM).Select(x => x.NAME).FirstOrDefault();
            stats.TrailingAgent = agentRecords.OrderBy(x => x.SUM).Select(x => x.NAME).FirstOrDefault();
            stats.LeadingAgentVal = agentRecords.OrderByDescending(x => x.SUM).Select(x => x.SUM).FirstOrDefault();
            stats.TrailingAgentVal = agentRecords.OrderBy(x => x.SUM).Select(x => x.SUM).FirstOrDefault();
            return stats;
        }

        #endregion

        #region Report

        internal List<ClientSummary> GetClientSummary()
        {
            ClientSummaryRes res = new ClientSummaryRes();
            List<ClientSummary> sSum = new List<ClientSummary>();
            var clients = db.Agents.ToList();

            foreach (var client in clients)
            {
                var clientId = client.Id;

                ClientSummary clientSummary = new ClientSummary();
                clientSummary.clientId = client.Id;
                clientSummary.ClientName = client.Name;
                clientSummary.TotalAgents = db.Agents.Where(x => x.ClientId == clientId).Count();

                IList<int> ClientAgents = db.Agents.Where(x => x.ClientId == clientId).Select(x => x.Id).ToList();

                if (ClientAgents.Count>0)
                clientSummary.TotalTerminal = db.Terminals.Where(x => x.Status == 1).Where(x => ClientAgents.Contains(x.AgentId.Value)).Count();

                clientSummary.TotalTransactionCount = db.TransactionLogs.Where(x => x.ClientId == client.Id).Count();
                clientSummary.TotalTransactionValue = db.TransactionLogs.Where(x => x.ClientId == client.Id).Sum(x => x.Amount);

                sSum.Add(clientSummary);
            }
            
            return sSum;
        }

        internal List<AgentSummary> GetAgentSummary(int clientId)
        {
            AgentSummaryRes res = new AgentSummaryRes();
            List<AgentSummary> aSum = new List<AgentSummary>();

            var agents = db.Agents.Where(x => x.ClientId == clientId);


            foreach (var agent in agents)
            {
                AgentSummary agentSummary = new AgentSummary();
                agentSummary.AgentId = agent.Id;
                agentSummary.AgentName = agent.Name;
                agentSummary.TotalTerminal = db.Terminals.Where(x => x.Status == 1).Where(x => x.AgentId == agent.Id).Count();
                agentSummary.TotalTransactionCount = db.TransactionLogs.Where(x => x.AgentId == agent.Id).Count();
                agentSummary.TotalTransactionValue = db.TransactionLogs.Where(x => x.AgentId == agent.Id).Sum(x=> x.Amount);

                aSum.Add(agentSummary);
            }

           
            return aSum;
        }

        internal List<Report> GetTransactionSummary(TransactionSummary request)
        {
            List<Report> res = new List<Report>();
            if (request.clientId == null || request.clientId == 0)
            {
                //Get All transaction filter with dates alone
                var transaction = from tLogs in db.TransactionLogs
                                  join agents in db.Agents on tLogs.AgentId equals agents.Id
                                  join terminals in db.Terminals on tLogs.TerminalId equals terminals.Id
                                  join revenues in db.RevenueItems on tLogs.RevenueCode equals revenues.Code
                                  join revenueHead in db.RevenueHeads on revenues.RevenueHeadId equals revenueHead.Id
                                  join ministry in db.Ministries on revenueHead.MinistryId equals ministry.Id
                                  where tLogs.TransactionDate >= request.startDate && tLogs.TransactionDate <= request.endDate

                                  select new Report
                                  {
                                      TransactionId = tLogs.Id,
                                      Agent = agents.Name,
                                      Terminal = terminals.Name,
                                      RevenueCode = tLogs.RevenueCode,
                                      RevenueName = revenues.Name,
                                      Ministry = ministry.Name,
                                       RevenueHead = revenueHead.Name,
                                      Amount = tLogs.Amount.Value,
                                      ResidentId = tLogs.ResidentId,
                                      TransactionCode = tLogs.Code,
                                      TransactionDate = tLogs.TransactionDate.Value
                                  };
                res.AddRange(transaction.ToList().Distinct());
            }
            else if (request.AgentId == null)
            {
                //Get All transaction filter with dates alone
                var transaction = from tLogs in db.TransactionLogs
                                  join agents in db.Agents on tLogs.AgentId equals agents.Id
                                  join terminals in db.Terminals on tLogs.TerminalId equals terminals.Id
                                  join revenues in db.RevenueItems on tLogs.RevenueCode equals revenues.Code
                                  join revenueHead in db.RevenueHeads on revenues.RevenueHeadId equals revenueHead.Id
                                  join ministry in db.Ministries on revenueHead.MinistryId equals ministry.Id
                                  where tLogs.TransactionDate >= request.startDate && tLogs.TransactionDate <= request.endDate
                                  where tLogs.ClientId == request.clientId

                                  select new Report
                                  {
                                      TransactionId = tLogs.Id,
                                      Agent = agents.Name,
                                      Terminal = terminals.Name,
                                      RevenueCode = tLogs.RevenueCode,
                                      RevenueName = revenues.Name,
                                      Ministry = ministry.Name,
                                      RevenueHead = revenueHead.Name,
                                      Amount = tLogs.Amount.Value,
                                      ResidentId = tLogs.ResidentId,
                                      TransactionCode = tLogs.Code,
                                      TransactionDate = tLogs.TransactionDate.Value
                                  };
                res.AddRange(transaction.ToList());
            }

            else if (request.terminalId == null)
            {
                //Get All transaction filter with dates alone
                var transaction = from tLogs in db.TransactionLogs
                                  join agents in db.Agents on tLogs.AgentId equals agents.Id
                                  join terminals in db.Terminals on tLogs.TerminalId equals terminals.Id
                                  join revenues in db.RevenueItems on tLogs.RevenueCode equals revenues.Code
                                  join revenueHead in db.RevenueHeads on revenues.RevenueHeadId equals revenueHead.Id
                                  join ministry in db.Ministries on revenueHead.MinistryId equals ministry.Id
                                  where tLogs.TransactionDate >= request.startDate && tLogs.TransactionDate <= request.endDate
                                  where tLogs.ClientId == request.clientId
                                  where tLogs.AgentId == request.AgentId

                                  select new Report
                                  {
                                      TransactionId = tLogs.Id,
                                      Agent = agents.Name,
                                      Terminal = terminals.Name,
                                      RevenueCode = tLogs.RevenueCode,
                                      RevenueName = revenues.Name,
                                      Ministry = ministry.Name,
                                      RevenueHead = revenueHead.Name,
                                      Amount = tLogs.Amount.Value,
                                      ResidentId = tLogs.ResidentId,
                                      TransactionCode = tLogs.Code,
                                      TransactionDate = tLogs.TransactionDate.Value
                                  };
                res.AddRange(transaction.ToList());
            }
            else if (request.RevenueCode == null)
            {
                //Get All transaction filter with dates alone
                var transaction = from tLogs in db.TransactionLogs
                                  join agents in db.Agents on tLogs.AgentId equals agents.Id
                                  join terminals in db.Terminals on tLogs.TerminalId equals terminals.Id
                                  join revenues in db.RevenueItems on tLogs.RevenueCode equals revenues.Code
                                  join revenueHead in db.RevenueHeads on revenues.RevenueHeadId equals revenueHead.Id
                                  join ministry in db.Ministries on revenueHead.MinistryId equals ministry.Id
                                  where tLogs.TransactionDate >= request.startDate && tLogs.TransactionDate <= request.endDate
                                  where tLogs.ClientId == request.clientId
                                  where tLogs.AgentId == request.AgentId
                                  where tLogs.TerminalId == request.terminalId

                                  select new Report
                                  {
                                      TransactionId = tLogs.Id,
                                      Agent = agents.Name,
                                      Terminal = terminals.Name,
                                      RevenueCode = tLogs.RevenueCode,
                                      RevenueName = revenues.Name,
                                      Ministry = ministry.Name,
                                      RevenueHead = revenueHead.Name,
                                      Amount = tLogs.Amount.Value,
                                      ResidentId = tLogs.ResidentId,
                                      TransactionCode = tLogs.Code,
                                      TransactionDate = tLogs.TransactionDate.Value
                                  };
                res.AddRange(transaction.ToList());
            }
            else
            {
                //Get All transaction filter with dates alone
                var transaction = from tLogs in db.TransactionLogs
                                  join agents in db.Agents on tLogs.AgentId equals agents.Id
                                  join terminals in db.Terminals on tLogs.TerminalId equals terminals.Id
                                  join revenues in db.RevenueItems on tLogs.RevenueCode equals revenues.Code
                                  join revenueHead in db.RevenueHeads on revenues.RevenueHeadId equals revenueHead.Id
                                  join ministry in db.Ministries on revenueHead.MinistryId equals ministry.Id
                                  where tLogs.TransactionDate >= request.startDate && tLogs.TransactionDate <= request.endDate
                                  where tLogs.ClientId == request.clientId
                                  where tLogs.AgentId == request.AgentId
                                  where tLogs.TerminalId == request.terminalId
                                  where tLogs.RevenueCode == request.RevenueCode

                                  select new Report
                                  {
                                      TransactionId = tLogs.Id,
                                      Agent = agents.Name,
                                      Terminal = terminals.Name,
                                      RevenueCode = tLogs.RevenueCode,
                                      RevenueName = revenues.Name,
                                      Ministry = ministry.Name,
                                      RevenueHead = revenueHead.Name,
                                      Amount = tLogs.Amount.Value,
                                      ResidentId = tLogs.ResidentId,
                                      TransactionCode = tLogs.Code,
                                      TransactionDate = tLogs.TransactionDate.Value
                                  };
                res.AddRange(transaction.ToList());
            }

            return res;
        }
        #endregion

        #region Audit & Notification
        internal List<Model.Notification> GetAllNotifications()
        {
            var result = from x in db.Notifications
                         join y in db.NotificationTypes on x.TypeId equals y.Id
                         select new Model.Notification
                         {
                             Id = x.Id,
                             Type = y.Type,
                             ReferenceId = x.ReferenceId,
                             Message = x.Message,
                             Recipient = x.Recipient,
                             Retry = x.RetryCount,
                             StatusMessage = x.StatusMessage,
                             Status = x.Status == 0 ? "Pending" : "Delivered"
                   };

            return result.ToList();
        }

        internal List<Model.AuditTrail> GetAllAuditTrails()
        {
            var result = from x in db.AuditTrails
                         join y in db.Clients on x.ClientId equals y.Id into ylist from y in ylist.DefaultIfEmpty()
                         join z in db.Users on x.UserId equals z.Id into zlist from z in zlist.DefaultIfEmpty()
                         
                         select new Model.AuditTrail
                         {
                             Client = y.Name,
                             User = z.Email,
                             Type = x.LogType,
                             TableAffected = x.TableAffected,
                             AuditLog = x.AuditLog,
                             LogDate = x.LogDate
                         };

            return result.ToList();
        }
        #endregion

        #region locations

        internal FindLocationRes FindLocation(int locationId)
        {
            FindLocationRes res = new FindLocationRes();
            var location = db.TerminalLocations.Where(x => x.Id == locationId).Select(x => new Location
            {
                Id = x.Id,
                AgentId = x.AgentId,
                ClientId = x.ClientId,
                LocationCode = x.LocationCode,
                LocationDescription = x.LocationDescription
            });

            res.Location = location.FirstOrDefault();

            return res;
        }

        internal GetAllLocationRes GetAllLocationsByClientId(int clientId)
        {
            GetAllLocationRes res = new GetAllLocationRes();
            var locations = db.TerminalLocations.Where(x => x.ClientId == clientId).Select(x => new Location
            {
                Id = x.Id,
                AgentId = x.AgentId,
                ClientId = x.ClientId,
                LocationCode = x.LocationCode,
                LocationDescription = x.LocationDescription
            });

            res.Locations = locations;

            return res;
        }

        internal bool CreateLocation(Location req, out string msg)
        {
            msg = string.Empty;
            //Check if the Code Exists
            var location = db.TerminalLocations.FirstOrDefault(x => x.LocationCode == req.LocationCode.Trim());
            if (location != null)
            {
                msg = "Location Code Already Exist";
                return false;
            }
            ChamsICSLib.Data.TerminalLocation loc = new ChamsICSLib.Data.TerminalLocation
            {
                AgentId = req.AgentId.HasValue ? req.AgentId.Value : 0,
                ClientId = req.ClientId.HasValue ? req.ClientId.Value : 0,
                LocationCode = req.LocationCode,
                LocationDescription = req.LocationDescription
            };

            db.TerminalLocations.Add(loc);
            int res = db.SaveChanges();

            if(res > 0)
            LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.CREATE, "Location", "LocationId:" + loc.Id,"");

            return res > 0;
        }

        internal bool UpdateLocation(Location req)
        {
            var location = db.TerminalLocations.FirstOrDefault(x => x.Id == req.Id);
            if (location == null)
            {
                return false;
            }

            location.LocationCode = req.LocationCode;
            location.LocationDescription = req.LocationDescription;
            db.TerminalLocations.Attach(location);
            db.Entry(location).State = EntityState.Modified;
            int res = db.SaveChanges();

            if(res > 0)
            LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.EDIT, "Location", "LocationId:" + location.Id, "");

            return res > 0;

        }

        internal GetAllLocationRes GetAllLocationsByAgentId(int agentId)
        {
            GetAllLocationRes res = new GetAllLocationRes();
            var locations = db.TerminalLocations.Where(x=> x.AgentId==agentId).Select(x => new Location
            {
                Id = x.Id,
                AgentId = x.AgentId,
                ClientId = x.ClientId,
                LocationCode = x.LocationCode,
                LocationDescription = x.LocationDescription
            });

            res.Locations = locations;

            return res;
        }

        internal GetAllLocationRes GetAllLocations()
        {
            GetAllLocationRes res = new GetAllLocationRes();
            var locations = db.TerminalLocations.Select(x => new Location
            {
                Id = x.Id,
                AgentId = x.AgentId,
                ClientId = x.ClientId,
                LocationCode = x.LocationCode,
                LocationDescription = x.LocationDescription
            });

            res.Locations = locations;

            return res;
        }

        #endregion

        #region wallet
        internal FindWalletRes GetWallet(string email)
        {
            FindWalletRes res = new FindWalletRes();
            var req = db.Wallets.FirstOrDefault(x => x.Email == email);

            var wallet = new Model.Wallet
            {
                Email = req.Email,
                Amount = req.Amount,
                WalletId = req.WalletId
            };

            if (wallet == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Email";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Wallet = (Model.Wallet)wallet;

            }
            return res;
        }

        internal bool FundWallet(Model.Payment paymentReq)
        {
            bool result = false;
            int changes;

            if (paymentReq.TransactionId == 0 && paymentReq.WalletId == 0)
                result = false;

            ChamsICSLib.Data.Payment payment = new ChamsICSLib.Data.Payment()
            {
                UserId = paymentReq.UserId,
                Amount = Math.Abs(paymentReq.Amount),
                Email = paymentReq.Email,
                Method = paymentReq.Method,
                Reference = paymentReq.Reference,
                Target = paymentReq.Target,
                WalletId = paymentReq.WalletId,
                TransactionId = paymentReq.TransactionId,
                EntryDate = DateTime.Now
            };

            try
            {
                db.Payments.Add(payment);
                changes = db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }


            if (changes > 0)
            {
                var walletEntity = db.Wallets.FirstOrDefault(w => w.Email == paymentReq.Email);
                walletEntity.Amount += paymentReq.Amount;
                db.Entry(walletEntity).State = System.Data.Entity.EntityState.Modified;

                if (db.SaveChanges() <= 0)
                    result = false;

                var paymentEntity = db.Payments.FirstOrDefault(p => p.Email == paymentReq.Email);
                paymentEntity.Status = true;
                var entry = db.Entry(paymentEntity).State = System.Data.Entity.EntityState.Modified;

                result = true;
            }

            return result;
        }

        #endregion

        #region Invoice
        internal GetUserInvoicesRes GetUserInvoices(string email)
        {
            GetUserInvoicesRes res = new GetUserInvoicesRes();
            var invoices = db.Invoices.Where(x => x.Email == email).OrderByDescending(x => x.EntryDate).Select(x => new Model.Invoice
            {
                InvoiceId = x.InvoiceId,
                InvoiceNo = x.InvoiceNo,
                Description = x.Description,
                Email = x.Email,
                Amount = x.Amount,
                RevenueCode = x.RevenueCode,
                EntryDate = x.EntryDate,
                
            });


            res.UserInvoices = invoices;

            return res;
        }

        internal GetUserInvoicesRes GetAllInvoices()
        {
            GetUserInvoicesRes res = new GetUserInvoicesRes();
            var invoices = db.Invoices.OrderByDescending(x => x.EntryDate).Select(x => new Model.Invoice
            {
                InvoiceId = x.InvoiceId,
                InvoiceNo = x.InvoiceNo,
                Description = x.Description,
                Email = x.Email,
                Amount = x.Amount,
                EntryDate = x.EntryDate,
                RevenueCode = x.RevenueCode
            });


            res.UserInvoices = invoices;

            return res;
        }

        internal Model.Invoice GetInvoiceById(int invoiceid)
        {
            Model.Invoice res = new Model.Invoice();
            var invoice = db.Invoices.FirstOrDefault(x => x.InvoiceId == invoiceid);

            if (invoice == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Invoice";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

                res.InvoiceId = invoice.InvoiceId;
                res.InvoiceNo = invoice.InvoiceNo.ToString();
                res.Description = invoice.Description;
                res.Amount = invoice.Amount;
                res.RevenueCode = invoice.RevenueCode;
            }
            return res;
        }

        internal GetInvoicePaymentRes GetTransactionDetails(int id, string email)
        {
            GetInvoicePaymentRes res = new GetInvoicePaymentRes();

            List<Model.Payment> payments = db.Payments.Where(x => x.Email == email && x.TransactionId == id).OrderByDescending(x => x.EntryDate).Select(x => new Model.Payment
            {
                Reference = x.Reference,
                TransactionId = x.TransactionId,
                Target = x.Target,
                Amount = x.Amount
            }).ToList();


            res.Payments = payments;

            return res;

        }

        internal GetInvoicePaymentRes GetAllTransactionDetailsByUser(string email)
        {
            GetInvoicePaymentRes res = new GetInvoicePaymentRes();

            List<Model.Payment> payments = db.Payments.Where(x => x.Email == email).OrderByDescending(x => x.EntryDate).Select(x => new Model.Payment
            {
                Reference = x.Reference,
                TransactionId = x.TransactionId,
                Target = x.Target,
                Method = x.Method,                
                Amount = x.Amount,
                EntryDate = x.EntryDate
            }).ToList();


            res.Payments = payments;

            return res;

        }

        internal bool MakePayment(Model.PaymentReq payReq, out string msg)
        {
            msg = string.Empty;

            if (payReq.Amount <= 0)
            {
                msg = "Invalid Amount";
                return false;
            }
                        
            ChamsICSLib.Data.Payment payment = new ChamsICSLib.Data.Payment()
            {
                Amount = payReq.Amount,
                Email = payReq.Email,
                Reference = payReq.Reference,
                Target = payReq.Target,
                EntryDate = DateTime.Now,
                Method = payReq.Method,
                TransactionId = payReq.TransactionId,
                UserId = payReq.UserId,
                WalletId = payReq.WalletId,
                
            };

            db.Payments.Add(payment);
            int res = db.SaveChanges();


            //TransactionLog transact_log = new TransactionLog();
            //transact_log.Amount = payReq.Amount;
            //transact_log.Email = payReq.Email;
            //transact_log.RevenueCode = invoice.InvoiceNo;

           

            ////Log Audit Trail
            //LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.CREATE, "Client", "ClientId:" + ClientId, "");

            return res > 0;
        }
        #endregion

        #region Taxpayer
        internal TaxpayersRes GetAllTaxpayers()
        {
            TaxpayersRes res = new TaxpayersRes();

            var taxpayer = (from us in db.Users
                         join bd in db.Biodatas on us.Id equals bd.Userid
                         where us.RoleId == 7
                         select new Model.Biodata
                         {
                             BiodataId = bd.Biodataid,
                             Email = us.Email,
                             Firstname = bd.Firstname,
                             Surname = bd.Surname,
                             RIN = bd.RIN,
                             UserId = bd.Userid,
                             EntryDate = bd.EntryDate,
                         }).OrderByDescending(x => x.EntryDate).ToList();


            res.Taxpayers = taxpayer;

            return res;
        }

        internal bool AddTaxpayer(Taxpayer taxpayer)
        {
            try
            {
                //hash pwd
                System.Security.Cryptography.MD5 md5Hash = System.Security.Cryptography.MD5.Create();

                //save user details 
                ChamsICSLib.Data.User user = new ChamsICSLib.Data.User()
                {
                    Email = taxpayer.Email,
                    ClientId = 3,
                    Mobile = taxpayer.Mobile.ToString(),
                    Password = Utils.GetMd5Hash(md5Hash, taxpayer.Password),
                    RoleId = taxpayer.RoleId,
                };
                db.Users.Add(user);
                db.SaveChanges();
                var UserId = user.Id;

                //save biodata
                ChamsICSLib.Data.Biodata biodata = new ChamsICSLib.Data.Biodata()
                {
                    Email = taxpayer.Email,
                    EntryDate = DateTime.Now,
                    Firstname = taxpayer.Firstname,
                    Surname = taxpayer.Surname,
                    RIN = taxpayer.RIN,
                    Userid = UserId,
                };

                db.Biodatas.Add(biodata);
                db.SaveChanges();

                //create wzllet for taxpayer
                ChamsICSLib.Data.Wallet wallet = new ChamsICSLib.Data.Wallet()
                {
                    Email = taxpayer.Email,
                    EntryDate = DateTime.Now,
                    Amount = 0,
                    LastUpdated = DateTime.Now,
                };

                db.Wallets.Add(wallet);
                db.SaveChanges();


                return true;
            }
            catch {
                return false;
            }
            

            ////Log Audit Trail
            //LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.CREATE, "Client", "ClientId:" + ClientId, "");
        }

        internal Model.Taxpayer GetTaxpayerById(int userid)
        {
            Model.Taxpayer res = new Model.Taxpayer();
            var biodata = db.Biodatas.FirstOrDefault(x => x.Userid == userid);

            if (biodata == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Invoice";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

                res.BiodataId = biodata.Biodataid;
                res.Email = biodata.Email;
                res.EntryDate = biodata.EntryDate;
                res.Firstname = biodata.Firstname;
                res.Surname = biodata.Surname;
                res.UserId = biodata.Userid;
                res.RIN = biodata.RIN;
            }
            return res;
        }

        internal AssessmentModelRes GetAssessmentByRole(AssessmentReq assessmentReq)
        {
            AssessmentModelRes res = new AssessmentModelRes();

            //get all revenue items
            var revenueItems = db.RevenueItems.Where(x => x.ClientId == assessmentReq.roleid).Select(x => new Model.RevenueItem
            {
                Amount = (decimal)x.Amount,
                Code = x.Code,
                Name = x.Name,                
            }).ToList();

            //get already generated invoice for taxpayer
            var invoices = GetUserInvoices(assessmentReq.email).UserInvoices.Select(x => x.RevenueCode).ToList();
            //clone revenueItems
            Model.RevenueItem[] revenueCopy = new Model.RevenueItem[revenueItems.Count];
            revenueItems.CopyTo(revenueCopy);

            foreach (var item in revenueCopy.ToList())
            {
                if(invoices.Contains(item.Code))
                {
                    revenueItems.Remove(item);
                }
            }


            res.RevenueItems = revenueItems;

            return res;
        }

        internal bool GenerateInvoice(GenerateInvoice generateInvoice)
        {
            try
            {
                //get revenue item details 
                var revenueItem = db.RevenueItems.FirstOrDefault(x => x.Code == generateInvoice.RevenueCode);

                //get taxpayer details 
                var taxpayer = db.Users.FirstOrDefault(x => x.Id == generateInvoice.TaxpayerId);

                //save invoice
                ChamsICSLib.Data.Invoice invoice = new ChamsICSLib.Data.Invoice()
                {
                    Amount = revenueItem.Amount.HasValue ? revenueItem.Amount.Value : 0,
                    Description = revenueItem.Name,
                    Email = taxpayer.Email,
                    EntryDate = DateTime.Now,
                    RevenueCode = revenueItem.Code,                    
                };

                //generate invoice no ##random 8-digit number
                Random rand = new Random();
                invoice.InvoiceNo = rand.Next(11111111, 99999999).ToString();

                db.Invoices.Add(invoice);
                db.SaveChanges();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }


            ////Log Audit Trail
            //LogAuditData(req.AuditTrailData.clientId.Value, req.AuditTrailData.userId.Value, AuditTrailType.CREATE, "Client", "ClientId:" + ClientId, "");
        }

        #endregion

    }
}