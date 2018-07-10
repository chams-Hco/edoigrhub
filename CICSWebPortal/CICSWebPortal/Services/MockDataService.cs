using CICSWebPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using CICSWebPortal.ViewModels;
using System.Web.Helpers;

namespace CICSWebPortal.Services
{
    public class MockDataService : IDataService
    {
        List<Client> Clients = null;
        List<Agent> Agents = null;
        List<Terminal> Terminals = null;
        List<Transaction> Transactions = null;
        List<Revenue> Revenues = null;
        List<Role> Roles = null;
        List<Identity> Identity = null;
        List<Location> Locations = null;

        public MockDataService()
        {
            Clients = new List<Client> { new Client{ClientId=1,ClientName="Scott Tiger",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true
            },
            new Client{ClientId=2,ClientName="Scott Tiger",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true},
            new Client{ClientId=3,ClientName="Scott Tiger",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true},
            new Client{ClientId=4,ClientName="Scott Tiger",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true},
            new Client{ClientId=5,ClientName="Scott Tiger",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true},
            new Client{ClientId=6,ClientName="Scott Tiger",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true},
            new Client{ClientId=7,ClientName="Scott Tiger",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true}
            };




            Agents = new List<Agent> { new Agent{Code="1000",Company="Konga Nieria Ltd",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true,Name="Samson Emmanuel"
            },
            new Agent{Code="2000",Company="Konga Nieria Ltd",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true,Name="Samson Emmanuel"
            },
            new Agent{Code="3000",Company="Konga Nieria Ltd",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true,Name="Samson Emmanuel"
            },
            new Agent{Code="4000",Company="Konga Nieria Ltd",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true,Name="Samson Emmanuel"
            },
            new Agent{Code="5000",Company="Konga Nieria Ltd",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true,Name="Samson Emmanuel"
            },
            new Agent{Code="6000",Company="Konga Nieria Ltd",
                Address="34, Raypower Road, Agbado",Email="scott@gmail.com",PhoneNo1="08035678990",
                PhoneNo2="090455555555",Status=true,Name="Samson Emmanuel"
            },
          };

            Locations = new List<Location> {
                new Location { AgentId=7, ClientId=1, LocationCode="100501", LocationDescription="Market 1"},
                new Location { AgentId=7, ClientId=1, LocationCode="100502", LocationDescription="Market 2"},
                new Location { AgentId=7, ClientId=1, LocationCode="100503", LocationDescription="Market 3"},
                new Location { AgentId=7, ClientId=1, LocationCode="100504", LocationDescription="Market 4"},
                new Location { AgentId=7, ClientId=1, LocationCode="100505", LocationDescription="Market 5"},
                new Location { AgentId=8, ClientId=1, LocationCode="100506", LocationDescription="Market 6"},
                new Location { AgentId=8, ClientId=1, LocationCode="100507", LocationDescription="Market 7"},
                new Location { AgentId=8, ClientId=1, LocationCode="100508", LocationDescription="Market 8"},
            };

            Terminals = new List<Terminal> { new Terminal{Code="1000",Name="Terminal1",SerialNumber="AP23455"},
            new Terminal{Code="2000",Name="Terminal1",SerialNumber="AP23455"},
            new Terminal{Code="3000",Name="Terminal1",SerialNumber="AP23455"},
            new Terminal{Code="4000",Name="Terminal1",SerialNumber="AP23455"},
            new Terminal{Code="5000",Name="Terminal1",SerialNumber="AP23455"}}
            ;


            Transactions = new List<Transaction>
            {
            };


            Revenues = new List<Revenue>() {new Revenue{RevenueId=10,ClientId=1,Code="10002",
            Amount=5000,MDA="Ministry of Agric",Name="Farmer's Levy",Status=true },
            new Revenue{RevenueId=20,ClientId=1,Code="10002",
            Amount=5000,MDA="Ministry of Agric",Name="Farmer's Levy",Status=true},
            new Revenue{RevenueId=30,ClientId=1,Code="10002",
            Amount=5000,MDA="Ministry of Agric",Name="Farmer's Levy",Status=true },
            new Revenue{RevenueId=40,ClientId=1,Code="10002",
            Amount=5000,MDA="Ministry of Agric",Name="Farmer's Levy",Status=true },
            new Revenue{RevenueId=50,ClientId=1,Code="10002",
            Amount=5000,MDA="Ministry of Agric",Name="Farmer's Levy",Status=true },
            new Revenue{RevenueId=60,ClientId=1,Code="10002",
            Amount=5000,MDA="Ministry of Agric",Name="Farmer's Levy",Status=true },
            new Revenue{RevenueId=70,ClientId=1,Code="10002",
            Amount=5000,MDA="Ministry of Agric",Name="Farmer's Levy",Status=true }
            };
        }

        #region Clients
        public IList<Client> GetAllClients()
        {
            return Clients;
        }


        public Client FindClientById(int id)
        {
            Client client = Clients.First(e => e.ClientId == id);
            return client;
        }


        public void AddClient(Client client)
        {
            if (client == null)
            { throw new Exception("Client is null"); }

            Clients.Add(client);

        }


        public void UpdateClient(Client client)
        {
            if (client == null)
            { throw new Exception("Client is null"); }
            Client currentclient = Clients.First(e => e.ClientId == client.ClientId);
            int clientIndex = Clients.FindIndex(e => e.ClientId == client.ClientId);
            Clients.Remove(currentclient);
            Clients.Insert(clientIndex, client);
        }


        public void DeleteClient(int id)
        {
            Client client = Clients.First(e => e.ClientId == id);
            Clients.Remove(client);


        }

        #endregion

        #region Agents
        public IList<Agent> GetAllAgents()
        {
            return Agents;
        }

        public Agent FindAgentById(int id)
        {
            Agent agent = Agents.First(e => e.Code == id.ToString());
            return agent;
        }

        public void AddAgent(Agent agent)
        {
            if (agent == null)
            { throw new Exception("Agent is null"); }

            Agents.Add(agent);

        }

        public void UpdateAgent(Agent agent)
        {
            if (agent == null)
            { throw new Exception("Agent is null"); }
            Agent currentagent = Agents.First(e => e.AgentId == agent.AgentId);
            int agentIndex = Agents.FindIndex(e => e.AgentId == agent.AgentId);
            Agents.Remove(currentagent);
            Agents.Insert(agentIndex, agent);

        }

        public void DeleteAgent(int id)
        {
            Agent agent = Agents.First(e => e.Code == id.ToString());
            Agents.Remove(agent);

        }

        public IList<Agent> GetAllAgentsByClientId(int id)
        {
            return Agents.Where(e => e.ClientId == id).ToList();
        }

        #endregion

        #region Terminals
        public IList<Terminal> GetAllTerminals()
        {
            return Terminals;
        }

        public Terminal FindTerminalById(int id)
        {
            return Terminals.FirstOrDefault(e => e.TerminalId == id);
        }

        public IList<Terminal> GetAllTerminalsByAgentId(int id)
        {
            return Terminals.Where(e => e.AgentId == id).ToList();
        }

        public IList<Terminal> GetAllTerminalsByClientId(int id)
        {
            return Terminals.Where(e => e.AgentId == id).ToList();
        }

        #endregion

        #region Transactions
        public IList<Transaction> GetAllTransactions()
        {
            return Transactions;
        }

        public IList<Transaction> GetAllTransactionByClientId(int id)
        {
            return Transactions.Where(e => e.ClientId == id).ToList();
        }

        public IList<Transaction> GetAllTransactionByAgentId(int id)
        {
            return Transactions.Where(e => e.AgentId == id).ToList();
        }

        public IList<Transaction> GetAllTransactionByTerminalId(int id)
        {
            return Transactions.Where(e => e.TerminalId == id).ToList();
        }

        public Transaction FindTransactionById(int id)
        {
            return Transactions.FirstOrDefault(e => e.TransactionId == id);
        }

        public Transaction FindTransactionByCode(string code)
        {
            return Transactions.FirstOrDefault(e => e.TransactionCode == code);
        }

        #endregion

        #region Revenue
        public IList<Revenue> GetAllRevenues()
        {
            return Revenues;
        }

        public Revenue FindRevenueById(int id)
        {
            var revenue = Revenues.FirstOrDefault(e => e.RevenueId == id);

            return new Revenue
            {
                RevenueId = revenue.RevenueId,
                Code = revenue.Code,
                MDA = revenue.MDA,
                Name = revenue.Name,
                Amount = revenue.Amount,
                ClientId = revenue.ClientId,
                Status = revenue.Status
            };
        }

        public void AddRevenue(Revenue revenue)
        {
            if (revenue == null)
            {
                throw new Exception("Revenue is null or empty");
            }
            Revenues.Add(revenue);
        }

        public void UpdateRevenue(Revenue revenue)
        {
            if (revenue == null)
            {
                throw new Exception("Revenue is null or empty");
            }
            var revenueData = Revenues.First(e => e.RevenueId == revenue.RevenueId);
            int revenueIndex = Revenues.FindIndex(e => e.RevenueId == revenue.RevenueId);
            Revenues.Remove(revenueData);
            Revenues.Insert(revenueIndex, revenue);
        }

        public void DeleteRevenue(int id)
        {

            Revenue revenue = Revenues.First(e => e.RevenueId == id);
            Revenues.Remove(revenue);
        }
        #endregion

        #region Identity
        public IList<Identity> GetAllIdentity()
        {
            throw new NotImplementedException();
        }

        public Identity FindIdentityId(int id)
        {
            throw new NotImplementedException();
        }

        public void AddIdentity(Identity revenue)
        {
            throw new NotImplementedException();
        }

        public void UpdateIdentity(Identity revenue)
        {
            throw new NotImplementedException();
        }

        public void DeleteIdentity(int id)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Authentication
        UserDashBoardViewModel IDataService.LoginUser(string email, string password)
        {
            if (email == "anambra_admin@igrhub.com" && password == "password")
            {
                return new UserDashBoardViewModel { UserId = 1, RoleId = 5, ClientId = 1, UserTypeParentId = 7, Email = "anambra_admin@igrhub.com" };
            }

            return new UserDashBoardViewModel { };

        }

        #endregion

        #region Authorization
        public IList<Role> GetAllRoles()
        {
            return Roles;
        }

        #endregion

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

        public Dashboard GetDashBoardSummary(int roleId, int userId)
        {
            throw new NotImplementedException();
        }

        public List<Payment> GetTransactionDetails(int id, string userEmail)
        {
            throw new NotImplementedException();
        }

        public UserDashBoardViewModel LoginUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAllUsers()
        {
            throw new NotImplementedException();
        }

        public User FindUserById(int id)
        {
            throw new NotImplementedException();
        }

        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public IList<AgentSummary> GetAgentReportSummary(int clientId)
        {
            throw new NotImplementedException();
        }

        public IList<ClientSummary> GetClientReportSummary()
        {
            throw new NotImplementedException();
        }

        public ReportViewModel GetTransactionReportSummary(ReportFilter request)
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> FindTransactionByResidentId(string code)
        {
            throw new NotImplementedException();
        }

        public List<Ministry> GetAllMinistryByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public List<RevenueHead> GetAllRevenueHeadByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public List<RevenueHead> GetAllRevenueHeadByMinistryId(int id)
        {
            throw new NotImplementedException();
        }

        public List<RevenueCategory> GetAllRevenueCategoryByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public List<RevenueCategory> GetAllRevenueCategoryByRevenueHeadId(int id)
        {
            throw new NotImplementedException();
        }

        public List<RevenueItem> GetAllRevenueItemByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public List<RevenueItem> GetAllRevenueItemByMinistryId(int id)
        {
            throw new NotImplementedException();
        }

        public List<RevenueItem> GetAllRevenueItemByCategoryId(int id)
        {
            throw new NotImplementedException();
        }

        public List<RevenueItem> GetAllRevenueItemByRevenueHeadId(int id)
        {
            throw new NotImplementedException();
        }

        public User FindUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public void ResetUserPassword(int Id)
        {
            throw new NotImplementedException();
        }

        public void ChangeUserPassword(int UserId, string UserEmail, string OldPassword, string NewPassword)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserStatus(User user)
        {
            throw new NotImplementedException();
        }

        public List<Ministry> GetAllMinistry()
        {
            throw new NotImplementedException();
        }

        public List<RevenueHead> GetAllRevenueHead()
        {
            throw new NotImplementedException();
        }

        public IList<User> GetUserAssesibleUsers(int roleId, int clientId)
        {
            throw new NotImplementedException();
        }

        public void ChangeUserPassword(ChangeUserPasswordModel user)
        {
            throw new NotImplementedException();
        }

        public void ResetUserPassword(ResetPasswordModel user)
        {
            throw new NotImplementedException();
        }

        public List<Notification> GetAllNotifications()
        {
            throw new NotImplementedException();
        }

        public List<AuditTrail> GetAllAuditTrail()
        {
            throw new NotImplementedException();
        }

        public ExecutiveDashboard GetExecutiveDashBoardSummary(int roleId, int userId)
        {
            throw new NotImplementedException();
        }

        public ExecutiveDashboard GetPeriodicDashboardSummary(int roleId, int userId, DateTime StartDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> GetLast1000Transactions()
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> GetLast1000TransactionByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> GetLast1000TransactionByAgentId(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> GetLast1000TransactionByTerminalId(int id)
        {
            throw new NotImplementedException();
        }

        public IList<ErrorTransaction> GetAllErrorTransactions()
        {
            throw new NotImplementedException();
        }

        public IList<ErrorTransaction> GetAllErrorTransactionByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public IList<ErrorTransaction> GetAllErrorTransactionByAgentId(int id)
        {
            throw new NotImplementedException();
        }

        public IList<ErrorTransaction> GetAllErrorTransactionByTerminalId(int id)
        {
            throw new NotImplementedException();
        }

        public ErrorTransaction FindErrorTransactionById(int id)
        {
            throw new NotImplementedException();
        }

        public ErrorTransaction FindErrorTransactionByCode(string code)
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> GetAllTransactions(GetTransactionRequest req)
        {
            throw new NotImplementedException();
        }

        public IList<ErrorTransaction> GetAllErrorTransactions(GetTransactionRequest req)
        {
            throw new NotImplementedException();
        }

        public ViewModels.EndofDayViewModel GetEODReport(ReportFilter request)
        {
            throw new NotImplementedException();
        }

        #region Location
        public IList<Location> GetAllLocations()
        {
            return Locations.ToList();
        }

        public IList<Location> GetAllLocationsByClientId(int userTypeParentId)
        {
            return Locations.Where(e => e.ClientId == userTypeParentId).ToList();
        }

        public IList<Location> GetAllLocationsByAgentId(int userTypeParentId)
        {
            return Locations.Where(e => e.ClientId == userTypeParentId).ToList();
        }

        public Location FindLocationById(int id)
        {
            Location location = Locations.First(e => e.Id == id);
            return location;
        }

        public string AddLocation(Location location)
        {
            if (location == null)
            { throw new Exception("Location is null"); }

            Locations.Add(location);

            return "";
        }
        public string UpdateLocation(Location location)
        {
            if (location == null)
            { throw new Exception("Location is null"); }
            Location currentlocation = Locations.First(e => e.Id == location.Id);
            int locationtIndex = Locations.FindIndex(e => e.Id == location.Id);
            Locations.Remove(currentlocation);
            Locations.Insert(locationtIndex, location);

            return "";
        }
        public string DeleteLocation(int id)
        {
            Location location = Locations.First(e => e.Id == id);
            Locations.Remove(location);
            return "";
        }

        public Wallet GetWallet(string email)
        {
            throw new NotImplementedException();
        }

        public bool FundWallet(Payment payment)
        {
            throw new NotImplementedException();
        }

        public List<InvoiceItem> GetInvoices(string userEmail)
        {
            throw new NotImplementedException();
        }

        public List<Invoice> GetUserInvoices(string userEmail)
        {
            throw new NotImplementedException();
        }

        public Invoice GetInvoiceById(string invoiceid)
        {
            throw new NotImplementedException();
        }

        public Invoice GetInvoiceById(int invoiceid)
        {
            throw new NotImplementedException();
        }

        public bool MakePayment(string payment_amount, int invoiceId, string userEmail)
        {
            throw new NotImplementedException();
        }

        public List<Invoice> GetAllInvoices()
        {
            throw new NotImplementedException();
        }

        public List<Biodata> GetAllTaxPayers()
        {
            throw new NotImplementedException();
        }

        List<Models.Biodata> IDataService.GetAllTaxPayers()
        {
            throw new NotImplementedException();
        }

        public bool AddTaxpayer(Taxpayer taxpayer)
        {
            throw new NotImplementedException();
        }

        public Taxpayer GetTaxpayerById(int id)
        {
            throw new NotImplementedException();
        }

        public Invoice GetAssessmentByRole(int roleid)
        {
            throw new NotImplementedException();
        }

        public bool GenerateInvoice(GenerateInvoice generateInvoice)
        {
            throw new NotImplementedException();
        }

        public List<RevenueItem> GetAssessmentByRole(AssessmentReq assessmentReq)
        {
            throw new NotImplementedException();
        }

        public bool MakePayment(PaymentReq payment)
        {
            throw new NotImplementedException();
        }

        public List<Payment> GetAllTransactionDetailsByUser(int id, string userEmail)
        {
            throw new NotImplementedException();
        }

        Dashboard IDataService.GetTaxpayerDashboardSummary(int roleId, int userId, string userEmail)
        {
            throw new NotImplementedException();
        }

        public void GenerateExcelReport(IEnumerable<Report> report)
        {
            throw new NotImplementedException();
        }

        public IList<Client> GetAllClientWithZones()
        {
            throw new NotImplementedException();
        }

        public Client FindClientWithZone(int id)
        {
            throw new NotImplementedException();
        }

        public IList<User> GetAllUsersByClientId(int id)
        {
            throw new NotImplementedException();
        }

        public void AddWebUser(WebUserViewModel user)
        {
            throw new NotImplementedException();
        }

        WebUser IDataService.AddWebUser(WebUserViewModel user)
        {
            throw new NotImplementedException();
        }

        public Role FindRole(int ID)
        {
            throw new NotImplementedException();
        }

        public Role FindRoleByCode(string code)
        {
            throw new NotImplementedException();
        }

        public Task<Client> FindClientByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public WebTransactionResponse ProcessWebTrancation(WebPayment webPayment)
        {
            throw new NotImplementedException();
        }

        public IList<Transaction> GetLast10TransactionsByTerminalId(int TerminalId)
        {
            throw new NotImplementedException();
        }

        public ExecutiveDashboard GetExecutiveDashBoardSummary(int roleId, int userId, string roleCode)
        {
            throw new NotImplementedException();
        }

        public ExecutiveDashboard GetPeriodicDashboardSummary(int roleId, int userId, string roleCode, DateTime StartDate, DateTime EndDate)
        {
            throw new NotImplementedException();
        }

        public Dashboard GetDashBoardSummary(int roleId, int userId, string roleCode)
        {
            throw new NotImplementedException();
        }

        public Terminal FindTerminalByCode(string code)
        {
            throw new NotImplementedException();
        }

        public VerifyResident FindResident(VerifyResidentRequest req)
        {
            throw new NotImplementedException();
        }

        public CashWoxIntegrationResponse SendInterswitchInvoice(CashWoxModel cashwoxmodel)
        {
            throw new NotImplementedException();
        }

        public WebTransactionResponse ProcessMultiWebTrancation(List<WebPayment> webPayment)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

