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

            //get taxpayer info
            if(Convert.ToInt32(roleId) == 7)
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
            

            return View(_dataContext.GetDashBoardSummary(Convert.ToInt32(roleId),Convert.ToInt32(userId)));
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