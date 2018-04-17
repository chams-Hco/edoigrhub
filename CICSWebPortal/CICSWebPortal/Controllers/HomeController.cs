using CICSWebPortal.Infrastructure;
using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private IDataService _dataContext;

        public HomeController() : this(MainContainer.DataService())
        {

        }

        public HomeController(IDataService dataContext)
        {
            this._dataContext = dataContext;
        }


        //Get Dashboard
        public ActionResult Index()
        {
            var roleId = Session["RoleId"];
            var userId = Session["UserId"];
            var userEmail = Session["User"];
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];

            //get taxpayer info
            if (user.RoleCode == "TP")
            {
                var taxpayerinfo = _dataContext.GetTaxpayerById(Convert.ToInt32(userId));
                ViewBag.TaxpayerName = taxpayerinfo.Firstname + " " + taxpayerinfo.Surname;
                Session["taxpayerInfo"] = taxpayerinfo;
                var taxpayerDashboard = _dataContext.GetTaxpayerDashboardSummary(Convert.ToInt32(roleId), Convert.ToInt32(userId), userEmail.ToString());
                //ViewBag.InvoiceCount = taxpayerDashboard.Invoices;
                //ViewBag.WalletBal = taxpayerDashboard.Wallet;
                //ViewBag.NotificationCount = taxpayerDashboard.Notification;

                return View(taxpayerDashboard);
            }
            else if (user.RoleCode == "WA")
            {
                if (user != null && user.TerminalId != null)
                {
                    var top10Transaction = _dataContext.GetLast10TransactionsByTerminalId(user.TerminalId.Value);
                    var lastpayment = (top10Transaction.FirstOrDefault() != null) ? top10Transaction.First().Amount : 0;
                    ViewBag.LastPayment = lastpayment;
                }
            }

            var dashboard = _dataContext.GetDashBoardSummary(Convert.ToInt32(roleId), Convert.ToInt32(userId), user.RoleCode);


            return View(dashboard);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}