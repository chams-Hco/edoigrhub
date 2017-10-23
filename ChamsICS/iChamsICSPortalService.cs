using ChamsICSWebService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ChamsICSWebService
{

    [ServiceContract(Namespace = "http://portalservices.cics.chams.com")]
    [Description("Chams IGR Consolidation System - CICS. For Technical support, contact itsupport@chams.com ")]
    public interface iChamsICSPortalService
    {
        [OperationContract]
        UserLoginRes UserLogin(UserLoginReq req);    
        [OperationContract]
        CreateClientRes CreateClient(CreateClientReq req);
        [OperationContract]
        FindClientRes FindClient(int clientId);
        [OperationContract]
        GetAllClientsRes GetAllClient();
        [OperationContract]
        CreateAgentRes CreateAgent(Model.Agent req);
        [OperationContract]
        FindAgentRes FindAgent(int Id);
        [OperationContract]
        GetAllAgentRes GetAllAgents();
        [OperationContract]
        GetAllAgentRes GetAllAgentsByClientId(int clientId);
        [OperationContract]
        FindTerminalRes FindTerminal(int Id);
        [OperationContract]
        GetAllTerminalRes GetAllTerminals();
        [OperationContract]
        GetAllTerminalRes GetAllTerminalsByAgentId(int AgentId);
        [OperationContract]
        FindTransactionRes FindTransaction(int Id);
        [OperationContract]
        FindTransactionRes FindTransactionByCode(string transactionCode);
        [OperationContract]
        GetAllTransactionRes GetAllTransaction();
        [OperationContract]
        GetAllTransactionRes GetAllTransactionByClientId(int clientId);
        [OperationContract]
        GetAllTransactionRes GetAllTransactionByAgentId(int agentId);

        [OperationContract]
        GetAllTransactionRes GetAllTransactionByTerminalId(int terminalId);

        [OperationContract]
        CreateRevenueRes CreateRevenue(Model.Revenue req);

        [OperationContract]
        FindRevenueRes FindRevenue(int Id);

        [OperationContract]
        GetAllRevenueRes GetAllRevenues();
        [OperationContract]
        GetAllRevenueRes GetAllRevenuesByClientId(int clientId);

    }
}
