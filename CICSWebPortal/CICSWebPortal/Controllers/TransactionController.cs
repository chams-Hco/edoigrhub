using CICSWebPortal.Global;
using CICSWebPortal.Models;
using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class TransactionController : Controller
    {

        private IDataService DataContext;

        public TransactionController():this(MainContainer.DataService())
        {
            
        }

        public TransactionController(IDataService DataContext)     
        {
            this.DataContext = DataContext;
        }

        // GET: Last 1000 Transaction
        public ActionResult Index()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);
            DateTime Today = DateTime.Now;
            DateTime StartDate = new DateTime(Today.Year, Today.Month, Today.Day, 0, 0, 0);
            DateTime EndDate = new DateTime(Today.Year, Today.Month, Today.Day, 23, 59, 59);

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.RoleId = Session["RoleId"];
            ViewBag.UserTypeId = Session["UserTypeParentId"];

            try
            {
                ViewBag.Message = "";
                switch (RoleId)
                {
                    case 1:
                        return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, RequireDateFilter = true, StartDate = StartDate, EndtDate = EndDate }));
                    case 2:
                        return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, RequireDateFilter = true, StartDate = StartDate, EndtDate = EndDate }));
                    case 3:
                        return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, UserType = UserType.Client, RequireDateFilter = true, UserTypeId = UserTypeParentId, StartDate = StartDate, EndtDate = EndDate }));
                    case 4:
                        return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, UserType = UserType.Client, RequireDateFilter = true, UserTypeId = UserTypeParentId, StartDate = StartDate, EndtDate = EndDate }));
                    case 5:
                        return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, UserType = UserType.Agent, RequireDateFilter = true, UserTypeId = UserTypeParentId, StartDate = StartDate, EndtDate = EndDate }));
                    case 6:
                        return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, UserType = UserType.Agent, RequireDateFilter = true, UserTypeId = UserTypeParentId, StartDate = StartDate, EndtDate = EndDate }));
                    default:
                        return View(new List<Transaction> { });
                }

            }
            catch (Exception exp)
            {
                ViewBag.Message = "Error: Unable to display requested data / report query. Please try again or Contact Administrator. " +
                    exp.Message.Substring(0, 15);
                return View(new List<Transaction> { });
            }
        }

        // GET: All Transaction
        public ActionResult TransactionsFilter(TransactionFilter filter)
        {
            DateTime StartDate = new DateTime(filter.startDate.Year, filter.startDate.Month, filter.startDate.Day, 0, 0, 0);
            DateTime EndDate = new DateTime(filter.endDate.Year, filter.endDate.Month, filter.endDate.Day, 23, 59, 59);

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            try
            {
                ViewBag.Message = "";

                if (filter.RoleId != null)
                {

                    switch (filter.RoleId)
                    {
                        case 1:
                            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, RequireDateFilter = true, StartDate = StartDate, EndtDate = EndDate }));
                        case 2:
                            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, RequireDateFilter = true, StartDate = StartDate, EndtDate = EndDate }));
                        case 3:
                            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = UserType.Client, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate }));
                        case 4:
                            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = UserType.Client, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate }));
                        case 5:
                            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = UserType.Agent, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate }));
                        case 6:
                            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = UserType.Agent, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate }));
                        default:
                            return View(new List<Transaction> { });
                    }
                }
                else
                {
                    return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = filter.UserType, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate }));
                }

            }
            catch (Exception exp)
            {
                ViewBag.Message = "Error: Unable to display requested data / report query. Please try again or Contact Administrator. "+
                    exp.Message.Substring(0,15);
                return View(new List<Transaction> { });
            }
        }

        public ActionResult TransactionByClient(int id)
        {
            DateTime Today = DateTime.Now;
            DateTime StartDate = new DateTime(Today.Year, Today.Month, Today.Day, 0, 0, 0);
            DateTime EndDate = new DateTime(Today.Year, Today.Month, Today.Day, 23, 59, 59);

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.Id = id;
            ViewBag.UserType = UserType.Client;

            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 1000, UserType = UserType.Client, RequireDateFilter = true, UserTypeId = id, StartDate = StartDate, EndtDate = EndDate }));
        }

        public ActionResult TransactionByAgent(int id)
        {
            DateTime Today = DateTime.Now;
            DateTime StartDate = new DateTime(Today.Year, Today.Month, Today.Day, 0, 0, 0);
            DateTime EndDate = new DateTime(Today.Year, Today.Month, Today.Day, 23, 59, 59);

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.Id = id;
            ViewBag.UserType = UserType.Agent;

            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 1000, UserType = UserType.Agent, RequireDateFilter = true, UserTypeId = id, StartDate = StartDate, EndtDate = EndDate }));
        }

        public ActionResult TransactionByTerminal(int id)
        {
            DateTime Today = DateTime.Now;
            DateTime StartDate = new DateTime(Today.Year, Today.Month, Today.Day, 0, 0, 0);
            DateTime EndDate = new DateTime(Today.Year, Today.Month, Today.Day, 23, 59, 59);

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.Id = id;
            ViewBag.UserType = UserType.Terminal;

            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 1000, UserType = UserType.Terminal, RequireDateFilter = true, UserTypeId = id, StartDate = StartDate, EndtDate = EndDate }));
        }

        public ActionResult TransactionByLocation(int id)
        {
            DateTime Today = DateTime.Now;
            DateTime StartDate = new DateTime(Today.Year, Today.Month, Today.Day, 0, 0, 0);
            DateTime EndDate = new DateTime(Today.Year, Today.Month, Today.Day, 23, 59, 59);

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.Id = id;
            ViewBag.UserType = UserType.Location;

            return View(DataContext.GetAllTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 1000, UserType = UserType.Location, RequireDateFilter = true, UserTypeId = id, StartDate = StartDate, EndtDate = EndDate }));
        }

        public ActionResult TransactionDetails(int id)
        {
            return View(DataContext.FindTransactionById(id));
        }

        public ActionResult TransactionReceipt(int id)
        {
            return View(DataContext.FindTransactionById(id));
        }
    }
}
