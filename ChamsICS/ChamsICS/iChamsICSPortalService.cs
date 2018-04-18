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
        #region Agents & Clients
        [OperationContract]
        CreateClientRes CreateClient(CreateClientReq req);
        [OperationContract]
        Response UpdateClient(UpdateClientReq req);
        [OperationContract]
        FindClientRes FindClient(int clientId);
        [OperationContract]
        FindClientWithZoneRes FindClientWithZones(int clientId);
        [OperationContract]
        GetAllClientsRes GetAllClient();
        [OperationContract]
        GetAllClientsWithZoneRes GetAllClientWithZones();
        [OperationContract]
        CreateAgentRes CreateAgent(Model.Agent req);
        [OperationContract]
        Response UpdateAgent(Model.Agent req);
        [OperationContract]
        FindAgentRes FindAgent(int Id);
        [OperationContract]
        GetAllAgentRes GetAllAgents();
        [OperationContract]
        GetAllAgentRes GetAllAgentsByClientId(int clientId);

        #endregion

        #region Terminals
        [OperationContract]
        Response UpdateTerminalStatus(TerminalStatus req);
        [OperationContract]
        FindTerminalRes FindTerminal(int Id);
        [OperationContract]
        FindTerminalRes FindTerminalByCode(string code);
        [OperationContract]
        GetAllTerminalRes GetAllTerminals();
        [OperationContract]
        GetAllTerminalRes GetAllTerminalsByAgentId(int AgentId);
        [OperationContract]
        GetAllTerminalRes GetAllTerminalsByClientId(int ClientId);
        #endregion

        #region Transactions
        [OperationContract]
        FindTransactionRes FindTransaction(int Id);
        [OperationContract]
        FindTransactionRes FindTransactionByCode(string transactionCode);
        [OperationContract]
        GetAllTransactionRes GetTransactions(GetTransactionRequest req);
        [OperationContract]
        GetAllTransactionRes GetAllTransaction();
        [OperationContract]
        GetAllTransactionRes GetAllTransactionByClientId(int clientId);
        [OperationContract]
        GetAllTransactionRes GetAllTransactionByAgentId(int agentId);
        [OperationContract]
        GetAllTransactionRes GetAllTransactionByTerminalId(int terminalId);
        [OperationContract]
        GetAllTransactionRes GetAllTransactionByTerminalPerPeriod(PeriodicTransactionRequest periodicTransactionRequest);
        [OperationContract]
        GetAllTransactionRes GetAllTransactionByAgentPerPeriod(PeriodicTransactionRequest periodicTransactionRequest);
        [OperationContract]
        GetAllTransactionRes GetAllTransactionByLocationId(int locationId);
        [OperationContract]
        GetAllTransactionRes GetLast1000Transaction();
        [OperationContract]
        GetAllTransactionRes GetLast1000TransactionByClientId(int clientId);
        [OperationContract]
        GetAllTransactionRes GetLast1000TransactionByAgentId(int agentId);
        [OperationContract]
        GetAllTransactionRes GetLast1000TransactionByTerminalId(int terminalId);
        [OperationContract]
        GetAllTransactionRes GetLast10TransactionByTerminalId(int terminalId);
        [OperationContract]
        GetAllTransactionRes GetAllTransactionByResidentId(string residentId);
        #endregion

        #region webUsers
        [OperationContract]
        WebTransactionRes ProcessWebTrancation(WebTransactionReq webTransactionReq);

        #endregion

        #region Error Upload Transactions
        [OperationContract]
        FindErrorTransactionRes FindErrorTransaction(int Id);
        [OperationContract]
        FindErrorTransactionRes FindErrorTransactionByCode(string transactionCode);
        [OperationContract]
        GetAllErrorTransactionRes GetErrorTransaction(GetTransactionRequest req);
        [OperationContract]
        GetAllErrorTransactionRes GetAllErrorTransaction();
        [OperationContract]
        GetAllErrorTransactionRes GetAllErrorTransactionByClientId(int clientId);
        [OperationContract]
        GetAllErrorTransactionRes GetAllErrorTransactionByAgentId(int agentId);
        [OperationContract]
        GetAllErrorTransactionRes GetAllErrorTransactionByTerminalId(int terminalId);
        #endregion

        #region Revenue
        [OperationContract]
        CreateRevenueRes CreateRevenue(Model.Revenue req);
        [OperationContract]
        Response UpdateRevenue(Model.Revenue req);
        [OperationContract]
        FindRevenueRes FindRevenue(int Id);

        [OperationContract]
        GetAllRevenueRes GetAllRevenues();
        [OperationContract]
        List<Model.Ministry> GetAllMinistry();
        [OperationContract]
        List<Model.RevenueHead> GetAllRevenueHead();
        [OperationContract]
        List<Model.Ministry> GetAllMinistryByClientId(int id);
        [OperationContract]
        List<Model.RevenueHead> GetAllRevenueHeadByClientId(int id);
        [OperationContract]
        List<Model.RevenueHead> GetAllRevenueHeadByMinistryId(int id);
        [OperationContract]
        List<Model.RevenueCategory> GetAllRevenueCategoryByClientId(int id);
        [OperationContract]
        List<Model.RevenueCategory> GetAllRevenueCategoryByRevenueHeadId(int id);
        [OperationContract]
        List<Model.RevenueItem> GetAllRevenueItemByClientId(int id);
        [OperationContract]
        List<Model.RevenueItem> GetAllRevenueItemByMinistryId(int id);
        [OperationContract]
        List<Model.RevenueItem> GetAllRevenueItemByCategoryId(int id);
        [OperationContract]
        List<Model.RevenueItem> GetAllRevenueItemByRevenueHeadId(int id);
        [OperationContract]
        GetAllRevenueRes GetAllRevenuesByClientId(int clientId);
        #endregion

        #region Identity
        [OperationContract]
        CreateIdentityRes CreateIdentity(Model.Identity req);
        [OperationContract]
        Response UpdateIdentity(Model.Identity req);
        [OperationContract]
        FindIdentityRes FindIdentity(int Id);
        [OperationContract]
        GetAllIdentityRes GetAllIdentity();
        [OperationContract]
        GetAllIdentityRes GetAllIdentityByClientId(int clientId);
        #endregion

        #region User
        [OperationContract]
        CreateUserRes CreateUser(Model.User req);
        [OperationContract]
        UserLoginRes UserLogin(UserLoginReq req);
        [OperationContract]
        Response ResetUserPassword(ResetUserPasswordReq req);
        [OperationContract]
        Response UpdateUserStatus(UserStatus request);
        [OperationContract]
        Response ChangeUserPassword(ChangeUserPassword request);

        [OperationContract]
        RoleRes GetAllRoles();
        [OperationContract]
        FindRoleRes FindRole(int ID);
        [OperationContract]
        FindRoleRes FindRoleByCode(string code);

        [OperationContract]
        GetAllUserRes GetAllUsers();

        [OperationContract]
        GetAllUserRes GetAllUsersByClientId(int id);

        [OperationContract]
        GetAllUserRes GetUserAssesibleUsers(GetAllUserReq req);

        [OperationContract]
        GetAllUserRes GetAllUsersByTypeId(GetAllUserReq req);

        [OperationContract]
        FindUserRes FindUser(int id);

        [OperationContract]
        FindUserRes FindUserByEmail(string email);


        #endregion

        #region Dashboard & Reports
        [OperationContract]
        DashboardRes GetDashboardSummary(DashboardReq req);

        [OperationContract]
        TaxpayerDashboardRes GetTaxpayerDashboardSummary(DashboardReq req);

        [OperationContract]
        ExecutiveDashboardRes GetExecutiveDashboardData(ExecutiveDashboardReq req);

        [OperationContract]
        ExecutiveDashboardRes GetPeriodicDashboardData(ExecutiveDashboardReq req);

        [OperationContract]
        AgentSummaryRes GetAgentsSummary(int clientId);

        [OperationContract]
        ClientSummaryRes GetClientsSummary();

        [OperationContract]
        TransactionSummaryRes GetReportSummary(TransactionSummary request);

        [OperationContract]
        GetNotificationsRes GetAllNotifications();

        [OperationContract]
        GetAuditTrailsRes GetAllAuditTrails();

        [OperationContract]
        FetchEndOfDayRes GetEODReport(FetchEndOfDayReq request);
        #endregion

        #region Location
        [OperationContract]
        CreateLocationRes CreateLocation(Location req);
        [OperationContract]
        Response UpdateLocation(Model.Location req);
        [OperationContract]
        FindLocationRes FindLocation(int Id);
        [OperationContract]
        GetAllLocationRes GetAllLocations();
        [OperationContract]
        GetAllLocationRes GetAllLocationsByClientId(int clientId);
        [OperationContract]
        GetAllLocationRes GetAllLocationsByAgentId(int agentId);
        #endregion

        #region wallet
        [OperationContract]
        FindWalletRes GetWallet(string email);
        [OperationContract]
        bool FundWallet(Payment payment);

        #endregion

        #region Invoices
        [OperationContract]
        GetUserInvoicesRes GetUserInvoices(string email);

        [OperationContract]
        GetUserInvoicesRes GetAllInvoices();

        [OperationContract]
        Invoice GetInvoiceById(int id);

        [OperationContract]
        GetInvoicePaymentRes GetTransactionDetails(PaymentReq paymentReq);

        [OperationContract]
        GetInvoicePaymentRes GetAllTransactionDetailsByUser(PaymentReq paymentReq);

        [OperationContract]
        Response MakePayment(PaymentReq payment);
        #endregion

        #region Taxpayer
        //[OperationContract]
        //TaxpayersRes GetAllTaxpayers();
        //[OperationContract]
        //Response AddTaxpayer(Taxpayer taxpayer);
        //[OperationContract]
        //Taxpayer GetTaxpayerById(int id);
        //[OperationContract]
        //AssessmentModelRes GetAssessmentByRole(AssessmentReq assessmentReq);
        //[OperationContract]
        //Response GenerateInvoice(GenerateInvoice generateInvoice);
        #endregion

    }
}
