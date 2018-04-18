using ChamsICSLib.Data;
using ChamsICSLib.Utilities;
using ChamsICSWebService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace ChamsICSWebService
{
    public partial class ChamsICSService
    {
        #region Client
        public CreateClientRes CreateClient(CreateClientReq req)
        {
            CreateClientRes res = new CreateClientRes();
            try
            {
                string msg = string.Empty;
                string clientCode = string.Empty;
                string clientId = string.Empty;

                bool result = ServiceHelper.CreateClient(req, out clientId, out clientCode, out msg);

                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Client Created Succesfully";
                    res.Code = clientCode;
                    res.ClientId = clientId;
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public Response UpdateClient(UpdateClientReq req)
        {
            Response res = new Response();
            try
            {

                bool result = ServiceHelper.UpdateClient(req);

                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = "Unable to Update Client Details";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Client Updated Succesfully";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public FindClientRes FindClient(int clientId)
        {
            FindClientRes res = new FindClientRes();
            try
            {
                res = ServiceHelper.FindClient(clientId);

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

        public FindClientWithZoneRes FindClientWithZones(int clientId)
        {
            FindClientWithZoneRes res = new FindClientWithZoneRes();
            try
            {
                res = ServiceHelper.FindClientWithZone(clientId);

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

        public GetAllClientsRes GetAllClient()
        {
            GetAllClientsRes res = new GetAllClientsRes();
            try
            {

                res = ServiceHelper.GetAllClients();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllClientsWithZoneRes GetAllClientWithZones()
        {
            GetAllClientsWithZoneRes res = new GetAllClientsWithZoneRes();
            try
            {

                res = ServiceHelper.GetAllClientsWithZones();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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
        #endregion

        #region Agent
        public CreateAgentRes CreateAgent(Model.Agent req)
        {
            CreateAgentRes res = new CreateAgentRes();
            string msg = string.Empty;
            string agentCode = string.Empty;
            string agentId = string.Empty;

            try
            {
                bool result = ServiceHelper.CreateAgent(req, out agentId, out agentCode, out msg);

                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Agent Created Succesfully";
                    res.AgentCode = agentCode;
                    res.AgentId = agentId;
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = msg + ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public Response UpdateAgent(Model.Agent req)
        {
            Response res = new Response();

            try
            {
                bool result = ServiceHelper.UpdateAgent(req);

                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = "Unable to Update Agent Details";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Agent Updated Succesfully";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public FindAgentRes FindAgent(int Id)
        {
            FindAgentRes res = new FindAgentRes();
            try
            {
                res = ServiceHelper.FindAgent(Id);

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

        public GetAllAgentRes GetAllAgents()
        {
            GetAllAgentRes res = new GetAllAgentRes();
            try
            {

                res = ServiceHelper.GetAllAgent();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllAgentRes GetAllAgentsByClientId(int clientId)
        {
            GetAllAgentRes res = new GetAllAgentRes();
            try
            {

                res = ServiceHelper.GetAllAgentByClientId(clientId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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
        #endregion

        #region Terminal
        public FindTerminalRes FindTerminal(int Id)
        {
            FindTerminalRes res = new FindTerminalRes();
            try
            {
                res = ServiceHelper.FindTerminal(Id);

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
        public FindTerminalRes FindTerminalByCode(string code)
        {
            FindTerminalRes res = new FindTerminalRes();
            try
            {
                res = ServiceHelper.FindTerminal(code);

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

        public GetAllTerminalRes GetAllTerminals()
        {
            GetAllTerminalRes res = new GetAllTerminalRes();
            try
            {

                res = ServiceHelper.GetAllTerminals();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTerminalRes GetAllTerminalsByAgentId(int AgentId)
        {
            GetAllTerminalRes res = new GetAllTerminalRes();
            try
            {

                res = ServiceHelper.GetAllTerminalsByAgentId(AgentId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTerminalRes GetAllTerminalsByClientId(int AgentId)
        {
            GetAllTerminalRes res = new GetAllTerminalRes();
            try
            {

                res = ServiceHelper.GetAllTerminalsByClientId(AgentId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public Response UpdateTerminalStatus(TerminalStatus req)
        {
            Response res = new Response();
            var result = ServiceHelper.UpdateTerminalStatus(req);
            if (!result)
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Update Terminal Status not succesful";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Terminal Updated Succesfully";
            }

            return res;
        }


        #endregion

        #region Transaction
        public FindTransactionRes FindTransaction(int Id)
        {
            FindTransactionRes res = new FindTransactionRes();
            try
            {
                res = ServiceHelper.FindTransaction(Id);

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

        public FindTransactionRes FindTransactionByCode(string transactionCode)
        {
            FindTransactionRes res = new FindTransactionRes();
            try
            {
                res = ServiceHelper.FindTransactionByCode(transactionCode);

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

        public GetAllTransactionRes GetTransactions(GetTransactionRequest req)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetTransactions(req);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetAllTransactionByLocationId(int locationId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {
                res = ServiceHelper.GetAllTransactionsByLocationId(locationId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        #region ===Depricated.. Now Replaced by GetAllTransactionRes GetTransactions(GetTransactionRequest req)
        public GetAllTransactionRes GetAllTransaction()
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetAllTransactions();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetAllTransactionByClientId(int clientId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetAllTransactionsByClientId(clientId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetAllTransactionByAgentId(int agentId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetAllTransactionsByAgentId(agentId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetAllTransactionByTerminalId(int terminalId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetAllTransactionsByTerminalId(terminalId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetAllTransactionByResidentId(string residentId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetAllTransactionsByResidentId(residentId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetLast10TransactionByTerminalId(int terminalId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetLast10TransactionsByTerminalId(terminalId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetLast1000Transaction()
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetLast1000Transactions();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetLast1000TransactionByClientId(int clientId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetLast1000TransactionsByClientId(clientId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetLast1000TransactionByAgentId(int agentId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetLast1000TransactionsByAgentId(agentId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetLast1000TransactionByTerminalId(int terminalId)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetLast1000TransactionsByTerminalId(terminalId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetAllTransactionByTerminalPerPeriod(PeriodicTransactionRequest periodicTransactionRequest)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetAllTransactionByTerminalPerPeriod(periodicTransactionRequest.TerminalId, periodicTransactionRequest.FromDate, periodicTransactionRequest.ToDate);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllTransactionRes GetAllTransactionByAgentPerPeriod(PeriodicTransactionRequest periodicTransactionRequest)
        {
            GetAllTransactionRes res = new GetAllTransactionRes();
            try
            {

                res = ServiceHelper.GetAllTransactionByAgentPerPeriod(periodicTransactionRequest.AgentId, periodicTransactionRequest.FromDate, periodicTransactionRequest.ToDate);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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


        #endregion

        #endregion

        #region Upload Error
        public FindErrorTransactionRes FindErrorTransaction(int Id)
        {
            FindErrorTransactionRes res = new FindErrorTransactionRes();
            try
            {
                res = ServiceHelper.FindErrorTransaction(Id);

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

        public FindErrorTransactionRes FindErrorTransactionByCode(string transactionCode)
        {
            FindErrorTransactionRes res = new FindErrorTransactionRes();
            try
            {
                res = ServiceHelper.FindErrorTransactionByCode(transactionCode);

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

        public GetAllErrorTransactionRes GetErrorTransaction(GetTransactionRequest req)
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();
            try
            {
                res = ServiceHelper.GetErrorTransactions(req);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllErrorTransactionRes GetAllErrorTransaction()
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();
            try
            {

                res = ServiceHelper.GetAllErrorTransactions();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllErrorTransactionRes GetAllErrorTransactionByClientId(int clientId)
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();
            try
            {

                res = ServiceHelper.GetAllErrorTransactionsByClientId(clientId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllErrorTransactionRes GetAllErrorTransactionByAgentId(int agentId)
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();
            try
            {

                res = ServiceHelper.GetAllErrorTransactionsByAgentId(agentId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllErrorTransactionRes GetAllErrorTransactionByTerminalId(int terminalId)
        {
            GetAllErrorTransactionRes res = new GetAllErrorTransactionRes();
            try
            {

                res = ServiceHelper.GetAllErrorTransactionsByTerminalId(terminalId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        #endregion

        #region Revenue
        public CreateRevenueRes CreateRevenue(Model.Revenue req)
        {
            CreateRevenueRes res = new CreateRevenueRes();
            string msg = string.Empty;
            int? reveuneId = null;

            try
            {
                bool result = ServiceHelper.CreateRevenue(req, out reveuneId, out msg);

                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Revenue Created Succesfully";
                    res.RevenueId = reveuneId;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = msg + ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
            return res;
        }

        public Response UpdateRevenue(Model.Revenue req)
        {
            Response res = new Response();
            string msg = string.Empty;

            try
            {
                bool result = ServiceHelper.UpdateRevenue(req, out msg);

                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = "Unable to Update Revenue";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Revenue Updated Succesfully";
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = msg + ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
            return res;
        }

        public FindRevenueRes FindRevenue(int Id)
        {
            FindRevenueRes res = new FindRevenueRes();
            try
            {
                res = ServiceHelper.GetRevenue(Id);

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

        public GetAllRevenueRes GetAllRevenues()
        {
            GetAllRevenueRes res = new GetAllRevenueRes();
            try
            {

                res = ServiceHelper.GetAllRevenue();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllRevenueRes GetAllRevenuesByClientId(int clientId)
        {
            GetAllRevenueRes res = new GetAllRevenueRes();
            try
            {

                res = ServiceHelper.GetAllRevenueByClientId(clientId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public List<Model.Ministry> GetAllMinistry()
        {
            try
            {
                var res = ServiceHelper.GetAllMinistry();
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.Ministry>() { };
            }
        }

        public List<Model.Ministry> GetAllMinistryByClientId(int id)
        {
            try
            {
                var res = ServiceHelper.GetAllMinistryByClientId(id);
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.Ministry>() { };
            }
        }

        public List<Model.RevenueHead> GetAllRevenueHead()
        {
            try
            {
                var res = ServiceHelper.GetAllRevenueHead();
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.RevenueHead>() { };
            }
        }

        public List<Model.RevenueHead> GetAllRevenueHeadByClientId(int id)
        {
            try
            {
                var res = ServiceHelper.GetAllRevenueHeadByClientId(id);
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.RevenueHead>() { };
            }
        }

        public List<Model.RevenueHead> GetAllRevenueHeadByMinistryId(int id)
        {
            try
            {
                var res = ServiceHelper.GetAllRevenueHeadByMinistryId(id);
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.RevenueHead>() { };
            }
        }

        public List<Model.RevenueCategory> GetAllRevenueCategoryByClientId(int id)
        {
            try
            {
                var res = ServiceHelper.GetAllRevenueCategoryByClientId(id);
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.RevenueCategory>() { };
            }
        }

        public List<Model.RevenueCategory> GetAllRevenueCategoryByRevenueHeadId(int id)
        {
            try
            {
                var res = ServiceHelper.GetAllRevenueCategoryByRevenueHeadId(id);
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.RevenueCategory>() { };
            }
        }

        public List<Model.RevenueItem> GetAllRevenueItemByClientId(int id)
        {
            try
            {
                var res = ServiceHelper.GetAllRevenueItemByClientId(id);
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.RevenueItem>() { };
            }
        }

        public List<Model.RevenueItem> GetAllRevenueItemByMinistryId(int id)
        {
            try
            {
                var res = ServiceHelper.GetAllRevenueItemByMinistryId(id);
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.RevenueItem>() { };
            }
        }

        public List<Model.RevenueItem> GetAllRevenueItemByCategoryId(int id)
        {
            try
            {
                var res = ServiceHelper.GetAllRevenueItemByCategoryId(id);
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.RevenueItem>() { };
            }
        }

        public List<Model.RevenueItem> GetAllRevenueItemByRevenueHeadId(int id)
        {
            try
            {
                var res = ServiceHelper.GetAllRevenueItemByRevenueHeadId(id);
                return res;
            }
            catch (Exception ex)
            {
                Logger.logToFile(ex, ErrorLogPath);
                return new List<Model.RevenueItem>() { };
            }
        }

        #endregion

        #region Identity
        public CreateIdentityRes CreateIdentity(Model.Identity req)
        {
            CreateIdentityRes res = new CreateIdentityRes();
            string msg = string.Empty;
            int? identityId = null;

            try
            {
                bool result = ServiceHelper.CreateIdentity(req, out identityId, out msg);

                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Identity Created Succesfully";
                    res.IdentityId = identityId;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = msg + ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
            return res;
        }

        public Response UpdateIdentity(Model.Identity req)
        {
            Response res = new Response();
            string msg = string.Empty;

            try
            {
                bool result = ServiceHelper.UpdateIdentity(req, out msg);

                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Identity Updated Succesfully";
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = msg + ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
            return res;
        }

        public FindIdentityRes FindIdentity(int Id)
        {
            FindIdentityRes res = new FindIdentityRes();
            try
            {
                res = ServiceHelper.GetIdentity(Id);

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

        public GetAllIdentityRes GetAllIdentity()
        {
            GetAllIdentityRes res = new GetAllIdentityRes();
            try
            {

                res = ServiceHelper.GetAllIdentity();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllIdentityRes GetAllIdentityByClientId(int clientId)
        {
            GetAllIdentityRes res = new GetAllIdentityRes();
            try
            {
                res = ServiceHelper.GetAllIdentityByClientId(clientId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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
        #endregion

        #region User
        public RoleRes GetAllRoles()
        {
            RoleRes res = new RoleRes();
            try
            {
                res = ServiceHelper.GetAllRoles();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public FindRoleRes FindRole(int ID)
        {
            FindRoleRes res = new FindRoleRes();
            try
            {
                res = ServiceHelper.FindRole(ID);
                if (res.RoleId >= 0)
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Role not found";
                }

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

        public FindRoleRes FindRoleByCode(string code)
        {
            FindRoleRes res = new FindRoleRes();
            try
            {
                res = ServiceHelper.FindRole(code);
                if (res.RoleId >= 0)
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Role not found";
                }

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


        public CreateUserRes CreateUser(Model.User req)
        {
            CreateUserRes res = new CreateUserRes();

            try
            {
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

                int? userId = null;
                bool result = ServiceHelper.CreateUser(req, out userId);
                res.userId = userId.Value;
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "User Created Succesfully";

                return res;

            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.InnerException.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public Response UpdateUser(Model.User req)
        {
            Response res = new Response();
            string msg = string.Empty;

            try
            {
                bool result = ServiceHelper.UpdateUser(req, out msg);

                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Identity Updated Succesfully";
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = msg + ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
            return res;

        }

        public UserLoginRes UserLogin(UserLoginReq req)
        {
            UserLoginRes res = new UserLoginRes();

            bool result = ServiceHelper.UserLogin(req, out res);

            return res;
        }

        public Response ResetUserPassword(ResetUserPasswordReq req)
        {
            Response res = new Response();
            var result = ServiceHelper.ResetUserPassword(req);
            if (!result)
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Reset Password not succesful";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Identity Updated Succesfully";
            }

            return res;
        }

        public Response UpdateUserStatus(UserStatus req)
        {
            Response res = new Response();
            var result = ServiceHelper.UpdateUserStatus(req);
            if (!result)
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Update User Status not succesful";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "User Updated Succesfully";
            }

            return res;
        }

        public Response ChangeUserPassword(ChangeUserPassword request)
        {
            Response res = new Response();
            var result = ServiceHelper.ChangeUserPassword(request);
            if (!result)
            {
                res.ResponseCode = ResponseHelper.FAILED;
                res.ResponseDescription = "Update User Status not succesful";
            }
            else
            {
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "User Updated Succesfully";
            }

            return res;
        }

        public GetAllUserRes GetAllUsers()
        {
            GetAllUserRes res = new GetAllUserRes();
            try
            {
                res = ServiceHelper.GetAllUsers();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllUserRes GetAllUsersByClientId(int id)
        {
            GetAllUserRes res = new GetAllUserRes();
            try
            {
                res = ServiceHelper.GetAllUsersByClientId(id);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllUserRes GetUserAssesibleUsers(GetAllUserReq req)
        {
            GetAllUserRes res = new GetAllUserRes();
            try
            {
                res = ServiceHelper.GetUserAssesibleUsers(req.roleId, req.UserTypeParentId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllUserRes GetAllUsersByTypeId(GetAllUserReq req)
        {
            GetAllUserRes res = new GetAllUserRes();
            try
            {
                res = ServiceHelper.GetAllUsers(req.roleId, req.UserTypeParentId);
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

        public FindUserRes FindUser(int id)
        {
            FindUserRes res = new FindUserRes();
            try
            {
                res = ServiceHelper.GetUser(id);

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

        public FindUserRes FindUserByEmail(string email)
        {
            FindUserRes res = new FindUserRes();
            try
            {
                res = ServiceHelper.GetUserByEmail(email);

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

        #endregion

        #region Dashboard
        public DashboardRes GetDashboardSummary(DashboardReq req)
        {
            Logger.logToFile(req?.UserId + " " + req?.RoleId, ErrorLogPath); //please remove when complete.
            DashboardRes res = new DashboardRes();
            try
            {
                res = ServiceHelper.GetDashBoardData(req);

                if (res != null)
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Dashboard Data";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);

                return res;
            }
        }

        public TaxpayerDashboardRes GetTaxpayerDashboardSummary(DashboardReq req)
        {
            TaxpayerDashboardRes res = new TaxpayerDashboardRes();
            try
            {
                res = ServiceHelper.GetTaxpayerDashBoardData(req);
                if (res != null)
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Dashboard Data";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;


                return res;
            }
        }

        public ExecutiveDashboardRes GetExecutiveDashboardData(ExecutiveDashboardReq req)
        {
            ExecutiveDashboardRes res = new ExecutiveDashboardRes();
            try
            {
                res = ServiceHelper.GetExecutiveDashboardData(req);
                if (res != null)
                {

                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Dashboard Data";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);

                return res;
            }
        }

        public ExecutiveDashboardRes GetPeriodicDashboardData(ExecutiveDashboardReq req)
        {
            ExecutiveDashboardRes res = new ExecutiveDashboardRes();
            try
            {
                res = ServiceHelper.GetPeriodicDashboardData(req);
                if (res != null)
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Dashboard Data";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);

                return res;
            }
        }
        #endregion

        #region Reports
        public AgentSummaryRes GetAgentsSummary(int clientId)
        {
            AgentSummaryRes res = new AgentSummaryRes();
            try
            {
                List<AgentSummary> aSum = ServiceHelper.GetAgentSummary(clientId);
                if (aSum != null)
                {
                    res.AgentSummaries = aSum;
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Data";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public ClientSummaryRes GetClientsSummary()
        {
            ClientSummaryRes res = new ClientSummaryRes();
            try
            {
                List<ClientSummary> sSum = ServiceHelper.GetClientSummary();
                if (sSum != null)
                {
                    res.ClientSummaries = sSum;
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Data";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public TransactionSummaryRes GetReportSummary(TransactionSummary request)
        {
            TransactionSummaryRes res = new TransactionSummaryRes();
            try
            {
                List<Report> sSum = ServiceHelper.GetTransactionSummary(request);
                if (sSum != null)
                {
                    res.Report = sSum;
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Data";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        /// <summary>
        /// Sets service response based service call result
        /// </summary>
        /// <param name="request"></param>
        /// <returns>End of day report to DataService</returns>
        public FetchEndOfDayRes GetEODReport(FetchEndOfDayReq request)
        {
            FetchEndOfDayRes res = new FetchEndOfDayRes();
            try
            {
                List<EndOfDayModel> report = ServiceHelper.GetEndOfDayReport(request);
                if (report != null && report.Count > 0)
                {
                    res.EndOfDayReport = report;
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Data";
                    return res;
                }
            }
            catch (Exception ex)
            {

                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        #endregion

        #region Audit and Notifications
        public GetNotificationsRes GetAllNotifications()
        {
            GetNotificationsRes res = new GetNotificationsRes();
            try
            {
                List<Model.Notification> result = ServiceHelper.GetAllNotifications();
                if (result != null)
                {
                    res.Notifications = result;
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Data";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        public GetAuditTrailsRes GetAllAuditTrails()
        {
            GetAuditTrailsRes res = new GetAuditTrailsRes();
            try
            {
                List<Model.AuditTrail> result = ServiceHelper.GetAllAuditTrails();
                if (result != null)
                {
                    res.AuditTrails = result;
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Success";
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed to Load Data";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        #endregion

        #region Location
        public CreateLocationRes CreateLocation(Location req)
        {
            CreateLocationRes res = new CreateLocationRes();
            try
            {
                string msg = string.Empty;
                bool result = ServiceHelper.CreateLocation(req, out msg);

                if (result)
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = msg;
                }
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

        public Response UpdateLocation(Location req)
        {
            Response res = new Response();
            try
            {
                bool result = ServiceHelper.UpdateLocation(req);

                if (result)
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Successful";
                }
                else
                {
                    res.ResponseCode = ResponseHelper.FAILED;
                    res.ResponseDescription = "Failed";
                }

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

        public FindLocationRes FindLocation(int Id)
        {
            FindLocationRes res = new FindLocationRes();
            try
            {
                res = ServiceHelper.FindLocation(Id);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllLocationRes GetAllLocations()
        {
            GetAllLocationRes res = new GetAllLocationRes();
            try
            {
                res = ServiceHelper.GetAllLocations();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllLocationRes GetAllLocationsByClientId(int clientId)
        {
            GetAllLocationRes res = new GetAllLocationRes();
            try
            {
                res = ServiceHelper.GetAllLocationsByClientId(clientId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetAllLocationRes GetAllLocationsByAgentId(int agentId)
        {
            GetAllLocationRes res = new GetAllLocationRes();
            try
            {
                res = ServiceHelper.GetAllLocationsByAgentId(agentId);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        #endregion

        #region wallet
        public FindWalletRes GetWallet(string email)
        {
            FindWalletRes res = new FindWalletRes();
            try
            {
                res = ServiceHelper.GetWallet(email);

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

        public Response FundWallet(Model.Payment payment)
        {
            Response res = new Response();
            try
            {
                if (res == null)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = "Invalid Request";

                    return res;
                }
                bool result = ServiceHelper.FundWallet(payment);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Amount Added To Wallet Succesfully";

                return res;
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.InnerException.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }

        #endregion

        #region Invoice
        public GetUserInvoicesRes GetUserInvoices(string email)
        {
            GetUserInvoicesRes res = new GetUserInvoicesRes();
            try
            {

                res = ServiceHelper.GetUserInvoices(email);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetUserInvoicesRes GetAllInvoices()
        {
            GetUserInvoicesRes res = new GetUserInvoicesRes();
            try
            {

                res = ServiceHelper.GetAllInvoices();
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public Model.Invoice GetInvoiceById(int invoiceid)
        {
            Model.Invoice res = new Model.Invoice();
            try
            {
                res = ServiceHelper.GetInvoiceById(invoiceid);

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

        public GetInvoicePaymentRes GetTransactionDetails(PaymentReq payReq)
        {
            GetInvoicePaymentRes res = new GetInvoicePaymentRes();
            try
            {

                res = ServiceHelper.GetTransactionDetails(payReq.InvoiceId, payReq.Email);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public GetInvoicePaymentRes GetAllTransactionDetailsByUser(PaymentReq payReq)
        {
            GetInvoicePaymentRes res = new GetInvoicePaymentRes();
            try
            {

                res = ServiceHelper.GetAllTransactionDetailsByUser(payReq.Email);
                res.ResponseCode = ResponseHelper.SUCCESS;
                res.ResponseDescription = "Successful";

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

        public Response MakePayment(PaymentReq payReq)
        {
            Response res = new Response();
            string msg = String.Empty;
            try
            {
                bool result = ServiceHelper.MakePayment(payReq, out msg);
                if (!result)
                {
                    res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
                    res.ResponseDescription = msg;
                    return res;
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Payment Completed Succesfully";
                    return res;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
        }
        #endregion

        #region Taxpayer
        //public TaxpayersRes GetAllTaxpayers()
        //{
        //    TaxpayersRes res = new TaxpayersRes();
        //    try
        //    {

        //        res = ServiceHelper.GetAllTaxpayers();
        //        res.ResponseCode = ResponseHelper.SUCCESS;
        //        res.ResponseDescription = "Successful";

        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
        //        res.ResponseDescription = ex.Message;
        //        Logger.logToFile(ex, ErrorLogPath);
        //        return res;
        //    }
        //}

        //public Response AddTaxpayer(Taxpayer taxpayer)
        //{
        //    Response res = new Response();
        //    try
        //    {
        //        bool result = ServiceHelper.AddTaxpayer(taxpayer);
        //        if (!result)
        //        {
        //            res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
        //            res.ResponseDescription = "Operation Not Succesful";
        //            return res;
        //        }
        //        else
        //        {
        //            res.ResponseCode = ResponseHelper.SUCCESS;
        //            res.ResponseDescription = "Taxpayer Added Succesfully";
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
        //        res.ResponseDescription = ex.Message;
        //        Logger.logToFile(ex, ErrorLogPath);
        //        return res;
        //    }
        //}

        //public Model.Taxpayer GetTaxpayerById(int userid)
        //{
        //    Model.Taxpayer res = new Model.Taxpayer();
        //    try
        //    {
        //        res = ServiceHelper.GetTaxpayerById(userid);

        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
        //        res.ResponseDescription = ex.Message;
        //        Logger.logToFile(ex, ErrorLogPath);
        //        return res;
        //    }
        //}

        //public AssessmentModelRes GetAssessmentByRole(AssessmentReq assessmentReq)
        //{
        //    AssessmentModelRes res = new AssessmentModelRes();
        //    try
        //    {
        //        res = ServiceHelper.GetAssessmentByRole(assessmentReq);

        //        return res;
        //    }
        //    catch (Exception ex)
        //    {
        //        res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
        //        res.ResponseDescription = ex.Message;
        //        Logger.logToFile(ex, ErrorLogPath);
        //        return res;
        //    }
        //}

        //public Response GenerateInvoice(GenerateInvoice generateInvoice)
        //{
        //    Response res = new Response();
        //    try
        //    {
        //        bool result = ServiceHelper.GenerateInvoice(generateInvoice);
        //        if (!result)
        //        {
        //            res.ResponseCode = ResponseHelper.VALIDATION_ERROR;
        //            res.ResponseDescription = "Operation Not Succesful";
        //            return res;
        //        }
        //        else
        //        {
        //            res.ResponseCode = ResponseHelper.SUCCESS;
        //            res.ResponseDescription = "Invoive Generated Succesfully";
        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
        //        res.ResponseDescription = ex.Message;
        //        Logger.logToFile(ex, ErrorLogPath);
        //        return res;
        //    }
        //}
        #endregion
    }
}