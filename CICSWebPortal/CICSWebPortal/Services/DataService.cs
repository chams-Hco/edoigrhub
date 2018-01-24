using CICSWebPortal.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CICSWebPortal.ServiceReference2;
using CICSWebPortal.ViewModels;
using System.Web.Helpers;
using CICSWebPortal.Helpers;
using System.Data;

namespace CICSWebPortal.Services
{
    public class DataService : IDataService
    {
        //private static IEnumerable<Models.Report> _reportToExport = null;
        iChamsICSPortalServiceClient _client = null;

        public DataService()
        {
            _client = new iChamsICSPortalServiceClient("BasicHttpBinding_iChamsICSPortalService");

        }

        #region Clients
        public IList<Models.Client> GetAllClients()
        {
            return _client.GetAllClient().Clients.Select(e => new Models.Client { ClientId = Convert.ToInt32(e.ClientId),
                ClientName = e.Name, Email = e.Email, PhoneNo1 = e.Phone1, Address = e.Addres, ClientCode = e.Code,
                PhoneNo2 = e.Phone2, Status = e.status == 1 ? true : false
            }).ToList();
        }

        public Models.Client FindClientById(int id)
        {
            var client = _client.FindClient(id);

            return new Models.Client
            {
                ClientId = Convert.ToInt32(client.ClientId),
                Address = client.Addres,
                ClientName = client.Name,
                ClientCode = client.Code,
                Email = client.Email,
                PhoneNo1 = client.Phone1,
                PhoneNo2 = client.Phone2,
                Status = client.status == 1 ? true : false
            };
        }

        public void AddClient(Models.Client client)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = client.userId,
                clientId = client.clientId
            };

            CreateClientReq clientReq = new CreateClientReq();
            clientReq.Name = client.ClientName;
            clientReq.Addres = client.Address;
            clientReq.Email = client.Email;
            clientReq.Phone1 = client.PhoneNo1;
            clientReq.Phone2 = client.PhoneNo2;
            clientReq.Status = client.Status == true ? 1 : 0;
            clientReq.AuditTrailData = _AuditTrailData;

            var result = _client.CreateClient(clientReq);

            if (result.ResponseCode != "0000")
            {

            }
        }

        public void UpdateClient(Models.Client client)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = client.userId,
                clientId = client.clientId
            };
            UpdateClientReq clientReq = new UpdateClientReq();
            clientReq.ClientId = client.ClientId;
            clientReq.ClientCode = client.ClientCode;
            clientReq.Name = client.ClientName;
            clientReq.Address = client.Address;
            clientReq.Email = client.Email;
            clientReq.Phone1 = client.PhoneNo1;
            clientReq.Phone2 = client.PhoneNo2;
            clientReq.Status = client.Status == true ? 1 : 0;
            clientReq.AuditTrailData = _AuditTrailData;

            var result = _client.UpdateClient(clientReq);

            if (result.ResponseCode != "0000")
            {

            }
        }

        public void DeleteClient(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Agents
        public IList<Models.Agent> GetAllAgents()
        {
            return _client.GetAllAgents().Agents.Select(e => new Models.Agent
            {
                AgentId = Convert.ToInt32(e.Id),
                ClientId = Convert.ToInt32(e.ClientId),
                Code = e.Code,
                Name = e.Name,
                Company = e.Company,
                Email = e.Email,
                Address = e.Address,
                PhoneNo1 = e.Phone1,
                PhoneNo2 = e.Phone2,
                Status = e.status == 1 ? true : false
            }).ToList();
        }

        public IList<Models.Agent> GetAllAgentsByClientId(int id)
        {
            return _client.GetAllAgentsByClientId(id).Agents.Select(e => new Models.Agent
            {
                AgentId = Convert.ToInt32(e.Id),
                ClientId = Convert.ToInt32(e.ClientId),
                Code = e.Code,
                Name = e.Name,
                Company = e.Company,
                Email = e.Email,
                Address = e.Address,
                PhoneNo1 = e.Phone1,
                PhoneNo2 = e.Phone2,
                Status = e.status == 1 ? true : false
            }).ToList();
        }

        public CICSWebPortal.Models.Agent FindAgentById(int id)
        {
            var agent = _client.FindAgent(id);

            return new Models.Agent
            {
                AgentId = Convert.ToInt32(agent.Agent.Id),
                ClientId = Convert.ToInt32(agent.Agent.ClientId), Code = agent.Agent.Code, Company = agent.Agent.Company, Name = agent.Agent.Name,
                Email = agent.Agent.Email, Address = agent.Agent.Address, PhoneNo1 = agent.Agent.Phone1, PhoneNo2 = agent.Agent.Phone2,
                Status = agent.Agent.status == 1 ? true : false
            };
        }

        public void AddAgent(Models.Agent agent)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = agent.userId,
                clientId = agent.clientId
            };
            ServiceReference2.Agent agentReq = new ServiceReference2.Agent();
            agentReq.ClientId = agent.ClientId;
            agentReq.Name = agent.Name;
            agentReq.Company = agent.Company;
            agentReq.Address = agent.Address;
            agentReq.Email = agent.Email;
            agentReq.Phone1 = agent.PhoneNo1;
            agentReq.Phone2 = agent.PhoneNo2;
            agentReq.status = agent.Status == true ? 1 : 0;
            agentReq.AuditTrailData = _AuditTrailData;

            var result = _client.CreateAgent(agentReq);

            if (result.ResponseCode != "0000")
            {

            }
        }

        public void UpdateAgent(Models.Agent agent)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = agent.userId,
                clientId = agent.clientId
            };
            ServiceReference2.Agent agentReq = new ServiceReference2.Agent();
            agentReq.Id = agent.AgentId;
            agentReq.ClientId = agent.ClientId;
            agentReq.Name = agent.Name;
            agentReq.Company = agent.Company;
            agentReq.Address = agent.Address;
            agentReq.Email = agent.Email;
            agentReq.Phone1 = agent.PhoneNo1;
            agentReq.Phone2 = agent.PhoneNo2;
            agentReq.status = agent.Status == true ? 1 : 0;
            agentReq.AuditTrailData = _AuditTrailData;

            var result = _client.UpdateAgent(agentReq);

            if (result.ResponseCode != "0000")
            {

            }
        }

        public void DeleteAgent(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Terminals
        public IList<CICSWebPortal.Models.Terminal> GetAllTerminals()
        {
            return _client.GetAllTerminals().Terminals.Select(e => new Models.Terminal
            {
                TerminalId = e.TerminalId,
                AgentName = e.AgentName,
                AgentId = e.AgentId,
                Code = e.Code,
                Name = e.Name,
                SerialNumber = e.SerialNumber,
                status = e.status == 1 ? true : false
            }).ToList();
        }

        public Models.Terminal FindTerminalById(int id)
        {
            var terminal = _client.FindTerminal(id);
            return new Models.Terminal { TerminalId = terminal.Terminal.TerminalId, AgentName = terminal.Terminal.AgentName,
                Code = terminal.Terminal.Code, SerialNumber = terminal.Terminal.SerialNumber, status = terminal.Terminal.status == 1 ? true : false };
        }

        public IList<Models.Terminal> GetAllTerminalsByAgentId(int id)
        {

            return _client.GetAllTerminalsByAgentId(id).Terminals.Select(e => new Models.Terminal
            {
                TerminalId = e.TerminalId,
                AgentName = e.AgentName,
                Code = e.Code,
                Name = e.Name,
                SerialNumber = e.SerialNumber,
                status = e.status == 1 ? true : false
            }).ToList();
        }

        public IList<Models.Terminal> GetAllTerminalsByClientId(int id)
        {

            return _client.GetAllTerminalsByClientId(id).Terminals.Select(e => new Models.Terminal
            {
                TerminalId = e.TerminalId,
                AgentName = e.AgentName,
                Code = e.Code,
                Name = e.Name,
                SerialNumber = e.SerialNumber,
                status = e.status == 1 ? true : false
            }).ToList();
        }

        #endregion

        #region Transactions
        public Models.Transaction FindTransactionById(int id)
        {
            var e = _client.FindTransaction(id).Transaction;
            return new Models.Transaction()
            {
                ResidentId = e.ResidentId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Email = e.Email,
                Gender = e.Gender,
                DateOfBirth = e.DateOfBirth,
                PhoneNumber = e.PhoneNumber,
                Status = e.status,
                TerminalCode = e.TerminalCode,
                Amount = Convert.ToDecimal(e.Amount),
                Address = e.Address,
                UploadDate = e.UploadDate,
                PaymentReference = e.PaymentReference,
                TransactionDate = e.TransactionDate,
                TransactionCode = e.TransactionCode,
                RevenueCode = e.RevenueCode,
                RevenueName = e.RevenueName,
                Ministry = e.Ministry,
                RevenueHead = e.RevenueHead,
                TransactionId = Convert.ToInt32(e.Id),
                AgentId = Convert.ToInt32(e.AgentId),
                AgentName = e.AgentName,
                ClientId = Convert.ToInt32(e.ClientId),
                TerminalId = Convert.ToInt32(e.TerminalId)
            };
        }

        public Models.Transaction FindTransactionByCode(string code)
        {
            var e = _client.FindTransactionByCode(code).Transaction;
            if (e != null)
            {
                return new Models.Transaction()
                {
                    ResidentId = e.ResidentId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    Email = e.Email,
                    Gender = e.Gender,
                    DateOfBirth = e.DateOfBirth,
                    PhoneNumber = e.PhoneNumber,
                    Status = e.status,
                    TerminalCode = e.TerminalCode,
                    Amount = Convert.ToDecimal(e.Amount),
                    Address = e.Address,
                    UploadDate = e.UploadDate,
                    PaymentReference = e.PaymentReference,
                    TransactionDate = e.TransactionDate,
                    TransactionCode = e.TransactionCode,
                    RevenueCode = e.RevenueCode,
                    RevenueName = e.RevenueName,
                    Ministry = e.Ministry,
                    RevenueHead = e.RevenueHead,
                    TransactionId = Convert.ToInt32(e.Id),
                    AgentId = Convert.ToInt32(e.AgentId),
                    AgentName = e.AgentName,
                    ClientId = Convert.ToInt32(e.ClientId),
                    TerminalId = Convert.ToInt32(e.TerminalId)
                };
            }
            else { return new Models.Transaction() { }; }
        }

        public IList<Models.Transaction> FindTransactionByResidentId(string code)
        {
            return _client.GetAllTransactionByResidentId(code).Transactions.Select(e => new Models.Transaction
            {
                ResidentId = e.ResidentId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Email = e.Email,
                Gender = e.Gender,
                DateOfBirth = e.DateOfBirth,
                PhoneNumber = e.PhoneNumber,
                Status = e.status,
                TerminalCode = e.TerminalCode,
                Amount = Convert.ToDecimal(e.Amount),
                Address = e.Address,
                UploadDate = e.UploadDate,
                PaymentReference = e.PaymentReference,
                TransactionDate = e.TransactionDate,
                TransactionCode = e.TransactionCode,
                RevenueCode = e.RevenueCode,
                RevenueName = e.RevenueName,
                Ministry = e.Ministry,
                RevenueHead = e.RevenueHead,
                TransactionId = Convert.ToInt32(e.Id),
                AgentId = Convert.ToInt32(e.AgentId),
                AgentName = e.AgentName,
                ClientId = Convert.ToInt32(e.ClientId),
                TerminalId = Convert.ToInt32(e.TerminalId)
            }).ToList();
        }

        public IList<Models.Transaction> GetAllTransactions(Models.GetTransactionRequest req)
        {
            var result = _client.GetTransactions(new ServiceReference2.GetTransactionRequest
            {
                UserType = req.UserType,
                UserTypeId = req.UserTypeId,
                RequireLimit = req.RequireLimit,
                RequireDateFilter = req.RequireDateFilter,
                Limit = req.Limit,
                EndtDate = req.EndtDate,
                StartDate = req.StartDate
            });
            
            return result.Transactions.Select(e => new Models.Transaction
            {
                ResidentId = e.ResidentId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Email = e.Email,
                Gender = e.Gender,
                DateOfBirth = e.DateOfBirth,
                PhoneNumber = e.PhoneNumber,
                Status = e.status,
                TerminalCode = e.TerminalCode,
                Amount = Convert.ToDecimal(e.Amount),
                Address = e.Address,
                UploadDate = e.UploadDate,
                PaymentReference = e.PaymentReference,
                TransactionDate = e.TransactionDate,
                TransactionCode = e.TransactionCode,
                RevenueCode = e.RevenueCode,
                RevenueName = e.RevenueName,
                Ministry = e.Ministry,
                RevenueHead = e.RevenueHead,
                TransactionId = Convert.ToInt32(e.Id),
                AgentId = Convert.ToInt32(e.AgentId),
                AgentName = e.AgentName,
                LocationName = e.LocationName,
                ClientId = Convert.ToInt32(e.ClientId),
                TerminalId = Convert.ToInt32(e.TerminalId)
            }).OrderByDescending(x=> x.TransactionDate).ToList();
        }
        #endregion

        #region Error Upload Transactions
        public Models.ErrorTransaction FindErrorTransactionById(int id)
        {
            try
            {
                var e = _client.FindErrorTransaction(id).Transaction;
                if (e != null)
                {
                    return new Models.ErrorTransaction()
                    {
                        ResidentId = e.ResidentId,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        MiddleName = e.MiddleName,
                        Email = e.Email,
                        Gender = e.Gender,
                        DateOfBirth = e.DateOfBirth ?? null,
                        PhoneNumber = e.PhoneNumber,
                        Status = e.status,
                        TerminalCode = e.TerminalCode,
                        Amount = e.Amount.Value,
                        Address = e.Address,
                        UploadDate = e.UploadDate,
                        PaymentReference = e.PaymentReference,
                        TransactionDate = e.TransactionDate ?? null,
                        TransactionCode = e.TransactionCode,
                        RevenueCode = e.RevenueCode,
                        RevenueName = e.RevenueName,
                        Ministry = e.Ministry,
                        RevenueHead = e.RevenueHead,
                        TransactionId = e.Id,
                        AgentId = e.AgentId,
                        AgentName = e.AgentName,
                        ClientId = e.ClientId,
                        TerminalId = e.TerminalId,

                        UploadError = e.UploadError,
                        ResolutionDate = e.ResolutionDate ?? null,
                        ResolutionError = e.ResolutionError,
                        ResolutionStatus = e.ResolutionStatus
                    };

                }
                else {
                    return new Models.ErrorTransaction { };
                }
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        public Models.ErrorTransaction FindErrorTransactionByCode(string code)
        {
            var e = _client.FindErrorTransactionByCode(code).Transaction;
            if (e != null)
            {
                return new Models.ErrorTransaction()
                {
                    ResidentId = e.ResidentId,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    MiddleName = e.MiddleName,
                    Email = e.Email,
                    Gender = e.Gender,
                    DateOfBirth = e.DateOfBirth ?? null,
                    PhoneNumber = e.PhoneNumber,
                    Status = e.status,
                    TerminalCode = e.TerminalCode,
                    Amount = e.Amount.Value,
                    Address = e.Address,
                    UploadDate = e.UploadDate,
                    PaymentReference = e.PaymentReference,
                    TransactionDate = e.TransactionDate ?? null,
                    TransactionCode = e.TransactionCode,
                    RevenueCode = e.RevenueCode,
                    RevenueName = e.RevenueName,
                    Ministry = e.Ministry,
                    RevenueHead = e.RevenueHead,
                    TransactionId = e.Id,
                    AgentId = e.AgentId,
                    AgentName = e.AgentName,
                    ClientId = e.ClientId,
                    TerminalId = e.TerminalId,

                    UploadError = e.UploadError,
                    ResolutionDate = e.ResolutionDate ?? null,
                    ResolutionError = e.ResolutionError,
                    ResolutionStatus = e.ResolutionStatus
                };
            }
            else { return new Models.ErrorTransaction() { }; }
        }

        public IList<Models.ErrorTransaction> GetAllErrorTransactions(Models.GetTransactionRequest req)
        {
            try
            {
                var result = _client.GetErrorTransaction(new ServiceReference2.GetTransactionRequest
                {
                    UserType = req.UserType,
                    UserTypeId = req.UserTypeId,
                    RequireLimit = req.RequireLimit,
                    RequireDateFilter = req.RequireDateFilter,
                    Limit = req.Limit,
                    EndtDate = req.EndtDate,
                    StartDate = req.StartDate
                });
                if (result.Transactions != null)
                {
                    return result.Transactions.Select(e => new Models.ErrorTransaction
                    {
                        ResidentId = e.ResidentId,
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        MiddleName = e.MiddleName,
                        Email = e.Email,
                        Gender = e.Gender,
                        DateOfBirth = e.DateOfBirth ?? null,
                        PhoneNumber = e.PhoneNumber,
                        Status = e.status,
                        TerminalCode = e.TerminalCode,
                        Amount = e.Amount.Value,
                        Address = e.Address,
                        UploadDate = e.UploadDate,
                        PaymentReference = e.PaymentReference,
                        TransactionDate = e.TransactionDate ?? null,
                        TransactionCode = e.TransactionCode,
                        RevenueCode = e.RevenueCode,
                        RevenueName = e.RevenueName,
                        Ministry = e.Ministry,
                        RevenueHead = e.RevenueHead,
                        TransactionId = e.Id,
                        AgentId = e.AgentId,
                        AgentName = e.AgentName,
                        ClientId = e.ClientId,
                        TerminalId = e.TerminalId,

                        UploadError = e.UploadError,
                        ResolutionDate = e.ResolutionDate ?? null,
                        ResolutionError = e.ResolutionError,
                        ResolutionStatus = e.ResolutionStatus
                    }).ToList();
                }
                else { return null; }
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        public IList<Models.ErrorTransaction> GetAllErrorTransactionByTerminalId(int id)
        {
            return _client.GetAllErrorTransactionByTerminalId(id).Transactions.Select(e => new Models.ErrorTransaction
            {
                ResidentId = e.ResidentId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                MiddleName = e.MiddleName,
                Email = e.Email,
                Gender = e.Gender,
                DateOfBirth = e.DateOfBirth ?? null,
                PhoneNumber = e.PhoneNumber,
                Status = e.status,
                TerminalCode = e.TerminalCode,
                Amount = e.Amount.Value,
                Address = e.Address,
                UploadDate = e.UploadDate,
                PaymentReference = e.PaymentReference,
                TransactionDate = e.TransactionDate ?? null,
                TransactionCode = e.TransactionCode,
                RevenueCode = e.RevenueCode,
                RevenueName = e.RevenueName,
                Ministry = e.Ministry,
                RevenueHead = e.RevenueHead,
                TransactionId = e.Id,
                AgentId = e.AgentId,
                AgentName = e.AgentName,
                ClientId = e.ClientId,
                TerminalId = e.TerminalId,

                UploadError = e.UploadError,
                ResolutionDate = e.ResolutionDate ?? null,
                ResolutionError = e.ResolutionError,
                ResolutionStatus = e.ResolutionStatus
            }).ToList();
        }
        #endregion

        #region Revenue
        public IList<Models.Revenue> GetAllRevenues()
        {

            return _client.GetAllRevenues().Revenues.Select(e => new Models.Revenue
            {
                RevenueId = e.RevenueId,
                ClientId = e.ClientId,
                Code = e.RevenueCode,
                Name = e.Name,
                Amount = e.Amount,
                MDA = e.MDA,
                Status = e.Status == 1 ? true : false

            }).ToList();
        }

        public CICSWebPortal.Models.Revenue FindRevenueById(int id)
        {
            var e = _client.FindRevenue(id);
            return new Models.Revenue
            {
                RevenueId = e.Revenue.RevenueId,
                ClientId = e.Revenue.ClientId,
                Code = e.Revenue.RevenueCode,
                Name = e.Revenue.Name,
                Amount = e.Revenue.Amount,
                MDA = e.Revenue.MDA,
                Status = e.Revenue.Status == 1 ? true : false
            };
        }

        public void AddRevenue(CICSWebPortal.Models.Revenue revenue)
        {
            ServiceReference2.Revenue revenueReq = new ServiceReference2.Revenue()
            {
                RevenueId = revenue.RevenueId,
                ClientId = revenue.ClientId,
                RevenueCode = revenue.Code,
                Name = revenue.Name,
                Amount = revenue.Amount,
                MDA = revenue.MDA,
                Status = revenue.Status == true ? 1 : 0
            };

            var result = _client.CreateRevenue(revenueReq);

            if (result.ResponseCode != "0000")
            {

            }
        }

        public void UpdateRevenue(CICSWebPortal.Models.Revenue revenue)
        {
            ServiceReference2.Revenue revenueReq = new ServiceReference2.Revenue()
            {
                RevenueId = revenue.RevenueId,
                ClientId = revenue.ClientId,
                RevenueCode = revenue.Code,
                Name = revenue.Name,
                Amount = revenue.Amount,
                MDA = revenue.MDA,
                Status = revenue.Status == true ? 1 : 0
            };

            var result = _client.UpdateRevenue(revenueReq);

            if (result.ResponseCode != "0000")
            {

            }
        }

        public void DeleteRevenue(int id)
        {
            throw new NotImplementedException();
        }

        public List<Models.Ministry> GetAllMinistry()
        {
            return _client.GetAllMinistry().Select(x => new Models.Ministry
            {
                Id = x.Id,
                ClientId = x.ClientId,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status == 1 ? true : false

            }).ToList();
        }

        public List<Models.Ministry> GetAllMinistryByClientId(int id)
        {
            return _client.GetAllMinistryByClientId(id).Select(x => new Models.Ministry
            {
                Id = x.Id,
                ClientId = x.ClientId,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status == 1 ? true : false

            }).ToList();
        }

        public List<Models.RevenueHead> GetAllRevenueHead()
        {
            return _client.GetAllRevenueHead().Select(x => new Models.RevenueHead
            {
                Id = x.Id,
                ClientId = x.ClientId,
                MinistryId = x.MinistryId,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status == 1 ? true : false
            }).ToList();
        }

        public List<Models.RevenueHead> GetAllRevenueHeadByClientId(int id)
        {
            return _client.GetAllRevenueHeadByClientId(id).Select(x => new Models.RevenueHead
            {
                Id = x.Id,
                ClientId = x.ClientId,
                MinistryId = x.MinistryId,
                Ministry = x.Ministry,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status == 1 ? true : false
            }).ToList();
        }

        public List<Models.RevenueHead> GetAllRevenueHeadByMinistryId(int id)
        {
            return _client.GetAllRevenueHeadByMinistryId(id).Select(x => new Models.RevenueHead
            {
                Id = x.Id,
                ClientId = x.ClientId,
                MinistryId = x.MinistryId,
                Ministry = x.Ministry,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status == 1 ? true : false
            }).ToList();
        }

        public List<Models.RevenueCategory> GetAllRevenueCategoryByClientId(int id)
        {
            return _client.GetAllRevenueCategoryByClientId(id).Select(x => new Models.RevenueCategory
            {
                Id = x.Id,
                ClientId = x.ClientId,
                MinistryId = x.MinistryId,
                Ministry = x.Ministry,
                RevenueHeadId = x.RevenueHeadId,
                RevenueHead = x.RevenueHead,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status == 1 ? true : false
            }).ToList();
        }

        public List<Models.RevenueCategory> GetAllRevenueCategoryByRevenueHeadId(int id)
        {
            return _client.GetAllRevenueCategoryByRevenueHeadId(id).Select(x => new Models.RevenueCategory
            {
                Id = x.Id,
                ClientId = x.ClientId,
                MinistryId = x.MinistryId,
                Ministry = x.Ministry,
                RevenueHeadId = x.RevenueHeadId,
                RevenueHead = x.RevenueHead,
                Code = x.Code,
                Name = x.Name,
                Status = x.Status == 1 ? true : false
            }).ToList();
        }

        public List<Models.RevenueItem> GetAllRevenueItemByClientId(int id)
        {
            return _client.GetAllRevenueItemByClientId(id).Select(x => new Models.RevenueItem
            {
                Id = x.Id,
                ClientId = x.ClientId,
                MinistryId = x.MinistryId,
                Ministry = x.Ministry,
                CategoryId = x.CategoryId,
                Category = x.Category,
                RevenueHeadId = x.RevenueHeadId,
                RevenueHead = x.RevenueHead,
                Code = x.Code,
                Name = x.Name,
                Amount = x.Amount.Value,
                Status = x.Status == 1 ? true : false
            }).ToList();
        }

        public List<Models.RevenueItem> GetAllRevenueItemByMinistryId(int id)
        {
            return _client.GetAllRevenueItemByMinistryId(id).Select(x => new Models.RevenueItem
            {
                Id = x.Id,
                ClientId = x.ClientId,
                MinistryId = x.MinistryId,
                Ministry = x.Ministry,
                CategoryId = x.CategoryId,
                Category = x.Category,
                RevenueHeadId = x.RevenueHeadId,
                RevenueHead = x.RevenueHead,
                Code = x.Code,
                Name = x.Name,
                Amount = x.Amount.Value,
                Status = x.Status == 1 ? true : false
            }).ToList();
        }

        public List<Models.RevenueItem> GetAllRevenueItemByCategoryId(int id)
        {
            return _client.GetAllRevenueItemByCategoryId(id).Select(x => new Models.RevenueItem
            {
                Id = x.Id,
                ClientId = x.ClientId,
                MinistryId = x.MinistryId,
                Ministry = x.Ministry,
                CategoryId = x.CategoryId,
                Category = x.Category,
                RevenueHeadId = x.RevenueHeadId,
                RevenueHead = x.RevenueHead,
                Code = x.Code,
                Name = x.Name,
                Amount = x.Amount.Value,
                Status = x.Status == 1 ? true : false
            }).ToList();
        }

        public List<Models.RevenueItem> GetAllRevenueItemByRevenueHeadId(int id)
        {
            return _client.GetAllRevenueItemByRevenueHeadId(id).Select(x => new Models.RevenueItem
            {
                Id = x.Id,
                ClientId = x.ClientId,
                MinistryId = x.MinistryId,
                Ministry = x.Ministry,
                CategoryId = x.CategoryId,
                Category = x.Category,
                RevenueHeadId = x.RevenueHeadId,
                RevenueHead = x.RevenueHead,
                Code = x.Code,
                Name = x.Name,
                Amount = x.Amount.Value,
                Status = x.Status == 1 ? true : false
            }).ToList();
        }

        #endregion

        #region Identity
        public IList<Models.Identity> GetAllIdentity()
        {

            return _client.GetAllIdentity().Identities.Select(e => new Models.Identity
            {
                IdentityId = e.IdentityId,
                URL = e.URL,
                ClientId = e.ClientId,
                UserName = e.UserName,
                Password = e.Password,
                Status = e.Status == 1 ? true : false,

            }).ToList();
        }

        public Models.Identity FindIdentityId(int id)
        {
            var e = _client.FindIdentity(id);
            return new Models.Identity
            {
                IdentityId = e.Identity.IdentityId,
                URL = e.Identity.URL,
                ClientId = e.Identity.ClientId,
                UserName = e.Identity.UserName,
                Password = e.Identity.Password,
                Status = e.Identity.Status == 1 ? true : false,
            };
        }

        public void AddIdentity(CICSWebPortal.Models.Identity identity)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = identity.userId,
                clientId = identity.clientId
            };

            ServiceReference2.Identity req = new ServiceReference2.Identity()
            {
                URL = identity.URL,
                ClientId = identity.ClientId,
                UserName = identity.UserName,
                Password = identity.Password,
                Status = identity.Status == true ? 1 : 0,
               AuditTrailData = _AuditTrailData
            };

            var result = _client.CreateIdentity(req);

            if (result.ResponseCode != "0000")
            {

            }
        }

        public void UpdateIdentity(CICSWebPortal.Models.Identity identity)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData {
                 userId = identity.userId,
                  clientId = identity.clientId
            };

            ServiceReference2.Identity req = new ServiceReference2.Identity()
            {
                IdentityId = identity.IdentityId,
                URL = identity.URL,
                ClientId = identity.ClientId,
                UserName = identity.UserName,
                Password = identity.Password,
                Status = identity.Status == true ? 1 : 0,
                AuditTrailData = _AuditTrailData
            };

            var result = _client.UpdateIdentity(req);

            if (result.ResponseCode != "0000")
            {

            }
        }

        public void DeleteIdentity(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Authentication

        UserDashBoardViewModel IDataService.LoginUser(string email, string password)
        {
            var user = new UserDashBoardViewModel();
            var result = _client.UserLogin(new UserLoginReq { Email = email, UserPassword = password });
            if (result.ResponseCode == "0000")
            {
                user.Email = result.Email;
                user.RoleId = result.RoleId;
                user.UserId = result.UserId;
                user.Mobile = result.Mobile;

                user.ClientId = result.UserDashBoardData.ClientId;
                user.RoleName = result.UserDashBoardData.RoleName;
                user.ClientName = result.UserDashBoardData.ClientName;
                user.ClientLogoUrl = result.UserDashBoardData.ClientLogoUrl;
                user.UserTypeParentId = result.UserDashBoardData.UserTypeParentId;


            }
            return user;
        }
        #endregion

        #region Authorization
        public IList<Models.Role> GetAllRoles()
        {
            return _client.GetAllRoles().Role.Select(e => new Models.Role { RoleId = e.RoleId, RoleName = e.Description }).ToList();
        }

        #endregion

        #region Dashboard
        public Dashboard GetDashBoardSummary(int roleId, int userId)
        {
            var e = _client.GetDashboardSummary(new DashboardReq() { RoleId = roleId, UserId = userId });
            return new Models.Dashboard
            {
                TotalClient = e.TotalClient,
                TotalAgent = e.TotalAgent,
                TotalTerminal = e.TotalTerminal,
                TotalTransaction = e.TotalTransaction,
                TransctionValue = e.TransctionValue,
                TotalNotifications = e.TotalNotifications,
                Chart = GetChart()
            };
        }

        public Dashboard GetTaxpayerDashboardSummary(int roleId, int userId, string userEmail)
        {
            var e = _client.GetTaxpayerDashboardSummary(new DashboardReq() { Email = userEmail, RoleId = roleId, UserId = userId });
            return new Dashboard
            {
                Invoices = e.Invoices,
                Notification = e.Notification,
                Wallet = e.Wallet,
            };
        }

        public ExecutiveDashboard GetExecutiveDashBoardSummary(int roleId, int userId)
        {
            var e = _client.GetExecutiveDashboardData(new ExecutiveDashboardReq() { RoleId = roleId, UserId = userId });
            ExecutiveDashboard ed = new ExecutiveDashboard();

            ed.TotalAgent = e.TotalAgent;
                ed.TotalTransaction = e.TotalTransaction;
            ed.TotalTerminal = e.TotalTerminal;
            ed.TotalNotification = e.TotalNotification;
            ed.TotalTransctionValue = e.TotalTransctionValue;

            if (e.AgentLeaderStats != null)
            {
                ed.AgentLeaderStats = new Models.AgentLeaderStats
                {
                    LeadingAgent = e.AgentLeaderStats.LeadingAgent,
                    LeadingAgentVal = e.AgentLeaderStats.LeadingAgentVal,
                    TrailingAgent = e.AgentLeaderStats.TrailingAgent,
                    TrailingAgentVal = e.AgentLeaderStats.TrailingAgentVal
                };
            }

            if(e.RevenueLeaderStats != null)
            {
                ed.RevenueLeaderStats = new Models.RevenueLeaderStats
                {
                    LeadingRevenue = e.RevenueLeaderStats.LeadingRevenue,
                    LeadingRevenueVal = e.RevenueLeaderStats.LeadingRevenueVal,
                    TrailingRevenue = e.RevenueLeaderStats.TrailingRevenue,
                    TrailingRevenueVal = e.RevenueLeaderStats.TrailingRevenueVal
                };
            }
            
           

            List<Models.AgentStats> AgentStats = new List<Models.AgentStats>();
            if (e.AgentStats != null && e.AgentStats.Length > 0)
            {
                foreach (var aStats in e.AgentStats)
                {
                    Models.AgentStats stat = new Models.AgentStats
                    {
                        AgentCode = aStats.AgentCode,
                        AgentId = aStats.AgentId,
                        AgentName = aStats.AgentName,
                        TerminalStats = new Models.AgentTerminalStats
                        {
                            Total30DaysActiveTerminals = aStats.TerminalStats.Total30DaysActiveTerminals,
                            Total30DaysInActiveTerminals = aStats.TerminalStats.Total30DaysInActiveTerminals,
                            Total30DaysTransactions = aStats.TerminalStats.Total30DaysTransactions,
                            Total30DaysTransactionVal = aStats.TerminalStats.Total30DaysTransactionVal,
                            Total3MonthsActiveTerminals = aStats.TerminalStats.Total3MonthsActiveTerminals,
                            Total3MonthsInActiveTerminals = aStats.TerminalStats.Total3MonthsInActiveTerminals,
                            Total3MonthsTransactions = aStats.TerminalStats.Total3MonthsTransactions,
                            Total3MonthsTransactionVal = aStats.TerminalStats.Total3MonthsTransactionVal,
                            Total6MonthsActiveTerminals = aStats.TerminalStats.Total6MonthsActiveTerminals,
                            Total6MonthsInActiveTerminals = aStats.TerminalStats.Total6MonthsInActiveTerminals,
                            Total6MonthsTransactions = aStats.TerminalStats.Total6MonthsTransactions,
                            Total6MonthsTransactionVal = aStats.TerminalStats.Total6MonthsTransactionVal,
                            Total7DaysActiveTerminals = aStats.TerminalStats.Total7DaysActiveTerminals,
                            Total7DaysInActiveTerminals = aStats.TerminalStats.Total7DaysInActiveTerminals,
                            Total7DaysTransactions = aStats.TerminalStats.Total7DaysTransactions,
                            Total7DaysTransactionVal = aStats.TerminalStats.Total7DaysTransactionVal,
                            TotalActiveTerminals = aStats.TerminalStats.TotalActiveTerminals,
                            TotalInActiveTerminals = aStats.TerminalStats.TotalInActiveTerminals,
                            TotalTerminals = aStats.TerminalStats.TotalTerminals,
                            TotalTodayActiveTerminals = aStats.TerminalStats.TotalTodayActiveTerminals,
                            TotalTodayInActiveTerminals = aStats.TerminalStats.TotalTodayInActiveTerminals,
                            TotalTodayTransactions = aStats.TerminalStats.TotalTodayTransactions,
                            TotalTodayTransactionVal = aStats.TerminalStats.TotalTodayTransactionVal,
                            TotalTransactions = aStats.TerminalStats.TotalTransactions,
                            TotalTransactionVal = aStats.TerminalStats.TotalTransactionVal
                        }
                    };
                    AgentStats.Add(stat);
                }
            }

            ed.AgentStats = AgentStats;

            return ed;
        }

        public ExecutiveDashboard GetPeriodicDashboardSummary(int roleId, int userId, DateTime StartDate, DateTime EndDate)
        {
            var e = _client.GetPeriodicDashboardData(new ExecutiveDashboardReq() { RoleId = roleId, UserId = userId, StartDate = StartDate, EndDate = EndDate });
            ExecutiveDashboard ed = new ExecutiveDashboard();

            ed.StartDate = StartDate;
            ed.EndDate = EndDate;
            ed.TotalAgent = e.TotalAgent;
            ed.TotalTransaction = e.TotalTransaction;
            ed.TotalTerminal = e.TotalTerminal;
            ed.TotalNotification = e.TotalNotification;
            ed.TotalTransctionValue = e.TotalTransctionValue;

            ed.AgentLeaderStats = new Models.AgentLeaderStats
            {
                LeadingAgent = e.AgentLeaderStats.LeadingAgent,
                LeadingAgentVal = e.AgentLeaderStats.LeadingAgentVal==null?0: e.AgentLeaderStats.LeadingAgentVal,
                TrailingAgent = e.AgentLeaderStats.TrailingAgent,
                TrailingAgentVal = e.AgentLeaderStats.TrailingAgentVal==null?0: e.AgentLeaderStats.TrailingAgentVal
            };

            ed.RevenueLeaderStats = new Models.RevenueLeaderStats
            {
                LeadingRevenue = e.RevenueLeaderStats.LeadingRevenue,
                LeadingRevenueVal = e.RevenueLeaderStats.LeadingRevenueVal==null?0 : e.RevenueLeaderStats.LeadingRevenueVal,
                TrailingRevenue = e.RevenueLeaderStats.TrailingRevenue,
                TrailingRevenueVal = e.RevenueLeaderStats.TrailingRevenueVal==null? 0 : e.RevenueLeaderStats.TrailingRevenueVal
            };

            List<Models.AgentStats> AgentStats = new List<Models.AgentStats>();
            foreach (var aStats in e.AgentStats)
            {
                Models.AgentStats stat = new Models.AgentStats
                {
                    AgentCode = aStats.AgentCode,
                    AgentId = aStats.AgentId,
                    AgentName = aStats.AgentName,
                    TerminalStats = new Models.AgentTerminalStats
                    {
                        TotalActiveTerminals = aStats.TerminalStats.TotalActiveTerminals,
                        TotalInActiveTerminals = aStats.TerminalStats.TotalInActiveTerminals,
                        TotalTerminals = aStats.TerminalStats.TotalTerminals,
                        TotalTransactions = aStats.TerminalStats.TotalTransactions,
                        TotalTransactionVal = aStats.TerminalStats.TotalTransactionVal,

                        TotalPeriodicActiveTerminals = aStats.TerminalStats.TotalPeriodicActiveTerminals,
                        TotalPeriodicInActiveTerminals = aStats.TerminalStats.TotalPeriodicInActiveTerminals,
                        TotalPeriodicTransactions = aStats.TerminalStats.TotalPeriodicTransactions,
                        TotalPeriodicTransactionVal = aStats.TerminalStats.TotalPeriodicTransactionVal
                    },                    
                };

                var revStats = from results in aStats.RevenueStats
                               select new Models.RevenueStats {
                                RevenueName = results.RevenueName,
                                 TotalTransactionVal = results.TotalTransactionVal,
                                 TotalTransactionVol = results.TotalTransactionVol
                               };

                var periodicRevStats = from results in aStats.PeriodicRevenueStats
                               select new Models.RevenueStats
                               {
                                   RevenueName = results.RevenueName,
                                   TotalTransactionVal = results.TotalTransactionVal,
                                   TotalTransactionVol = results.TotalTransactionVol
                               };

                stat.RevenueStats = revStats;
                stat.PeriodicRevenueStats = periodicRevStats;
                AgentStats.Add(stat);
            }

            ed.AgentStats = AgentStats;

            return ed;
        }

        private Chart GetChart()
        {
            return new Chart(600, 400, ChartTheme.Blue)
                .AddTitle("Number of website readers")
                .AddLegend()
                .AddSeries(
                    name: "WebSite",
                    chartType: "Pie",
                    xValue: new[] { "Digg", "DZone", "DotNetKicks", "StumbleUpon" },
                    yValues: new[] { "150000", "180000", "120000", "250000" });
        }

        #endregion

        #region User
        public IList<Models.User> GetAllUsers()
        {
            return _client.GetAllUsers().Users.Select(e => new Models.User
            {
                UserId = e.UserId,
                Email = e.Email,
                Password = e.Password,
                PasswordStatus = e.PasswordStatus.Value,
                RoleId = e.RoleId.Value,
                Status = e.Status == 1 ? true : false

            }).ToList();
        }

        public IList<Models.User> GetUserAssesibleUsers(int roleId, int clientId)
        {
            return _client.GetUserAssesibleUsers(new GetAllUserReq { roleId=roleId,UserTypeParentId=clientId}).Users.Select(e => new Models.User
            {
                UserId = e.UserId,
                Email = e.Email,
                Password = e.Password,
                PasswordStatus = e.PasswordStatus.Value,
                RoleId = e.RoleId.Value,
                Status = e.Status == 1 ? true : false

            }).ToList();
        }

        public Models.User FindUserById(int id)
        {
            var user = _client.FindUser(id).User;
            return new Models.User
            {
                UserId = user.UserId,
                RoleId = user.RoleId.Value,
                UserTypeParentId = user.UserTypeParentId,
                Email = user.Email,
                Password = user.Password,
                PasswordStatus = user.PasswordStatus.Value,
                Mobile = user.Mobile,
                Status = user.Status == 1 ? true : false
            };
        }

        public Models.User FindUserByEmail(string email)
        {
            var user = _client.FindUserByEmail(email).User;
            return new Models.User
            {
                UserId = user.UserId,
                RoleId = user.RoleId.Value,
                UserTypeParentId = user.UserTypeParentId,
                Email = user.Email,
                Password = user.Password,
                PasswordStatus = user.PasswordStatus.Value,
                Mobile = user.Mobile,
                Status = user.Status == 1 ? true : false
            };
        }

        public void AddUser(Models.User user)
        {
            if (user == null)
            {
                throw new Exception("User is null or empty");
            }

            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = user.userId,
                clientId = user.clientId
            };

            ServiceReference2.User userReq = new ServiceReference2.User()
            {
                UserId = user.UserId,
                UserTypeParentId = user.UserTypeParentId,
                ClientId = user.ClientId,
                Email = user.Email,
                PasswordStatus = user.PasswordStatus,
                Password = user.Password,
                Mobile = user.Mobile,
                RoleId = user.RoleId,
                Status = user.Status == true ? 1 : 0,
                 AuditTrailData = _AuditTrailData
            };

            var result = _client.CreateUser(userReq);

            if (result.ResponseCode != "0000")
            {

            }

        }

        public void UpdateUser(Models.User user)
        {
            if (user == null)
            {
                throw new Exception("User is null or empty");
            }
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = user.userId,
                clientId = user.clientId
            };
            ServiceReference2.User userReq = new ServiceReference2.User()
            {
                UserId = user.UserId,
                UserTypeParentId = user.UserTypeParentId,
                Email = user.Email,
                PasswordStatus = user.PasswordStatus,
                Password = user.Password,
                Mobile = user.Mobile,
                RoleId = user.RoleId,
                Status = user.Status == true ? 1 : 0,
                 AuditTrailData = _AuditTrailData
            };
        }

        public void ResetUserPassword(ResetPasswordModel user)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = user.userId,
                clientId = user.clientId
            };

            var result = _client.ResetUserPassword(new ResetUserPasswordReq {
                Id =user.UserId,
                 AuditTrailData = _AuditTrailData
            });
        }

        public void ChangeUserPassword(ChangeUserPasswordModel user)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = user.userId,
                clientId = user.clientId
            };
            var result = _client.ChangeUserPassword(new ChangeUserPassword {
                UserEmail =user.UserEmail,
                UserId = user.UserId,
                OldPassword =  user.OldPassword,
                NewPassword  = user.NewPassword,
                AuditTrailData = _AuditTrailData
            });
        }

        public void UpdateUserStatus(Models.User user)
        {
            if (user == null)
            {
                throw new Exception("User is null or empty");
            }

            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = user.userId,
                clientId = user.clientId
            };
            var result = _client.UpdateUserStatus(new UserStatus {
                UserId = user.UserId,
                status = user.Status == true ? 1 : 0,
                AuditTrailData = _AuditTrailData
            });
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Reports
        public IList<Models.AgentSummary> GetAgentReportSummary(int clientId)
        {
            var agentSummaries = _client.GetAgentsSummary(clientId).AgentSummaries;
            if (agentSummaries != null)
            {
                return agentSummaries.Select(x => new Models.AgentSummary
                {
                    AgentId = x.AgentId,
                    AgentName = x.AgentName,
                    TotalActiveTerminals = x.TotalActiveTerminals,
                    TotalTerminal = x.TotalTerminal,
                    TotalTransactionCount = x.TotalTransactionCount,
                    TotalTransactionValue = x.TotalTransactionValue
                }).ToList();
            }
            else {
                return new List<Models.AgentSummary>{ };
            }
        }

        public IList<Models.ClientSummary> GetClientReportSummary()
        {
            var cSummary = _client.GetClientsSummary().ClientSummaries.ToList();
            if (cSummary != null)
            {
                return  cSummary.Select(x => new Models.ClientSummary
                {
                    ClientId = x.clientId,
                    ClientName = x.ClientName,
                    TotalAgent = x.TotalAgents,
                    TotalRevenueHeads = x.TotalRevenueHeads,
                    TotalActiveTerminals = x.TotalActiveTerminals,
                    TotalTerminal = x.TotalTerminal,
                    TotalTransactionCount = x.TotalTransactionCount,
                    TotalTransactionValue = x.TotalTransactionValue
                }).ToList();
            }
            else
            {
                return new List<Models.ClientSummary> { };
            }
        }

        public ReportViewModel GetTransactionReportSummary(ReportFilter request)
        {
            ReportViewModel rvm = new ReportViewModel();

            var cSummaryList = _client.GetReportSummary(new TransactionSummary
            {
                clientId = request.clientId,
                AgentId = request.agentId,
                terminalId = request.terminalId,
                RevenueCode = request.RevenueCode,
                Ministry = request.ministry,
                startDate = request.startDate,
                endDate = request.endDate
            });

            var cSummary = (cSummaryList != null && cSummaryList.Report != null) ? cSummaryList.Report.ToList() : null;

            if (cSummary != null && cSummary.Count != 0)
            {
                rvm.Report = cSummary.Select(x => new Models.Report
                {
                    Agent = x.Agent,
                    RevenueName = x.RevenueName,
                    Ministry = x.Ministry,
                    RevenueHead = x.RevenueHead,
                    ResidentId = x.ResidentId,
                    Amount = x.Amount,
                    RevenueCode = x.RevenueCode,
                    Terminal = x.Terminal,
                    TransactionCode = x.TransactionCode,
                    TransactionDate = x.TransactionDate,
                    TransactionId = x.TransactionId

                }).ToList();

                //check if the report enumerable of the RepoertViewModel is null or empty. If it isn't, assign it to _report to export.
                //if (rvm.Report != null && rvm.Report.Any())
                //{
                //    _reportToExport = rvm.Report;
                //}

                return rvm;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Generates the excel format for transaction Report
        /// </summary>
        public void GenerateExcelReport(IEnumerable<Models.Report> report)
        {
            var day = DateTime.Now.Day;
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var fileName = $"{day}-{month}-{year}_Report";
            DataSet dataSet = report.ConvertToDataSet();
            Utility.ExportToExcel(dataSet, HttpContext.Current, fileName);
        }

        public List<Models.Notification> GetAllNotifications()
        {
            var notifications = _client.GetAllNotifications();
            if (notifications != null)
            {
                return notifications.Notifications.Select(x =>  new Models.Notification
                {
                   Id = x.Id,
                    Message = x.Message,
                    Recipient = x.Recipient,
                    ReferenceId = x.ReferenceId,
                    Retry = x.Retry,
                    Status = x.Status,
                    StatusMessage= x.StatusMessage,
                    Type = x.Type
                }).ToList();
            }
            else
            {
                return new List<Models.Notification> { };
            }
        }

        public List<Models.AuditTrail> GetAllAuditTrail()
        {
            var auditT = _client.GetAllAuditTrails();
            if (auditT != null)
            {
                return auditT.AuditTrails.Select(x => new Models.AuditTrail
                {
                    AuditLog = x.AuditLog,
                    Client = x.Client,
                    TableAffected = x.TableAffected,
                    LogDate = x.LogDate.Value.ToString(),
                    Type = x.Type,
                    User = x.User
                }).ToList();
            }
            else
            {
                return new List<Models.AuditTrail> { };
            }
        }
        #endregion

        #region Location
        public IList<Models.Location> GetAllLocations()
        {
            return _client.GetAllLocations().Locations.Select(e => new Models.Location
            {
                Id = e.Id,
                AgentId = e.AgentId.Value,
                ClientId = e.ClientId.Value,
                LocationCode = e.LocationCode,
                LocationDescription = e.LocationDescription
            }).ToList();
        }

        public IList<Models.Location> GetAllLocationsByClientId(int clientId)
        {
            return _client.GetAllLocationsByClientId(clientId).Locations.Select(e => new Models.Location
            {
                Id = e.Id,
                AgentId = e.AgentId.Value,
                ClientId = e.ClientId.Value,
                LocationCode = e.LocationCode,
                LocationDescription = e.LocationDescription
            }).ToList();
        }

        public IList<Models.Location> GetAllLocationsByAgentId(int agentId)
        {
            return _client.GetAllLocationsByAgentId(agentId).Locations.Select(e => new Models.Location
            {
                Id = e.Id,
                AgentId = e.AgentId.Value,
                ClientId = e.ClientId.Value,
                LocationCode = e.LocationCode,
                LocationDescription = e.LocationDescription
            }).ToList();
        }

        public Models.Location FindLocationById(int id)
        {
            var e = _client.FindLocation(id).Location;

            if (e != null)
                return new Models.Location
                {
                    Id = e.Id,
                    AgentId = e.AgentId.Value,
                    ClientId = e.ClientId.Value,
                    LocationCode = e.LocationCode,
                    LocationDescription = e.LocationDescription
                };
            else return new Models.Location { };
        }

        public string AddLocation(Models.Location location)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = location.userId,
                clientId = location.clientId
            };

            ServiceReference2.Location loc = new ServiceReference2.Location
            {
                AgentId = location.AgentId,
                ClientId = location.ClientId,
                LocationCode = location.LocationCode,
                LocationDescription = location.LocationDescription,
                AuditTrailData = _AuditTrailData,
            };

            var result = _client.CreateLocation(loc);
            return result.ResponseDescription;
        }

        public string UpdateLocation(Models.Location location)
        {
            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData
            {
                userId = location.userId,
                clientId = location.clientId
            };

            ServiceReference2.Location loc = new ServiceReference2.Location
            {
                 Id = location.Id,
                AgentId = location.AgentId,
                ClientId = location.ClientId,
                LocationCode = location.LocationCode,
                LocationDescription = location.LocationDescription,
                AuditTrailData = _AuditTrailData,
            };

            var result = _client.UpdateLocation(loc);
            return result.ResponseDescription;
        }

        public string DeleteLocation(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Wallet
        public Models.Wallet GetWallet(string email)
        {
            var res = _client.GetWallet(email);
            if (res.Wallet != null)
            {
                Models.Wallet wallet = new Models.Wallet
                {
                    Email = res.Wallet.Email,
                    Amount = res.Wallet.Amount,
                    WalletId = res.Wallet.WalletId
                };
                return wallet;
            }
            else
                return null;
        }

        public Boolean FundWallet(Models.Payment payment)
        {
            if (payment == null)
            {
                throw new Exception("paymet is null or empty");
            }

            ServiceReference2.AuditTrailData _AuditTrailData = new ServiceReference2.AuditTrailData()
            {
                userId = payment.UserId
            };

            ServiceReference2.Payment paymentReq = new ServiceReference2.Payment()
            {
                WalletId = payment.WalletId,
                TransactionId = payment.TransactionId,
                Reference = payment.Reference,
                Amount = payment.Amount,
                Target = payment.Target,
                Method = payment.Method,
                Email = payment.Email,
                UserId = payment.UserId
            };

            var result = _client.FundWallet(paymentReq);
            return result;
        }
        #endregion

        #region Invoice
        public List<Models.Invoice> GetUserInvoices(string email)
        {
            return _client.GetUserInvoices(email).UserInvoices.Select(x => new Models.Invoice
            {
                InvoiceId = x.InvoiceId,
                InvoiceNo = x.InvoiceNo,
                Description = x.Description,
                Amount = x.Amount,
                EntryDate = x.EntryDate.ToString(),
                Email = x.Email,
                RevenueCode = x.RevenueCode
            }).ToList();
        }

        public List<Models.Invoice> GetAllInvoices()
        {
            return _client.GetAllInvoices().UserInvoices.Select(x => new Models.Invoice
            {
                InvoiceId = x.InvoiceId,
                InvoiceNo = x.InvoiceNo,
                Description = x.Description,
                Amount = x.Amount,
                Email = x.Email,
                EntryDate = x.EntryDate.ToString(),
                RevenueCode = x.RevenueCode
            }).ToList();
        }

        public Models.Invoice GetInvoiceById(int id)
        {
            var invoice = _client.GetInvoiceById(id);
            
            return invoice.InvoiceId != 0 ? new Models.Invoice
            {
                InvoiceId = invoice.InvoiceId,
                InvoiceNo = invoice.InvoiceNo.ToString(),
                Description = invoice.Description,
                Amount = invoice.Amount,
                RevenueCode = invoice.RevenueCode
            } : null ;
        }

        public List<Models.Payment> GetTransactionDetails(int id, string email)
        {
            ServiceReference2.PaymentReq _paymentReq = new ServiceReference2.PaymentReq
            {
                InvoiceId = id,
                Email = email
            };
            return _client.GetTransactionDetails(_paymentReq).Payments.Select(x => new Models.Payment
            {
                Amount = x.Amount,
                TransactionId = x.TransactionId,
                Reference = x.Reference,
                Target = x.Target

            }).ToList();
        }

        public List<Models.Payment> GetAllTransactionDetailsByUser(int id, string email)
        {
            ServiceReference2.PaymentReq _paymentReq = new ServiceReference2.PaymentReq
            {
                InvoiceId = id,
                Email = email
            };
            return _client.GetAllTransactionDetailsByUser(_paymentReq).Payments.Select(x => new Models.Payment
            {
                Amount = x.Amount,
                TransactionId = x.TransactionId,
                Reference = x.Reference,
                Target = x.Target,
                Method = x.Method,
                Status = x.Status,
                EntryDate = x.EntryDate
            }).ToList();
        }

        public Boolean MakePayment(Models.PaymentReq payment)
        {
            ServiceReference2.PaymentReq _paymentReq = new ServiceReference2.PaymentReq
            {
                Amount = payment.Amount,
                Email = payment.Email,
                Method = payment.Method,
                Reference = payment.Reference,
                Target = payment.Target,
                TransactionId = payment.TransactionId,
                UserId = payment.UserId,
                WalletId = payment.WalletId,                
            };

            var result = _client.MakePayment(_paymentReq);
            if (result.ResponseCode == "0000")
            {
                return true;
            }
            return false;
        }
        #endregion

        #region Taxpayer
        public List<Models.Biodata> GetAllTaxPayers()
        {
            return _client.GetAllTaxpayers().Taxpayers.Select(x => new Models.Biodata
            {
               BiodataId = x.BiodataId,
               Email = x.Email,
               EntryDate = x.EntryDate,
               Firstname = x.Firstname,
               Surname = x.Surname,
               RIN = x.RIN,
               UserId = x.UserId,

            }).ToList();
        }
        
        public Boolean AddTaxpayer(Models.Taxpayer taxpayer)
        {
            ServiceReference2.Taxpayer _taxpayerReq = new ServiceReference2.Taxpayer
            {
                Email = taxpayer.Email,
                Firstname = taxpayer.Firstname,
                Surname = taxpayer.Surname,
                Mobile = taxpayer.Mobile,
                Password = taxpayer.Password,
                RIN = taxpayer.RIN,
                RoleId = taxpayer.RoleId,                
            };
            var result = _client.AddTaxpayer(_taxpayerReq);

            if (result.ResponseCode == "0000")
            {
                return true;
            }
            return false;
        }

        public Models.Taxpayer GetTaxpayerById(int id)
        {
            var taxpayer = _client.GetTaxpayerById(id);

            return new Models.Taxpayer
            {
                BiodataId = taxpayer.BiodataId,
                Email = taxpayer.Email,
                Firstname = taxpayer.Firstname,
                Surname = taxpayer.Surname,
                RIN = taxpayer.RIN,
                UserId = taxpayer.UserId,
                Mobile = taxpayer.Mobile
            };
        }

        public List<Models.RevenueItem> GetAssessmentByRole(Models.AssessmentReq assessmentReq)
        {
            ServiceReference2.AssessmentReq _assessmentRequest = new ServiceReference2.AssessmentReq
            {
                roleid = assessmentReq.roleid,
                email = assessmentReq.email,
            };

            return _client.GetAssessmentByRole(_assessmentRequest).RevenueItems.Select(x => new Models.RevenueItem
            {
                Amount = x.Amount.HasValue ? x.Amount.Value : 0,
                Name = x.Name,
                Code = x.Code,
                

            }).ToList();
        }

        public Boolean GenerateInvoice(Models.GenerateInvoice generateInvoice)
        {
            ServiceReference2.GenerateInvoice _generateInvoice = new ServiceReference2.GenerateInvoice
            {
                RevenueCode = generateInvoice.RevenueCode,
                TaxpayerId = generateInvoice.TaxpayerId,
            };
            var result = _client.GenerateInvoice(_generateInvoice);

            if (result.ResponseCode == "0000")
            {
                return true;
            }
            return false;
        }
        #endregion
    }
}