using CICSWebPortal.Helpers;
using CICSWebPortal.Models;
using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class ReportController : Controller
    {
        private IDataService DataContext;

        public ReportController():this(MainContainer.DataService())
        {

        }

        public ReportController(IDataService DataContext)
        {
            this.DataContext = DataContext;
        }

        // GET: Report
        public ActionResult Index()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int ClientId = Convert.ToInt32(Session["ClientId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            ReportFilter filter = new ReportFilter();
            filter.startDate = DateTime.Now.AddDays(-1);
            filter.endDate = DateTime.Now.AddDays(1);


            if (RoleId == 3 || RoleId == 4)
            {
                filter.clientId = ClientId;
            }
            if (RoleId == 4 || RoleId == 5)
            {
                filter.clientId = ClientId;
                filter.agentId = UserTypeParentId;
            }

            ReportViewModel RVM = DataContext.GetTransactionReportSummary(filter);

            //Fill the filters
            RVM.clientList = Utility.GetClients(DataContext,RoleId,ClientId).ToList();
            RVM.agentList = Utility.GetAgents(DataContext,RoleId,UserTypeParentId).ToList();
            RVM.terminalList = Utility.GetTerminals(DataContext,RoleId,UserTypeParentId).ToList();
            RVM.ministryList = Utility.GetMDAs(DataContext, RoleId, ClientId).ToList();
            RVM.revenueList = Utility.GetRevenues(DataContext, RoleId, ClientId).ToList();

            if (RoleId == 3 || RoleId == 4)
            {
                RVM.SelectedClientId = ClientId;
            }
            else if (RoleId == 5 || RoleId == 6)
            {
                RVM.SelectedAgentId = UserTypeParentId;
                RVM.SelectedClientId = ClientId;
            }

            RVM.TotalTransactionValue = RVM.Report.Sum(x => x.Amount);
            RVM.StartDate = filter.startDate;
            RVM.EndDate = filter.endDate;

            return View(RVM);
        }

        [HttpPost]
        public ActionResult TransactionSummary(ReportFilter filter)
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int ClientId = Convert.ToInt32(Session["ClientId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            ReportViewModel RVM = null;

            if (RoleId == 3 || RoleId == 4)
            {
                filter.clientId = ClientId;
            }
            if (RoleId == 4 || RoleId == 5)
            {
                filter.clientId = ClientId;
                filter.agentId = UserTypeParentId;
            }

            RVM = DataContext.GetTransactionReportSummary(filter);

            RVM.clientList = Utility.GetClients(DataContext,RoleId,ClientId).ToList();
            RVM.agentList = Utility.GetAgents(DataContext,RoleId, UserTypeParentId).ToList();
            RVM.terminalList = Utility.GetTerminals(DataContext,RoleId,UserTypeParentId).ToList();
            RVM.ministryList = Utility.GetMDAs(DataContext, RoleId, ClientId).ToList();
            RVM.revenueList = Utility.GetRevenues(DataContext, RoleId, ClientId).ToList();

            //if (RoleId == 3 || RoleId == 4)
            //{
            //    RVM.SelectedClientId = ClientId;
            //}
            //else if (RoleId == 5 || RoleId == 6)
            //{
            //    RVM.SelectedAgentId = UserTypeParentId;
            //    RVM.SelectedClientId = ClientId;
            //}

            RVM.TotalTransactionValue = RVM.Report.Sum(x => x.Amount);
            RVM.StartDate = filter.startDate;
            RVM.EndDate = filter.endDate;

            return View(RVM);
        }

        public ActionResult TransactionReceipt()
        {
            return View(new Transaction() { });
        }

        public ActionResult TransactionStatement()
        {
            return View(new List<Transaction>() { });            
        }

        public ActionResult Receipt(string id)
        {
            var receipt = DataContext.FindTransactionByCode(id);
            if (receipt != null)
            {
                return View(DataContext.FindTransactionByCode(id));
            }
            else {
                return View(new Transaction() { });
            }
        }

        public ActionResult Statement(string id)
        {
            return View(DataContext.FindTransactionByResidentId(id).ToList());
        }

        // GET: Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult AgentSummary()
        {
            int clientId = Convert.ToInt32(Session["UserTypeParentId"]);
            return View(DataContext.GetAgentReportSummary(clientId));
        }

        [HttpGet]
        public JsonResult AgentSummaryChart()
        {
            int clientId = Convert.ToInt32(Session["UserTypeParentId"]);
            var res = DataContext.GetAgentReportSummary(clientId);
            
           var result = res.Select(x=> new AgentChartData {
                Agent = x.AgentName,
                terminals = x.TotalTerminal,
                transactions = x.TotalTransactionCount,
                transactionSum = x.TotalTransactionValue
            }).ToList();

            var jsonRes = Json(result, JsonRequestBehavior.AllowGet);
            return jsonRes;
        }

        public ActionResult ClientSummary()
        {
            return View(DataContext.GetClientReportSummary());

        }

        public ActionResult ExecutiveDashBoard()
        {
            var roleId = Session["RoleId"];
            var userId = Session["UserId"];
            return View(DataContext.GetExecutiveDashBoardSummary(Convert.ToInt32(roleId), Convert.ToInt32(userId)));
        }

        public ActionResult PeriodicDashboard()
        {
            var roleId = Session["RoleId"];
            var userId = Session["UserId"];

            ExecutiveDashboard result = new ExecutiveDashboard();
            result = DataContext.GetPeriodicDashboardSummary(Convert.ToInt32(roleId), Convert.ToInt32(userId), DateTime.Now.AddDays(-7), DateTime.Now);
            return View(result);
        }
        public ActionResult PeriodicDashBoardData(PeriodicDashboardFilter filter)
        {
            var roleId = Session["RoleId"];
            var userId = Session["UserId"];
            filter.roleId = Convert.ToInt32(roleId);
            filter.userId = Convert.ToInt32(userId);

            ExecutiveDashboard result = new ExecutiveDashboard();
            result = DataContext.GetPeriodicDashboardSummary(filter.roleId.Value, filter.userId.Value, filter.startDate, filter.endDate);
            return View(result);
        }
    }
}
