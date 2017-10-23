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
    public class UploadErrorController : Controller
    {
        private IDataService DataContext;

        public UploadErrorController() : this(MainContainer.DataService())
        {

        }

        public UploadErrorController(IDataService DataContext)
        {
            this.DataContext = DataContext;
        }

        // GET: UploadError
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
                        return View(DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, RequireDateFilter = true, StartDate = StartDate, EndtDate = EndDate }));
                    case 2:
                        return View(DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, RequireDateFilter = true, StartDate = StartDate, EndtDate = EndDate }));
                    case 3:
                        return View(DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, UserType = UserType.Client, RequireDateFilter = true, UserTypeId = UserTypeParentId, StartDate = StartDate, EndtDate = EndDate }));
                    case 4:
                        return View(DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, UserType = UserType.Client, RequireDateFilter = true, UserTypeId = UserTypeParentId, StartDate = StartDate, EndtDate = EndDate }));
                    case 5:
                        return View(DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, UserType = UserType.Agent, RequireDateFilter = true, UserTypeId = UserTypeParentId, StartDate = StartDate, EndtDate = EndDate }));
                    case 6:
                        return View(DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = 500, UserType = UserType.Agent, RequireDateFilter = true, UserTypeId = UserTypeParentId, StartDate = StartDate, EndtDate = EndDate }));
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

        public ActionResult ErrorTransactionsFilter(ErrorTransactionFilter filter)
        {
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);
            DateTime StartDate = new DateTime(filter.startDate.Year, filter.startDate.Month, filter.startDate.Day, 0, 0, 0);
            DateTime EndDate = new DateTime(filter.endDate.Year, filter.endDate.Month, filter.endDate.Day, 23, 59, 59);

            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            try
            {
                ViewBag.Message = "" ;
                if (filter.RoleId != null)
                {
                    IList<ErrorTransaction> res = null;

                    switch (filter.RoleId)
                    {
                        case 1:
                            res = DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, RequireDateFilter = true, StartDate = StartDate, EndtDate = EndDate });
                            break;
                        case 2:
                            res = DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, RequireDateFilter = true, StartDate = StartDate, EndtDate = EndDate });
                            break;
                        case 3:
                            res = DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = UserType.Client, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate });
                            break;
                        case 4:
                            res = DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = UserType.Client, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate });
                            break;
                        case 5:
                            res = DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = UserType.Agent, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate });
                            break;
                        case 6:
                            res = DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = UserType.Agent, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate });
                            break;
                        default:
                            break;
                    }

                    var statusFilter = filter.FilterData;
                    //If the Filter Data Sent Is 0, then return only Unresollved Data by setting Status Filter to Null
                    if (statusFilter == 0) statusFilter = null;

                    //Filter Result only if Filter Data is Not Null
                    return View(filter.FilterData != null ? res.Where(x => x.ResolutionStatus == statusFilter) : res);
                }
                else
                {
                    return View(DataContext.GetAllErrorTransactions(new GetTransactionRequest { RequireLimit = true, Limit = filter.Limit ?? null, UserType = filter.UserType, RequireDateFilter = true, UserTypeId = filter.UserTypeId.Value, StartDate = StartDate, EndtDate = EndDate }));
                }
            }
            catch (Exception exp)
            {
                ViewBag.Message = "Error: Unable to display requested data / report query. Please try again or Contact Administrator. " +
                    exp.Message.Substring(0, 15);
                return View(new List<Transaction> { });
            }
        }

        public ActionResult TransactionDetails(int id)
        {
            return View(DataContext.FindErrorTransactionById(id));
        }

    }
}