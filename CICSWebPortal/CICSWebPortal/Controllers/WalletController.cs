using CICSWebPortal.Services;
using CICSWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class WalletController : Controller
    {
        private IDataService _dataContext;
        private EmailSv.WebService sendMail = new EmailSv.WebService();

        public WalletController() : this(MainContainer.DataService())
        {

        }

        public WalletController(IDataService dataContext)
        {
            this._dataContext = dataContext;
        }

        // GET: Wallet
        public ActionResult Index()
        {
            var roleId = Session["RoleId"];
            var userId = Session["UserId"] != null ? int.Parse(Session["UserId"].ToString()) : 0;
            var userEmail = Session["User"].ToString();

            var model = _dataContext.GetWallet(userEmail);
            WalletViewModel walletmodel = new WalletViewModel();
           
            if (model == null)
            {
                model = new Models.Wallet() { Amount = 0, Email = userEmail };
            }

            walletmodel.Wallet = model;
            Session["walletid"] = model.WalletId;

            //get displayMessage
            if (Session["displayMessage"] != null)
                ViewBag.displayMessage = new HtmlString(Session["displayMessage"].ToString());
            Session.Remove("displayMessage");

            return View(walletmodel);
        }

        //Post: Wallet
        [HttpPost]
        public ActionResult Index(WalletViewModel walletViewModel)
        {
            #region validate dummy card
            string cardNumber = Request["cardnumber"].ToString();
            string civv = Request["civv"].ToString();
            string expdate = Request["expdate"].ToString();

            if (cardNumber.Trim() != "11112222333344445555" || civv.Trim() != "000" || expdate.Trim() != "12/20")
            {
                Session["displayMessage"] = "<span style='color: red'>Transaction Error! Invalid card details.</span>";

                //return Index
                return RedirectToAction("Index");
            }
            #endregion



            walletViewModel.Payment.UserId = Session["UserId"] != null ? int.Parse(Session["UserId"].ToString()) : 0;
            walletViewModel.Payment.Email = Session["User"].ToString();
            walletViewModel.Payment.Method = "CreditCard";
            walletViewModel.Payment.Reference = Guid.NewGuid().ToString();
            walletViewModel.Payment.Target = "Wallet";
            walletViewModel.Payment.WalletId = Convert.ToInt32(Session["walletid"]);

            var isFunded = _dataContext.FundWallet(walletViewModel.Payment);

            //TODO: Handle condition if isFunded is false


            //return response message
            if (isFunded)
            {
                Session["displayMessage"] =  "<span style='color: green'>Wallet Funded successfully</span>";
                //send mail
                string msg = "You wallet was funded successfully. Logon to Portal to view balance";

                string response = sendMail.sendmail1(walletViewModel.Payment.Email, "EDO-IGR Invoice", msg);
                if (response != "")
                {
                    Session["displayMessage"] = "<span style='color:green'>Wallet Funded successfully. A Notification has been sent to your email.</span>";
                }
            }               
            else
                Session["displayMessage"] = "<span style='color: red'>An Error Occured!</span>";

            return RedirectToAction("Index");
        }
    }
}