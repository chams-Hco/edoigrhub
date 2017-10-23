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
        public UserLoginRes UserLogin(UserLoginReq req)
        {
            UserLoginRes res = new UserLoginRes();

            return res;
        }

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
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Client Created Succesfully";
                    res.Code = clientCode;
                    res.ClientId = clientId;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }

            return res;
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
                }
                else
                {
                    res.ResponseCode = ResponseHelper.SUCCESS;
                    res.ResponseDescription = "Client Created Succesfully";
                    res.AgentCode = agentCode;
                    res.AgentId = agentId;
                }
            }
            catch (Exception ex)
            {
                res.ResponseCode = ResponseHelper.APPLICATION_ERROR;
                res.ResponseDescription = msg+ ex.Message;
                Logger.logToFile(ex, ErrorLogPath);
                return res;
            }
            return res;
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
                    res.ResponseDescription = "Client Created Succesfully";                    
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
    }
}