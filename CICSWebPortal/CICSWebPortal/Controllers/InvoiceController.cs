using CICSWebPortal.Models;
using CICSWebPortal.Services;
using CICSWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class InvoiceController : Controller
    {
        private IDataService _dataContext;
        private EmailSv.WebService sendMail = new EmailSv.WebService();

        public InvoiceController()
            : this(MainContainer.DataService())
        {

        }

        public InvoiceController(IDataService dataContext)
        {
            this._dataContext = dataContext;
        }

        // GET: /Invoice/
        public ActionResult Index()
        {
            //get user info
            var roleId = Session["RoleId"];
            var userEmail = Session["User"].ToString();

            List<Invoice> model = new List<Invoice>();

            if (Convert.ToInt32(roleId) == 3)
            {
                model = _dataContext.GetAllInvoices();
                //get unpaid invoices
                for (int i = 0; i < model.Count; i++)
                {
                    //get transaction details by invoice for user
                    var transactionDetails = _dataContext.GetTransactionDetails(model[i].InvoiceId, model[i].Email);

                    model[i].AmountPaid = transactionDetails != null ? transactionDetails.Select(x => x.Amount).Sum() : 0;
                }

                //get displayMessage
                if (Session["displayMessage"] != null)
                    ViewBag.displayMessage = new HtmlString(Session["displayMessage"].ToString());
                Session.Remove("displayMessage");

                return View("All", model);
            }
            else
            {
                List<Invoice> viewmodel = new List<Invoice>();
                viewmodel = _dataContext.GetUserInvoices(userEmail);
                

                //get unpaid invoices
                for (int i = 0; i < viewmodel.Count; i++)
                {
                    //get transaction details by invoice for user
                    var transactionDetails = _dataContext.GetTransactionDetails(viewmodel[i].InvoiceId, userEmail);

                    var amountpaid = transactionDetails != null ? transactionDetails.Select(x => x.Amount).Sum() : 0;
                    if (viewmodel[i].Amount - amountpaid != 0 && Convert.ToInt32(viewmodel[i].Amount) != 0) model.Add(viewmodel[i]);
                    viewmodel[i].AmountPaid = transactionDetails != null ? transactionDetails.Select(x => x.Amount).Sum() : 0;
                }

                //get displayMessage
                if (Session["displayMessage"] != null)
                    ViewBag.displayMessage = new HtmlString(Session["displayMessage"].ToString());
                Session.Remove("displayMessage");

                return View(model);
            }
        }
        
        // GET: /Invoice/PayWithWallet/2
        [HttpGet]
        public ActionResult PayWithWallet(int id)
        {
            var userEmail = Session["User"].ToString();

            //get invoice by id
            var invoice = _dataContext.GetInvoiceById(id);
            Session["activeinvoice"] = invoice != null ? invoice : null;

            //get transaction details by invoice for user
            var transactionDetails = _dataContext.GetTransactionDetails(id, userEmail);

            var amountpaid = transactionDetails != null ? transactionDetails.Select(x => x.Amount).Sum() : 0;
            var amountLeft = amountpaid == 0 ? invoice.Amount : invoice.Amount - amountpaid;

            //display transaction details
            ViewBag.AmountPaid = " ₦ " + amountpaid;
            ViewBag.AmountLeft = " ₦ " + amountLeft;

            Session["amountLeft"] = amountLeft;

            //get wallet balance
            var model = _dataContext.GetWallet(userEmail);
            ViewBag.WalletBalance = model.Amount;

            //get displayMessage
            if (Session["displayMessage"] != null)
                ViewBag.displayMessage = new HtmlString(Session["displayMessage"].ToString()); ;
            Session.Remove("displayMessage");

            return View(invoice);
        }

        // POST: /Invoice/PayWithWallet/2
        [HttpPost]
        public ActionResult PayWithWallet()
        {
            //get user info
            var userEmail = Session["User"].ToString();
            var userId = Session["UserId"] != null ? int.Parse(Session["UserId"].ToString()) : 0;
            
            //get binvoice info
            Invoice invoice;
            if (Session["activeinvoice"] != null) invoice = (Invoice)Session["activeinvoice"];
            else return RedirectToAction("~/Home");

            //check amount left
            decimal amountLeft = 0;
            if (Session["amountLeft"] != null)
                amountLeft = Convert.ToDecimal(Session["amountLeft"]);
            if(Convert.ToInt32(amountLeft) == 0)
            {
                Session["displayMessage"] = "<span style='color: green'>You have already paid for this Assessment</span>";
                return RedirectToAction("PayWithWallet", invoice);
            }

            //get wallet balance
            var model = _dataContext.GetWallet(userEmail);
            ViewBag.WalletBalance = model.Amount;

            //get amount to pay
            string payment_amount = Request["paymentamount"].ToString();
            decimal pay = 0;

            //check if wallet > paymentamount : return error if false
            if (decimal.TryParse(payment_amount, out pay))
            {
                if (pay <= 0)
                {
                    //return insufficent balance message
                    Session["displayMessage"] = "<span style='color: red'>Enter a valid amount</span>";
                }
                else if (pay > model.Amount)
                {
                    //return insufficent balance message
                    Session["displayMessage"] = "<span style='color: red'>Insufficient Wallet balance</span>";
                }
                else if (amountLeft == invoice.Amount && pay > amountLeft)
                {
                    //return insufficent balance message
                    Session["displayMessage"] = "<span style='color: red'>Amount should not be more than Item Amount</span>";
                }
                else if (pay > amountLeft)
                {
                    //return insufficent balance message
                    Session["displayMessage"] = "<span style='color: red'>Amount should not be more than Amount Left</span>";
                }
                else
                {
                    //implement transaction
                    WalletViewModel req = new WalletViewModel();
                    req.Payment = new Payment();
                    req.Payment.UserId = userId;
                    req.Payment.Email = Session["User"].ToString();
                    req.Payment.WalletId = model.WalletId;
                    req.Payment.Method = "Wallet";
                    req.Payment.Reference = Guid.NewGuid().ToString();
                    req.Payment.Target = "Assessment";
                    req.Payment.Amount = -pay;
                    req.Payment.TransactionId = invoice.InvoiceId;                    
                    
                    req.Wallet = model;            

                    var isFunded = _dataContext.FundWallet(req.Payment);

                    //return successfull message and redirect to invoice
                    if (isFunded)
                    {
                        Session["displayMessage"] = new HtmlString("<span style='color: green'>Payment done successfully</span>");

                        //send mail
                        string msg = "You payment was successful. Logon to Portal to view payment";

                        string response = sendMail.sendmail1(req.Payment.Email, "EDO-IGR Invoice", msg);
                        if (response != "")
                        {
                            Session["displayMessage"] = new HtmlString("<span style='color:green'>Invoice Generated Successfully. A Notification has been sent to your email.</span>");
                        }
                    }                        
                    else
                        Session["displayMessage"] = new HtmlString("<span style='color: red'>An Error Occured!</span>");
                }
            } else
            {
                //return insufficent balance message
                Session["displayMessage"] = new HtmlString("<span style='color: red'>Enter a valid amount</span>");
            }


            //return view

            return RedirectToAction("PayWithWallet", new { id = invoice.InvoiceId });
        }


        // GET: /Invoice/PayWithCard/2
        [HttpGet]
        public ActionResult PayWithCard(int id)
        {
            var userEmail = Session["User"].ToString();

            //get invoice by id
            var invoice = _dataContext.GetInvoiceById(id);
            Session["activeinvoice"] = invoice != null ? invoice : null;

            //get transaction details by invoice for user
            var transactionDetails = _dataContext.GetTransactionDetails(id, userEmail);

            var amountpaid = transactionDetails != null ? transactionDetails.Select(x => x.Amount).Sum() : 0;
            var amountLeft = amountpaid == 0 ? invoice.Amount : invoice.Amount - amountpaid;

            //display transaction details
            ViewBag.AmountPaid = " ₦ " + amountpaid;
            ViewBag.AmountLeft = " ₦ " + amountLeft;

            Session["amountLeft"] = amountLeft;

            //get displayMessage
            if (Session["displayMessage"] != null)
                ViewBag.displayMessage = new HtmlString(Session["displayMessage"].ToString());
            Session.Remove("displayMessage");

            return View(invoice);
        }

        // POST: /Invoice/PayWithCard/2
        [HttpPost]
        public ActionResult PayWithCard()
        {
            //get user info
            var roleId = Session["RoleId"];
            var userId = Session["UserId"] != null ? int.Parse(Session["UserId"].ToString()) : 0;
            var userEmail = Session["User"].ToString();
            var amountLeft = decimal.Parse(Session["amountLeft"].ToString());

            //get binvoice info
            Invoice invoice;
            if (Session["activeinvoice"] != null) invoice = (Invoice)Session["activeinvoice"];
            else return RedirectToAction("~/Home");

            #region validate dummy card
            string cardNumber = Request["cardnumber"].ToString();
            string civv = Request["civv"].ToString();
            string expdate = Request["expdate"].ToString();

            if(cardNumber.Trim() != "11112222333344445555" && civv.Trim() != "000" && expdate.Trim() != "12/20")
            {
                ViewBag.displayMessage = new HtmlString("<span style='color: red'>Transaction Error! Invalid card details.</span>");

                //return view
                return View(invoice);
            }
            #endregion

            //get/check amount to pay
            string payment_amount = Request["paymentamount"].ToString();
            decimal pay = 0;
            
            if (decimal.TryParse(payment_amount, out pay))
            {
                if (pay <= 0)
                {
                    //return insufficent balance message
                    Session["displayMessage"] = "<span style='color: red'>Enter a valid amount</span>";
                }
                else if (amountLeft == invoice.Amount && pay > amountLeft)
                {
                    //return insufficent balance message
                    Session["displayMessage"] = "<span style='color: red'>Amount should not be more than Item Amount</span>";
                }
                else if (pay > amountLeft)
                {
                    //return success message
                    Session["displayMessage"] = "<span style='color: red'>Enter a valid amount not more than the Amount left </span>";
                    return RedirectToAction("PayWithCard", invoice.InvoiceId);
                }
                else
                {
                    //initiate payment 
                    var pmtModel = new PaymentReq()
                    {
                        Amount = Convert.ToDecimal(payment_amount),
                        Email = userEmail,
                        Method = "CreditCard",
                        Reference = Guid.NewGuid().ToString(),
                        TransactionId = invoice.InvoiceId,
                        UserId = Session["UserId"] != null ? int.Parse(Session["UserId"].ToString()) : 0,
                        Target = "Assessment",
                        InvoiceId = invoice.InvoiceId,
                    };

                    var res = _dataContext.MakePayment(pmtModel);

                    if (res)
                    {
                        //return success message
                        Session["displayMessage"] = "<span style='color: green'>Payment done successfully</span>";
                        //send mail
                        string msg = "You payment was successful. Logon to Portal to view payment";

                        string response = sendMail.sendmail1(pmtModel.Email, "EDO-IGR Invoice", msg);
                        if (response != "")
                        {
                            Session["displayMessage"] = "<span style='color:green'>Invoice Generated Successfully. A Notification has been sent to your email.</span>";
                        }
                    }
                    else
                    {
                        //return success message
                        Session["displayMessage"] = "<span style='color: red'>Error!</span>";
                    }
                }                
            }
            else
            {
                //return insufficent balance message
                Session["displayMessage"] = "<span style='color: red'>Enter a valid amount</span>";
            }

            //return view
            return RedirectToAction("PayWithCard");
        }
    }
}