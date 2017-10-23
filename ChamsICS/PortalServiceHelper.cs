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

namespace ChamsICSWebService
{
    public partial class ServiceHelper
    {

        internal bool CreateClient(CreateClientReq req, out string ClientId, out string ClientCode, out string msg)
        {
            bool result = true;
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
            client.Status = 1;
            db.Clients.Add(client);
            db.SaveChanges();

            ClientCode = client.Code;
            ClientId = client.Id.ToString();

            result = true;

            return result;
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
                res.Addres = client.Address;
                res.Phone1 = client.Phone1;
                res.Phone2 = client.Phone2;
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
                status = x.Status.ToString()
            });


            res.Clients = clients;

            return res;
        }

        internal FindTerminalRes FindTerminal(int id)
        {
            FindTerminalRes res = new FindTerminalRes();
            var terminalObj = db.Terminals.FirstOrDefault(x => x.Id == id);
            
            var terminal= new Model.Terminal
             {
                 Id = terminalObj.Id.ToString(),
                 AgentId = terminalObj.AgentId.ToString(),
                 Code = terminalObj.Code,
                 Name=terminalObj.Name,
                 SerialNumber = terminalObj.SerialNumber
             };

            if (terminal == null)
            {
                res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                res.ResponseDescription = "Invalid Id";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";
                res.Terminal = (Model.Terminal)terminal;

            }
            return res;
        }

        internal GetAllTerminalRes GetAllTerminals()
        {
            GetAllTerminalRes res = new GetAllTerminalRes();

            var terminal = db.Terminals.Select(x => new Model.Terminal
            {
                Id = x.Id.ToString(),
                AgentId = x.AgentId.ToString(),
                Name=x.Name,
                Code = x.Code,
                SerialNumber = x.SerialNumber
            });


            res.Terminals = terminal.ToList();

            return res;
        }

        internal GetAllTerminalRes GetAllTerminalsByAgentId(int agentId)
        {
            GetAllTerminalRes res = new GetAllTerminalRes();

            var terminal = db.Terminals.Where(x => x.AgentId == agentId).Select(x => new Model.Terminal
            {
                Id = x.Id.ToString(),
                AgentId = x.AgentId.ToString(),
                Code = x.Code,
                SerialNumber = x.SerialNumber
            });


            res.Terminals = terminal.ToList();

            return res;
        }

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
                agent.ClientId = Convert.ToInt32(req.ClientId);
                agent.Code = newAgentCode;
                agent.Name = req.Name;
                agent.Email = req.Email;
                agent.Address = req.Address;
                agent.Phone1 = req.Phone1;
                agent.Phone2 = req.Phone2;
                agent.Status = 1;

                db.Agents.Add(agent);
                db.SaveChanges();

                //Push out for Returned Result
                agentCode = agent.Code;
                agentId = agent.Id.ToString();

                result = true;
            }
            return result;
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
                 Id = agentObj.Id.ToString(),
                 ClientId = agentObj.ClientId.ToString(),
                 Code = agentObj.Code,
                 Company =agentObj.Company,
                 Name =agentObj.Name,
                 Address = agentObj.Address,
                 Email = agentObj.Email,
                 Phone1 = agentObj.Phone1,
                 Phone2 = agentObj.Phone2,
                 status = agentObj.Status.ToString()
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
                Id = x.Id.ToString(),
                ClientId = x.ClientId.ToString(),
                Code = x.Code,
                Company = x.Company,
                Name = x.Name,
                Address = x.Address,
                Email = x.Email,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                status = x.Status.ToString()
            });


            res.Agents = agent;

            return res;
        }

        internal GetAllAgentRes GetAllAgentByClientId(int clientId)
        {
            GetAllAgentRes res = new GetAllAgentRes();
            var agent = db.Agents.Where(x => x.ClientId == clientId).Select(x => new Model.Agent
            {
                Id = x.Id.ToString(),
                ClientId = x.ClientId.ToString(),
                Code = x.Code,
                Company = x.Company,
                Name = x.Name,
                Address = x.Address,
                Email = x.Email,
                Phone1 = x.Phone1,
                Phone2 = x.Phone2,
                status = x.Status.ToString()
            });


            res.Agents = agent;

            return res;
        }

        internal FindTransactionRes FindTransaction(int id)
        {
            FindTransactionRes res = new FindTransactionRes();
            var tLogsObj = db.TransactionLogs.FirstOrDefault(x => x.Id == id);
            
           var  tLogs=new Model.Transaction
             {
                 Id = tLogsObj.Id.ToString(),
                 AgentId = tLogsObj.AgentId.ToString(),
                 TerminalId = tLogsObj.TerminalId.ToString(),
                 TransactionCode = tLogsObj.Code,
                 ResidentId = tLogsObj.ResidentId,
                 FirstName = tLogsObj.FirstName,
                 MiddleName = tLogsObj.MiddleName,
                 LastName = tLogsObj.Lastname,
                 Address = tLogsObj.Address,
                 Email = tLogsObj.Email,
                 PhoneNumber = tLogsObj.PhoneNumber,
                 DateOfBirth = tLogsObj.DateOfBirth.ToString(),
                 Gender = tLogsObj.Gender,
                 RevenueCode = tLogsObj.RevenueCode,
                 Amount = tLogsObj.Amount.ToString(),
                 PaymentReference = tLogsObj.PaymentReference,
                 TransactionDate = tLogsObj.TransactionDate.ToString(),
                 UploadDate = tLogsObj.UploadDate.ToString(),
                 status = tLogsObj.Status.ToString()
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

            var tLogs = new Model.Transaction
            {
                Id = tLogsObj.Id.ToString(),
                AgentId = tLogsObj.AgentId.ToString(),
                TerminalId = tLogsObj.TerminalId.ToString(),
                TransactionCode = tLogsObj.Code,
                ResidentId = tLogsObj.ResidentId,
                FirstName = tLogsObj.FirstName,
                MiddleName = tLogsObj.MiddleName,
                LastName = tLogsObj.Lastname,
                Address = tLogsObj.Address,
                Email = tLogsObj.Email,
                PhoneNumber = tLogsObj.PhoneNumber,
                DateOfBirth = tLogsObj.DateOfBirth.ToString(),
                Gender = tLogsObj.Gender,
                RevenueCode = tLogsObj.RevenueCode,
                Amount = tLogsObj.Amount.ToString(),
                PaymentReference = tLogsObj.PaymentReference,
                TransactionDate = tLogsObj.TransactionDate.ToString(),
                UploadDate = tLogsObj.UploadDate.ToString(),
                status = tLogsObj.Status.ToString()
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

        internal GetAllTransactionRes GetAllTransactions()
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            var tLogs = db.TransactionLogs.Select(x => new Model.Transaction
            {
                Id = x.Id.ToString(),
                AgentId = x.AgentId.ToString(),
                TerminalId = x.TerminalId.ToString(),
                TransactionCode = x.Code,
                ResidentId = x.ResidentId,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.Lastname,
                Address = x.Address,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                DateOfBirth = x.DateOfBirth.ToString(),
                Gender = x.Gender,
                RevenueCode = x.RevenueCode,
                Amount = x.Amount.ToString(),
                PaymentReference = x.PaymentReference,
                TransactionDate = x.TransactionDate.ToString(),
                UploadDate = x.UploadDate.ToString(),
                status = x.Status.ToString()
            });

            res.Transactions = tLogs;

            return res;
        }

        internal GetAllTransactionRes GetAllTransactionsByClientId(int clientId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            var tLogs = db.TransactionLogs.Where(x => x.ClientId == clientId).Select(x => new Model.Transaction
            {
                Id = x.Id.ToString(),
                AgentId = x.AgentId.ToString(),
                TerminalId = x.TerminalId.ToString(),
                TransactionCode = x.Code,
                ResidentId = x.ResidentId,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.Lastname,
                Address = x.Address,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                DateOfBirth = x.DateOfBirth.ToString(),
                Gender = x.Gender,
                RevenueCode = x.RevenueCode,
                Amount = x.Amount.ToString(),
                PaymentReference = x.PaymentReference,
                TransactionDate = x.TransactionDate.ToString(),
                UploadDate = x.UploadDate.ToString(),
                status = x.Status.ToString()
            });

            res.Transactions = tLogs;

            return res;
        }

        internal GetAllTransactionRes GetAllTransactionsByAgentId(int agentId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            var tLogs = db.TransactionLogs.Where(x => x.AgentId == agentId).Select(x => new Model.Transaction
            {
                Id = x.Id.ToString(),
                AgentId = x.AgentId.ToString(),
                TerminalId = x.TerminalId.ToString(),
                TransactionCode = x.Code,
                ResidentId = x.ResidentId,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.Lastname,
                Address = x.Address,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                DateOfBirth = x.DateOfBirth.ToString(),
                Gender = x.Gender,
                RevenueCode = x.RevenueCode,
                Amount = x.Amount.ToString(),
                PaymentReference = x.PaymentReference,
                TransactionDate = x.TransactionDate.ToString(),
                UploadDate = x.UploadDate.ToString(),
                status = x.Status.ToString()
            });

            res.Transactions = tLogs;

            return res;
        }

        internal GetAllTransactionRes GetAllTransactionsByTerminalId(int terminalId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            var tLogs = db.TransactionLogs.Where(x => x.TerminalId == terminalId).Select(x => new Model.Transaction
            {
                Id = x.Id.ToString(),
                AgentId = x.AgentId.ToString(),
                TerminalId = x.TerminalId.ToString(),
                TransactionCode = x.Code,
                ResidentId = x.ResidentId,
                FirstName = x.FirstName,
                MiddleName = x.MiddleName,
                LastName = x.Lastname,
                Address = x.Address,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                DateOfBirth = x.DateOfBirth.ToString(),
                Gender = x.Gender,
                RevenueCode = x.RevenueCode,
                Amount = x.Amount.ToString(),
                PaymentReference = x.PaymentReference,
                TransactionDate = x.TransactionDate.ToString(),
                UploadDate = x.UploadDate.ToString(),
                status = x.Status.ToString()
            });


            var test = tLogs.ToList();
            res.Transactions = tLogs;

            return res;
        }

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
            db.SaveChanges();

            revenueId = revenue.Id;

            return result;

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

        internal GetAllRevenueRes GetAllRevenueByClientId(int clientId)
        {
            GetAllRevenueRes res = new GetAllRevenueRes();
            var revenue = db.Revenues.Where(x=> x.ClientId==clientId).Select(revenueObj => new Model.Revenue
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
    }
}