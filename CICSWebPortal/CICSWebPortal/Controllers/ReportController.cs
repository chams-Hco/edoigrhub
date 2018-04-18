using CICSWebPortal.Helpers;
using CICSWebPortal.Models;
using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Configuration;
using System.Web.Script.Serialization;
using System.IO;
using Newtonsoft.Json;
using CICSWebPortal.ViewModels;
using System.Globalization;

namespace CICSWebPortal.Controllers
{
    public class ReportController : Controller
    {
        private IDataService DataContext;

        public ReportController() : this(MainContainer.DataService())
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
            var user = (UserDashBoardViewModel)Session["LoggedInUser"];

            ReportFilter filter = new ReportFilter();
            filter.startDate = DateTime.Now.AddDays(-1);
            filter.endDate = DateTime.Now.AddDays(1);


            if (user.RoleCode == "CA" || user.RoleCode == "CU")
            {
                filter.clientId = ClientId;
            }
            if (user.RoleCode == "CU" || user.RoleCode == "AA")
            {
                filter.clientId = ClientId;
                filter.agentId = UserTypeParentId;
            }

            ReportViewModel RVM = DataContext.GetTransactionReportSummary(filter);

            RVM = (RVM != null) ? RVM : new ReportViewModel();

            //Fill the filters
            RVM.clientList = Utility.GetClients(DataContext, RoleId, ClientId).ToList();
            RVM.agentList = Utility.GetAgents(DataContext, RoleId, UserTypeParentId).ToList();
            RVM.terminalList = Utility.GetTerminals(DataContext, RoleId, UserTypeParentId).ToList();
            RVM.ministryList = Utility.GetMDAs(DataContext, RoleId, ClientId).ToList();
            RVM.revenueList = Utility.GetRevenues(DataContext, RoleId, ClientId).ToList();

            if (user.RoleCode == "CA" || user.RoleCode == "CU")
            {
                RVM.SelectedClientId = ClientId;
            }
            if (user.RoleCode == "AA" || user.RoleCode == "AU")
            {
                RVM.SelectedAgentId = UserTypeParentId;
                RVM.SelectedClientId = ClientId;
            }

            RVM.TotalTransactionValue = RVM.Report != null ? RVM.Report.Sum(x => x.Amount) : 0M;
            RVM.StartDate = filter.startDate;
            RVM.EndDate = filter.endDate;
            TempData["myPrint"] = RVM.Report;

            return View(RVM);
        }

        public ActionResult ExportToExcel()
        {
            IEnumerable<Report> report = ((IEnumerable<Report>)TempData["myReport"]);
            if (report != null && report.Count() > 0)
            {
                DataContext.GenerateExcelReport(report);
            }
            return RedirectToAction("Index", "Report");
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

            RVM = (RVM != null) ? RVM : new ReportViewModel();

            RVM.clientList = Utility.GetClients(DataContext, RoleId, ClientId).ToList();
            RVM.agentList = Utility.GetAgents(DataContext, RoleId, UserTypeParentId).ToList();
            RVM.terminalList = Utility.GetTerminals(DataContext, RoleId, UserTypeParentId).ToList();
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

            RVM.TotalTransactionValue = RVM.Report != null ? RVM.Report.Sum(x => x.Amount) : 0M;
            RVM.StartDate = filter.startDate;
            RVM.EndDate = filter.endDate;

            TempData["myReport"] = RVM.Report;

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
            else
            {
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

        public ActionResult EndOfDayReport()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            ReportFilter filter = new ReportFilter();

            filter.startDate = DateTime.Now.AddDays(-1);
            filter.endDate = DateTime.Now;
            filter.TerminalIds = Utility.GetTerminalsByCode(DataContext, RoleId, UserTypeParentId).ToList().Select(a => a.Value).ToList();

            ViewModels.EndofDayViewModel EODVM = DataContext.GetEODReport(filter);

            EODVM = (EODVM != null) ? EODVM : new ViewModels.EndofDayViewModel();

            //Fill the filters
            //EODVM.clientList = Utility.GetClients(DataContext, RoleId, ClientId).ToList();
            EODVM.agentList = Utility.GetAgents(DataContext, RoleId, UserTypeParentId).ToList();
            EODVM.terminalList = Utility.GetTerminalsByCode(DataContext, RoleId, UserTypeParentId).ToList();
            EODVM.statusList = Utility.GetStatusList();

            EODVM.TotalTransactionValue = EODVM.EODReport != null ? EODVM.EODReport.Sum(x => x.Amount) : 0M;
            EODVM.StartDate = filter.startDate;
            EODVM.EndDate = filter.endDate;
            TempData["eodTempData"] = EODVM.EODReport;
            return View(EODVM);
        }

        [HttpPost]
        public ActionResult GetEndOfDayReport(EodFilter modelfilter)
        {
            ReportFilter filter = new ReportFilter
            {
                agentId = modelfilter.agentId,
                clientId = modelfilter.clientId,
                endDate = Convert.ToDateTime(DateTime.ParseExact(modelfilter.endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                ministry = modelfilter.ministry,
                RevenueCode = modelfilter.RevenueCode,
                startDate = Convert.ToDateTime(DateTime.ParseExact(modelfilter.startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                Status = modelfilter.Status,
                Terminal = modelfilter.Terminal,
                terminalId = modelfilter.terminalId,
                TerminalIds = modelfilter.TerminalIds
            };

            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            if (filter.terminalId == null)
            {
                filter.TerminalIds = Utility.GetTerminalsByCode(DataContext, RoleId, UserTypeParentId).ToList().Select(a => a.Value).ToList();
            }

            ViewModels.EndofDayViewModel EODVM = DataContext.GetEODReport(filter);

            EODVM = (EODVM != null) ? EODVM : new ViewModels.EndofDayViewModel();

            EODVM.agentList = Utility.GetAgents(DataContext, RoleId, UserTypeParentId).ToList();
            EODVM.terminalList = Utility.GetTerminalsByCode(DataContext, RoleId, UserTypeParentId).ToList();
            EODVM.statusList = Utility.GetStatusList();

            EODVM.TotalTransactionValue = EODVM.EODReport != null ? EODVM.EODReport.Sum(x => x.Amount) : 0M;
            EODVM.StartDate = filter.startDate;
            EODVM.EndDate = filter.endDate;
            TempData["eodTempData"] = EODVM.EODReport;

            return View(EODVM);
        }

        [HttpGet]
        public JsonResult AgentSummaryChart()
        {
            int clientId = Convert.ToInt32(Session["UserTypeParentId"]);
            var res = DataContext.GetAgentReportSummary(clientId);

            var result = res.Select(x => new AgentChartData
            {
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
            var currentUser = (UserDashBoardViewModel)Session["LoggedInUser"];
            var executiveDashboard = DataContext.GetExecutiveDashBoardSummary(Convert.ToInt32(roleId), Convert.ToInt32(userId), currentUser.RoleCode);
            if (executiveDashboard.AgentLeaderStats != null && executiveDashboard.RevenueLeaderStats != null)
                return View(executiveDashboard);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PeriodicDashboard()
        {
            var roleId = Session["RoleId"];
            var userId = Session["UserId"];
            var currentUser = (UserDashBoardViewModel)Session["LoggedInUser"];

            ExecutiveDashboard result = new ExecutiveDashboard();
            result = DataContext.GetPeriodicDashboardSummary(Convert.ToInt32(roleId), Convert.ToInt32(userId), currentUser.RoleCode, DateTime.Now.AddDays(-7), DateTime.Now);
            return View(result);
        }
        public ActionResult PeriodicDashBoardData(PeriodicDashboardFilter filter)
        {
            var roleId = Session["RoleId"];
            var userId = Session["UserId"];
            var currentUser = (UserDashBoardViewModel)Session["LoggedInUser"];
            filter.roleId = Convert.ToInt32(roleId);
            filter.userId = Convert.ToInt32(userId);

            ExecutiveDashboard result = new ExecutiveDashboard();
            result = DataContext.GetPeriodicDashboardSummary(filter.roleId.Value, filter.userId.Value, currentUser.RoleCode, filter.startDate, filter.endDate);
            return View(result);
        }


        public ActionResult CollectionAgentsTransactions()
        {
            var roleId = Session["RoleId"];
            var userId = Session["UserId"];
            var currentUser = (UserDashBoardViewModel)Session["LoggedInUser"];

            var roles = new string[] { "SA", "SU", "CA", "CU", "WA" };
            if (currentUser == null || !roles.Contains(currentUser.RoleCode))
            {

                Response.Redirect("~/Account/login");
            }


            if (currentUser.RoleCode == "SA" || currentUser.RoleCode == "SU")
            {
                ReportViewModel model = new ReportViewModel
                {
                    StartDate = DateTime.Today.AddDays(-7),
                    EndDate = DateTime.Today.AddDays(1),
                    clientList = DataContext.GetAllClients().Where(a => a.HasWebUsers == true).Select(a => new SelectListItem { Text = a.ClientName, Value = a.clientId.ToString() }),
                    agentList = DataContext.GetAllAgents().Select(a => new SelectListItem { Text = a.Name, Value = a.AgentId.ToString() }),

                };
                return View(model);
            }
            else if (currentUser.RoleCode == "CA" || currentUser.RoleCode == "CU")
            {
                ReportViewModel model = new ReportViewModel
                {
                    StartDate = DateTime.Today.AddDays(-7),
                    EndDate = DateTime.Today.AddDays(1),
                    agentList = DataContext.GetAllAgentsByClientId(currentUser.ClientId).Select(a => new SelectListItem { Text = a.Name, Value = a.AgentId.ToString() }),

                };
                return View(model);
            }
            else if (currentUser.RoleCode == "WA")
            {
                ReportViewModel model = new ReportViewModel
                {
                    StartDate = DateTime.Today.AddDays(-7),
                    EndDate = DateTime.Today.AddDays(1),
                };

                return View(model);
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetCollectionAgentTransactions(ReportFilter filter)
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int ClientId = Convert.ToInt32(Session["ClientId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);
            var currentUser = (UserDashBoardViewModel)Session["LoggedInUser"];
            ReportViewModel RVM = null;
            var roles = new string[] { "SA", "SU", "CA", "CU", "WA" };
            if (currentUser == null || !roles.Contains(currentUser.RoleCode))
            {
            }

            if(filter.Terminal != null && filter.Terminal != string.Empty && filter.Terminal!= "")
            {
                var terminal = DataContext.FindTerminalByCode(filter.Terminal);
                filter.terminalId = terminal != null ? terminal.TerminalId : 0;
            }
            
            if (currentUser.RoleCode == "CA" || currentUser.RoleCode =="CU")
            {
                filter.clientId = ClientId;
            }
            if (currentUser.RoleCode == "CU" || currentUser.RoleCode == "AA")
            {
                filter.clientId = ClientId;
                filter.agentId = UserTypeParentId;
            }

            RVM = DataContext.GetTransactionReportSummary(filter);

            RVM = (RVM != null) ? RVM : new ReportViewModel();

            RVM.clientList = Utility.GetClients(DataContext, RoleId, ClientId).ToList();
            RVM.agentList = Utility.GetAgents(DataContext, RoleId, UserTypeParentId).ToList();
            RVM.terminalList = Utility.GetTerminals(DataContext, RoleId, UserTypeParentId).ToList();
            RVM.ministryList = Utility.GetMDAs(DataContext, RoleId, ClientId).ToList();
            RVM.revenueList = Utility.GetRevenues(DataContext, RoleId, ClientId).ToList();

            

            RVM.TotalTransactionValue = RVM.Report != null ? RVM.Report.Sum(x => x.Amount) : 0M;
            RVM.StartDate = filter.startDate;
            RVM.EndDate = filter.endDate;

            TempData["myReport"] = RVM.Report;

            return View(RVM);
        }



        public class EOD
        {
            public int userId { get; set; }
            public string id { get; set; }
        }

        public class CollectionReportFilter
        {
            public int? lgaId { get; set; }
            public int? clientid { get; set; }
            public string terminalCode { get; set; }
            public string startdate { get; set; }
            public string enddate { get; set; }

        }
    }
}
