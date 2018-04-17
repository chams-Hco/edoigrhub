using CICSWebPortal.Infrastructure;
using CICSWebPortal.Models;
using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class PaymentsController : Controller
    {
        private IDataService _dataContext;

        public PaymentsController()
            : this(MainContainer.DataService())
        {

        }

        public PaymentsController(IDataService dataContext)
        {
            this._dataContext = dataContext;
        }

        // GET: /Payments/
        public ActionResult Index()
        {
            //get user info
            var roleId = Session["RoleId"];
            var userEmail = Session["User"].ToString();

            List<Payment> model = new List<Payment>();

            model = _dataContext.GetAllTransactionDetailsByUser(0, userEmail);

            //get payment title
            for (int i = 0; i < model.Count; i++)
            {
                //get transaction details by invoice for user
                if (model[i].TransactionId.HasValue)
                {
                    Invoice inv = _dataContext.GetInvoiceById(model[i].TransactionId.Value);

                    if (inv != null)
                    {
                        model[i].TransactionInfo = inv.Description;

                    }
                }                
            }

            //get displayMessage
            if (Session["displayMessage"] != null)
                ViewBag.displayMessage = (HtmlString)Session["displayMessage"];
            Session.Remove("displayMessage");

            return View(model);

        }


        [CustomAuthorize]
        public ActionResult RemittanceForm()
        {
            //get user info
            var user =  (ViewModels.UserDashBoardViewModel) Session["LoggedInUser"];
            var client = _dataContext.FindClientById(user.ClientId);

            if (user.RoleCode != "WA")
            {
                RedirectToAction("login", "Account");
            }

            decimal PercentageDeduction = (client.ClientSetting.PercentageDeduction.Value / 100);
            ViewBag.PercentageDeduction = PercentageDeduction;


            if (user.TerminalId != null)
            {
                var Last10Trans = _dataContext.GetLast10TransactionsByTerminalId(user.TerminalId.Value);
                ViewBag.TerminalTransaction = Last10Trans.ToList();
            }



            return View();

        }

        [CustomAuthorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult RemittanceForm(WebPayment model)
        {
            //get user info
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];
            

            if (user == null)
            {
                ViewBag.Status = false;
                ViewBag.message = "Please revalidate you user by login off and login on.";
                return View();
            }

            if (user.TerminalId == null)
            {
                ViewBag.Status = false;
                ViewBag.message = "Your account has not been configured completely, please contact admin.";
                return View();
            }

            if (user.RoleCode != "WA")
            {
                RedirectToAction("login", "Account");
                return View();
            }

            

            var client = _dataContext.FindClientById(user.ClientId);
            decimal PercentageDeduction = (client.ClientSetting!= null ? client.ClientSetting.PercentageDeduction.Value / 100 : 0);
            ViewBag.PercentageDeduction = PercentageDeduction;

            model.GrossIncome = model.FoodAmount + model.DrinkAmount + model.RentalAmount + model.OtherAmount;
            model.Amount = PercentageDeduction * (model.GrossIncome);
            var datesString = model.DatePeriod.Split('-');
            model.FromDate = Convert.ToDateTime(datesString[0].ToString().Trim());
            model.ToDate = Convert.ToDateTime(datesString[1].ToString().Trim());
            model.PercentageDeduction = client.ClientSetting != null ? client.ClientSetting.PercentageDeduction.Value : 0;
            model.UserId = user.UserId;
            model.TerminalId = user.TerminalId.Value;
            model.RevenueItemId = client.ClientSetting != null ? client.ClientSetting.DefaultRevenueItemId.Value : 0; //change to originalNumber
            model.Name = user.Name;

            var result = _dataContext.ProcessWebTrancation(model);
            if(result.ResponseCode =="0000")
            {
                if (user.TerminalId != null)
                {
                    var Last10Trans = _dataContext.GetLast10TransactionsByTerminalId(user.TerminalId.Value);
                    ViewBag.TerminalTransaction = Last10Trans.ToList();
                }
                ViewBag.Status = true;
                ViewBag.message = "Remittance Code : "+ result.RemittanceCode + " # Payment posted successfully. Please proceed to any bank to pay using the remittance code.  <button class=\"btn btn-danger\" id=\"downloadinvoice\" transactioncode=\""+result.TransactionCode+"\"> Download Invoice </button>";
            }
            else
            {
                ViewBag.Status = false;
                ViewBag.message = result.ResponseDescription;
            }


            



            return View(new Models.WebPayment());

        }

        [CustomAuthorize]
        [HttpGet]
       // [Route("[action]/{code}")]
        public ActionResult FindTransaction(string id)
        {
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];


            if (user == null)
            {
                return Json(new { Status = false, Message = "Invalid User" }, JsonRequestBehavior.AllowGet);
            }


            var transaction =_dataContext.FindTransactionByCode(id);
            if(transaction == null || transaction.TransactionId <= 0)
            {
                return  Json(new { Status = false, Message = "No Record Found" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Staus = true, TransactionDate = transaction.TransactionDate.ToString("dd/MM/yy hh:mm:ss tt"), Period = (""+transaction.FromDate.Value.ToString("dd/MM/yy") +"-"+ transaction.ToDate.Value.ToString("dd/MM/yy")),   Transaction = transaction }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}