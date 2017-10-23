using CICSWebPortal.Models;
using CICSWebPortal.Services;
using CICSWebPortal.ViewModels;
using CICSWebPortal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class TaxpayerController : Controller
    {
        private IDataService _dataContext;
        private EmailSv.WebService sendMail = new EmailSv.WebService();

        public TaxpayerController()
            : this(MainContainer.DataService())
        {

        }

        public TaxpayerController(IDataService dataContext)
        {
            this._dataContext = dataContext;
        }


        // GET: TaxPayer
        public ActionResult Index()
        {
            //get user info
            var roleId = Session["RoleId"];
            var userId = Session["UserId"] != null ? int.Parse(Session["UserId"].ToString()) : 0;
            var userEmail = Session["User"].ToString();

            //get all tax payers
            var model = _dataContext.GetAllTaxPayers();

            //display message from redirect
            if (Session["displayMessage"] != null)
            {
                ViewBag.displayMessage = new HtmlString(Session["displayMessage"].ToString());
                Session.Remove("displayMessage");
            }

            return View(model);
        }

        // GET: Taxpayer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Level/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaxpayerViewModel taxpayer)
        {
            //get user info
            var roleId = Session["RoleId"];
            var userEmail = Session["User"].ToString();

            if (ModelState.IsValid)
            {
                var taxpayerModel = new Taxpayer()
                {
                    Firstname = taxpayer.Firstname,
                    Surname = taxpayer.Surname,
                    Email = taxpayer.Email,
                    Password = taxpayer.Password,
                    Mobile = taxpayer.Mobile,
                    RIN = taxpayer.RIN,
                    RoleId = 7,
                };

                bool res = _dataContext.AddTaxpayer(taxpayerModel);

                if (res)
                    Session["displayMessage"] = "<span style='color:green'>Taxpayer Added successfully</span>";
                else
                    Session["displayMessage"] = "<span style='color:red'>An error occured! Not successful</span>";

                return RedirectToAction("Index");
            }

            return View(taxpayer);
        }

        // GET: /Taxpayer/Invoices/2
        public ActionResult Invoices(int id)
        {
            //get user info
            var roleId = Session["RoleId"];
            var userEmail = Session["User"].ToString();

            //get taxpayer info
            Taxpayer taxpayer = new Taxpayer();
            taxpayer = _dataContext.GetTaxpayerById(id);

            //get invoices by email
            List<Invoice> model = new List<Invoice>();
            if (taxpayer != null)
            {
                ViewBag.TaxpayerName = taxpayer.Firstname + " " + taxpayer.Surname;
                ViewBag.Email = taxpayer.Email;
                ViewBag.RIN = taxpayer.RIN;
                model = _dataContext.GetUserInvoices(taxpayer.Email);

                //get unpaid invoices
                for (int i = 0; i < model.Count; i++)
                {
                    //get transaction details by invoice for user
                    var transactionDetails = _dataContext.GetTransactionDetails(model[i].InvoiceId, model[i].Email);

                    model[i].AmountPaid = transactionDetails != null ? transactionDetails.Select(x => x.Amount).Sum() : 0;
                }
            }
            return View(model);
        }

        // GET: /Taxpayer/AddAssessment/
        [HttpGet]
        public ActionResult AddAssessment(int id)
        {
            Session.Remove("activeId");
            if (Session["RoleId"] == null)
            {
                Response.Redirect("/Account/Login", true);
            }

            //get user info
            var roleId = Convert.ToInt32(Session["RoleId"]);
            var userEmail = Session["User"].ToString();

            //get taxpayer info
            Taxpayer taxpayer = new Taxpayer();
            taxpayer = _dataContext.GetTaxpayerById(id);

            //get invoices by email
            List<RevenueItem> model = new List<RevenueItem>();
            if (taxpayer != null)
            {
                ViewBag.TaxpayerName = taxpayer.Firstname + " " + taxpayer.Surname;
                ViewBag.Email = taxpayer.Email;
                ViewBag.RIN = taxpayer.RIN;
                Session["activeId"] = taxpayer.UserId;
                //get assessment
                model = _dataContext.GetAssessmentByRole(new AssessmentReq() { roleid = roleId, email = taxpayer.Email });
            }

            if(Session["displayMessage"] != null)
            {
                ViewBag.displayMessage = new HtmlString(Session["displayMessage"].ToString());
                Session.Remove("displayMessage");
            }

            return View(model);

        }

        // GET: /Taxpayer/GenerateInvoice/4
        [HttpGet]
        public ActionResult GenerateInvoice(int id)
        {

            if (Session["RoleId"] == null)
            {
                Response.Redirect("/Account/Login", true);
            }

            if (Session["activeId"] == null)
            {
                return RedirectToAction("Index");
            }

            //get user info
            var roleId = Convert.ToInt32(Session["RoleId"]);
            var userEmail = Session["User"].ToString();

            var activeTaxpayer = Session["activeId"].ToString();

            try
            {
                bool res = _dataContext.GenerateInvoice(new GenerateInvoice
                {
                    TaxpayerId = int.Parse(activeTaxpayer),
                    RevenueCode = id.ToString(),
                });
                if (res)
                {
                    Session["displayMessage"] = "<span style='color:green'>Invoice Generated Successfully.</span>";

                    //send mail
                    var biodata = _dataContext.GetTaxpayerById(int.Parse(activeTaxpayer));

                    string msg = "You have a new Invoice. Logon to Portal to make payment" ;

                    string response = sendMail.sendmail1(biodata.Email, "EDO-IGR Invoice", msg);
                    if (response != "")
                    {
                        Session["displayMessage"] = "<span style='color:green'>Invoice Generated Successfully. A Notification has been sent to your email.</span>";
                    }

                }
                else
                    Session["displayMessage"] = "<span style='color:red'>An error occured! Not successful</span>";

            }
            catch(Exception ex)
            {

            }
            return RedirectToAction("AddAssessment", new { id = activeTaxpayer });

        }
    }
}