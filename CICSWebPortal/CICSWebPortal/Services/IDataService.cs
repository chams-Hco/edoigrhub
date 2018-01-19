using CICSWebPortal.Models;
using CICSWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CICSWebPortal.Services
{
    public interface IDataService
    {
        #region Clients
        IList<Client> GetAllClients();
        Client FindClientById(int id);
        void AddClient(Client client);
        void UpdateClient(Client client);
        void DeleteClient(int id);

        #endregion

        #region Agents
        IList<Agent> GetAllAgents();
        Agent FindAgentById(int id);
        void AddAgent(Agent agent);
        void UpdateAgent(Agent agent);
        void DeleteAgent(int id);

        IList<Agent> GetAllAgentsByClientId(int id);
        #endregion

        #region Terminals
        IList<Terminal> GetAllTerminals();
        Terminal FindTerminalById(int id);
        IList<Terminal> GetAllTerminalsByAgentId(int id);
        IList<Terminal> GetAllTerminalsByClientId(int id);
        #endregion

        #region Transaction
        IList<Models.Transaction> GetAllTransactions(Models.GetTransactionRequest req);
        Transaction FindTransactionById(int id);

        Transaction FindTransactionByCode(string code);

        IList<Models.Transaction> FindTransactionByResidentId(string code);

        #endregion

        #region Upload Error Transaction
        IList<Models.ErrorTransaction> GetAllErrorTransactions(Models.GetTransactionRequest req);
        ErrorTransaction FindErrorTransactionById(int id);
        ErrorTransaction FindErrorTransactionByCode(string code);
        #endregion

        #region Revenue
        IList<Revenue> GetAllRevenues();
        Revenue FindRevenueById(int id);
        void AddRevenue(Revenue revenue);
        void UpdateRevenue(Revenue revenue);
        void DeleteRevenue(int id);

        List<Models.Ministry> GetAllMinistry();
        List<Models.Ministry> GetAllMinistryByClientId(int id);
        List<Models.RevenueHead> GetAllRevenueHead();
        List<Models.RevenueHead> GetAllRevenueHeadByClientId(int id);
        List<Models.RevenueHead> GetAllRevenueHeadByMinistryId(int id);
        List<Models.RevenueCategory> GetAllRevenueCategoryByClientId(int id);
        List<Models.RevenueCategory> GetAllRevenueCategoryByRevenueHeadId(int id);
        List<Models.RevenueItem> GetAllRevenueItemByClientId(int id);
        List<Models.RevenueItem> GetAllRevenueItemByMinistryId(int id);
        List<Models.RevenueItem> GetAllRevenueItemByCategoryId(int id);
        List<Models.RevenueItem> GetAllRevenueItemByRevenueHeadId(int id);
        void GenerateExcelReport(IEnumerable<Models.Report> report);
        #endregion

        #region Authentication
        UserDashBoardViewModel LoginUser(string email, string password);
        #endregion

        #region Authorization

        IList<Role> GetAllRoles();
        #endregion

        #region Identity
        IList<Identity> GetAllIdentity();
        Identity FindIdentityId(int id);
        void AddIdentity(Identity revenue);
        void UpdateIdentity(Identity revenue);
        void DeleteIdentity(int id);
        #endregion

        #region Dashboard
        Dashboard GetDashBoardSummary(int roleId, int userId);
        Dashboard GetTaxpayerDashboardSummary(int roleId, int userId, string userEmail);
        ExecutiveDashboard GetExecutiveDashBoardSummary(int roleId, int userId);
        ExecutiveDashboard GetPeriodicDashboardSummary(int roleId, int userId, DateTime StartDate, DateTime EndDate);
        #endregion

        #region User

        IList<User> GetAllUsers();
        IList<User> GetUserAssesibleUsers(int roleId, int clientId);
        User FindUserById(int id);
        User FindUserByEmail(string email);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        void ResetUserPassword(ResetPasswordModel user);
        void ChangeUserPassword(ChangeUserPasswordModel user);
        void UpdateUserStatus(Models.User user);
        #endregion

        #region Report        
        IList<Models.AgentSummary> GetAgentReportSummary(int clientId);
        IList<Models.ClientSummary> GetClientReportSummary();
        ReportViewModel GetTransactionReportSummary(ReportFilter request);
        #endregion

        #region Location
        IList<Location> GetAllLocations();
        IList<Location> GetAllLocationsByClientId(int userTypeParentId);
        IList<Location> GetAllLocationsByAgentId(int userTypeParentId);
        Location FindLocationById(int id);
        string AddLocation(Location location);
        string UpdateLocation(Location location);
        string DeleteLocation(int id);
        #endregion

        #region Wallet
        Models.Wallet GetWallet(string email);
        bool FundWallet(Models.Payment payment);
        #endregion

        #region Invoice
        List<Models.Invoice> GetUserInvoices(string userEmail);
        List<Models.Invoice> GetAllInvoices();
        Models.Invoice GetInvoiceById(int invoiceid);
        List<Models.Payment> GetTransactionDetails(int id, string userEmail);
        List<Models.Payment> GetAllTransactionDetailsByUser(int id, string userEmail);
        Boolean MakePayment(PaymentReq payment);
        #endregion

        #region Tax Payers
        List<Models.Biodata> GetAllTaxPayers();
        Taxpayer GetTaxpayerById(int id);
        Boolean AddTaxpayer(Taxpayer taxpayer);
        List<Models.RevenueItem> GetAssessmentByRole(AssessmentReq assessmentReq);
        Boolean GenerateInvoice(GenerateInvoice generateInvoice);
        #endregion

        List<Models.Notification> GetAllNotifications();
        List<Models.AuditTrail> GetAllAuditTrail();        
    }
}
