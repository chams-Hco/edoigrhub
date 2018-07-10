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
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];
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
        public ActionResult PayeForm()
        {
            //get user info
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];
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
        public ActionResult DevelopmentLevy()
        {
            //get user info
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];
            var client = _dataContext.FindClientById(user.ClientId);

            if (user.RoleCode != "WA")
            {
                RedirectToAction("login", "Account");
            }

            decimal PercentageDeduction = (client.ClientSetting.PercentageDeduction.Value / 100);
            ViewBag.PercentageDeduction = PercentageDeduction;





            return View(new WebPayment());

        }


        [CustomAuthorize]
        public ActionResult WithholdingTax()
        {
            //get user info
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];
            var client = _dataContext.FindClientById(user.ClientId);

            if (user.RoleCode != "WA")
            {
                RedirectToAction("login", "Account");
            }

            decimal PercentageDeduction = (client.ClientSetting.PercentageDeduction.Value / 100);
            ViewBag.PercentageDeduction = PercentageDeduction;

            return View(new WebPayment());

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
            if (client == null || client.ClientSetting == null || client.ClientSetting.ConsumptionTaxRevenueId == null)
            {
                ViewBag.Status = false;
                ViewBag.message = "Your account has not been configured completely, please contact admin. ";
                return View();
            }
            var revenueitems = _dataContext.GetAllRevenueItemByClientId(user.ClientId);
            var revenueItem = revenueitems.SingleOrDefault(a => a.Id == client.ClientSetting.ConsumptionTaxRevenueId);

            if (revenueItem == null)
            {
                ViewBag.Status = false;
                ViewBag.message = "Revenue has not been configured completely";
                return View();
            }

            decimal PercentageDeduction = (client.ClientSetting != null ? client.ClientSetting.PercentageDeduction.Value / 100 : 0);
            ViewBag.PercentageDeduction = PercentageDeduction;

            model.GrossIncome = model.FoodAmount + model.DrinkAmount + model.RentalAmount + model.OtherAmount;
            model.Amount = PercentageDeduction * (model.GrossIncome.Value);
            var datesString = model.DatePeriod.Split('-');
            model.FromDate = Convert.ToDateTime(datesString[0].ToString().Trim());
            model.ToDate = Convert.ToDateTime(datesString[1].ToString().Trim());
            model.PercentageDeduction = client.ClientSetting != null ? client.ClientSetting.PercentageDeduction.Value : 0;
            model.UserId = user.UserId;
            model.TerminalId = user.TerminalId.Value;
            model.RevenueItemId = client.ClientSetting != null ? client.ClientSetting.ConsumptionTaxRevenueId.Value : 0; //change to originalNumber
            model.Name = user.Name;

            var result = _dataContext.ProcessWebTrancation(model);
            if (result.ResponseCode == "0000")
            {
                //send cashinvoicetocashwox
                CashWoxModel cashWoxModel = new CashWoxModel
                {
                    accesskey = "qj9Kq6Atv5aA",
                    mdacode = "201",
                    invoices = new List<CashWoxInvoiceModel>()
                     {
                         new CashWoxInvoiceModel
                         {
                             customerfileno = user.TerminalCode,
                             customerid = user.UserId.ToString(),
                             customername = user.Name,
                             invoicedate = DateTime.Now.ToString(),
                             invoicelogid = result.TransactionCode,
                             invoiceno = result.RemittanceCode,
                             isreversal = 0,
                             invoicevaliduntil = DateTime.Now.AddYears(1).ToString(),
                             originalamount = model.Amount.ToString(),
                             items = new List<CashWoxItemModel>() {
                                new CashWoxItemModel
                                {
                                    itemamount = int.Parse(model.Amount.ToString()),
                                    itemcode = revenueItem.Code,
                                    itemname =revenueItem.Name
                                }
                             }

                         }
                     }
                };
                var switchresult = _dataContext.SendInterswitchInvoice(cashWoxModel);

                if (user.TerminalId != null)
                {
                    var Last10Trans = _dataContext.GetLast10TransactionsByTerminalId(user.TerminalId.Value);
                    ViewBag.TerminalTransaction = Last10Trans.ToList();
                }
                ViewBag.Status = true;
                ViewBag.message = "Remittance Code : " + result.RemittanceCode + " # Payment posted successfully. Please proceed to any bank to pay using the remittance code.  <button class=\"btn btn-danger\" id=\"downloadinvoice\" transactioncode=\"" + result.TransactionCode + "\"> Download Invoice </button>";
            }
            else
            {
                ViewBag.Status = false;
                ViewBag.message = result.ResponseDescription;
            }






            return View(new Models.WebPayment());

        }


        [CustomAuthorize]
        [HttpPost]
        public ActionResult DevelopmentLevyInvoice(WebPayment model)
        {
            //get user info
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];


            if (user == null)
            {
                return Json(new { status = false, message = "Please revalidate you user by login off and login on." }, JsonRequestBehavior.AllowGet);
            }

            if (user.TerminalId == null)
            {
                return Json(new { status = false, message = "Your account has not been configured completely, please contact admin." }, JsonRequestBehavior.AllowGet);
            }

            if (user.RoleCode != "WA")
            {
                RedirectToAction("login", "Account");
                return View();
            }



            var client = _dataContext.FindClientById(user.ClientId);
            if (client == null || client.ClientSetting == null || client.ClientSetting.DefaultRevenueItemId == null)
            {
                return Json(new { status = false, message = "Your account has not been configured completely, please contact admin. " }, JsonRequestBehavior.AllowGet);

            }
            //var revenueItem = _dataContext.FindRevenueById(client.ClientSetting.PayeeRevenueItem.Value);
            var revenueitems = _dataContext.GetAllRevenueItemByClientId(user.ClientId);
            var revenueItem = revenueitems.SingleOrDefault(a => a.Id == client.ClientSetting.DefaultRevenueItemId);

            if (revenueItem == null)
            {
                return Json(new { status = false, message = "Revenue has not been configured completely" }, JsonRequestBehavior.AllowGet);
            }




            model.Amount = model.NoOfStaff.Value * model.LiabilityPerStaff.Value;


            model.UserId = user.UserId;
            model.TerminalId = user.TerminalId.Value;
            model.RevenueItemId = client.ClientSetting != null ? client.ClientSetting.DefaultRevenueItemId.Value : 0; //change to originalNumber
            model.Name = user.Name;

            var result = _dataContext.ProcessWebTrancation(model);
            if (result.ResponseCode == "0000")
            {
                //send cashinvoicetocashwox
                CashWoxModel cashWoxModel = new CashWoxModel
                {
                    accesskey = "qj9Kq6Atv5aA",
                    mdacode = "201",
                    invoices = new List<CashWoxInvoiceModel>()
                        {
                         new CashWoxInvoiceModel
                         {
                             customerfileno = user.TerminalCode,
                             customerid = user.UserId.ToString(),
                             customername = user.Name,
                             invoicedate = DateTime.Now.ToString(),
                             invoicelogid = result.RemittanceCode,
                             invoiceno = result.RemittanceCode,
                             isreversal = 0,
                             invoicevaliduntil = DateTime.Now.AddYears(1).ToString(),
                             originalamount =model.Amount.ToString("0.##"),
                             items = new List<CashWoxItemModel>() {
                                new CashWoxItemModel
                                {
                                    itemamount = decimal.Parse(model.Amount.ToString("0.##")),
                                    itemcode = revenueItem.Code,
                                    itemname =revenueItem.Name
                                }
                             }

                         }
                     }
                };
                var switchresult = _dataContext.SendInterswitchInvoice(cashWoxModel);
                if (switchresult != null)
                {
                    return Json(new { status = true, message = "Remittance Code : # <b>" + result.RemittanceCode + "</b>  Payment posted successfully. Please proceed to any bank to pay using the remittance code.  <button class=\"btn btn-danger\" id=\"downloadinvoice\" transactioncode=\"" + result.TransactionCode + "\"> Download Invoice </button>" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { status = false, message = "Invoice Switch Not Available" }, JsonRequestBehavior.AllowGet);
                }



            }
            else
            {

                return Json(new { status = false, message = result.ResponseDescription }, JsonRequestBehavior.AllowGet);
            }
        }





        [CustomAuthorize]
        [HttpPost]
        public ActionResult PayeeInvoice(List<WebPayment> model)
        {
            //get user info
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];


            if (user == null)
            {
                return Json(new { status = false, message = "Please revalidate you user by login off and login on." }, JsonRequestBehavior.AllowGet);
            }

            if (user.TerminalId == null)
            {
                return Json(new { status = false, message = "Your account has not been configured completely, please contact admin." }, JsonRequestBehavior.AllowGet);
            }

            if (user.RoleCode != "WA")
            {
                RedirectToAction("login", "Account");
                return View();
            }



            var client = _dataContext.FindClientById(user.ClientId);
            if (client == null || client.ClientSetting == null || client.ClientSetting.PayeeRevenueItem == null)
            {
                return Json(new { status = false, message = "Your account has not been configured completely, please contact admin. " }, JsonRequestBehavior.AllowGet);

            }
            //var revenueItem = _dataContext.FindRevenueById(client.ClientSetting.PayeeRevenueItem.Value);
            var revenueitems = _dataContext.GetAllRevenueItemByClientId(user.ClientId);
            var revenueItem = revenueitems.SingleOrDefault(a => a.Id == client.ClientSetting.PayeeRevenueItem.Value);

            if (revenueItem == null)
            {
                return Json(new { status = false, message = "Revenue has not been configured completely" }, JsonRequestBehavior.AllowGet);
            }

            decimal PercentageDeduction = (client.ClientSetting != null ? client.ClientSetting.PercentageDeduction.Value / 100 : 0);
            ViewBag.PercentageDeduction = PercentageDeduction;


            foreach (var webpay in model)
            {
                //webpay.GrossIncome = model.FoodAmount + model.DrinkAmount + model.RentalAmount + model.OtherAmount;
                webpay.Amount = computePayeeAmount(webpay);
                webpay.UserId = user.UserId;
                webpay.TerminalId = user.TerminalId.Value;
                webpay.RevenueItemId = revenueItem.Id;
            }

            if (validatePayee(model))
            {
                var result = _dataContext.ProcessMultiWebTrancation(model);
                if (result != null && result.ResponseCode == "0000")
                {
                    //send cashinvoicetocashwox
                    CashWoxModel cashWoxModel = new CashWoxModel
                    {
                        accesskey = "qj9Kq6Atv5aA",
                        mdacode = "201",
                        invoices = new List<CashWoxInvoiceModel>()
                        {
                         new CashWoxInvoiceModel
                         {
                             customerfileno = user.TerminalCode,
                             customerid = user.UserId.ToString(),
                             customername = user.Name,
                             invoicedate = DateTime.Now.ToString(),
                             invoicelogid = result.RemittanceCode,
                             invoiceno = result.RemittanceCode,
                             isreversal = 0,
                             invoicevaliduntil = DateTime.Now.AddYears(1).ToString(),
                             originalamount = model.Sum(a=>a.Amount).ToString("0.##"),
                             items = new List<CashWoxItemModel>() {
                                new CashWoxItemModel
                                {
                                    itemamount = decimal.Parse(model.Sum(a=>a.Amount).ToString("0.##")),
                                    itemcode = revenueItem.Code,
                                    itemname =revenueItem.Name
                                }
                             }

                         }
                     }
                    };
                    var switchresult = _dataContext.SendInterswitchInvoice(cashWoxModel);

                    if (switchresult != null)
                    {
                        return Json(new { status = true, message = "Remittance Code : <b> #" + result.RemittanceCode + " </b> Payment posted successfully. Please proceed to any bank to pay using the remittance code.  <button class=\"btn btn-danger\" id=\"downloadinvoice\" transactioncode=\"" + result.TransactionCode + "\"> Download Invoice </button>" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "Invoice Switch Not Available" }, JsonRequestBehavior.AllowGet);
                    }



                }
                else
                {

                    return Json(new { status = false, message = result.ResponseDescription }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = "So sorry, but you didnt pass some verification, please contact administrators" }, JsonRequestBehavior.AllowGet);
            }

        }

        
        [CustomAuthorize]
        [HttpPost]
        public ActionResult WithholdingTaxInvoice(List<WebPayment> model)
        {
            //get user info
            var user = (ViewModels.UserDashBoardViewModel)Session["LoggedInUser"];


            if (user == null)
            {
                return Json(new { status = false, message = "Please revalidate you user by login off and login on." }, JsonRequestBehavior.AllowGet);
            }

            if (user.TerminalId == null)
            {
                return Json(new { status = false, message = "Your account has not been configured completely, please contact admin." }, JsonRequestBehavior.AllowGet);
            }

            if (user.RoleCode != "WA")
            {
                RedirectToAction("login", "Account");
                return View();
            }



            var client = _dataContext.FindClientById(user.ClientId);
            if (client == null || client.ClientSetting == null || client.ClientSetting.WithholdingTaxRevenueItem == null)
            {
                return Json(new { status = false, message = "Your account has not been configured completely, please contact admin. " }, JsonRequestBehavior.AllowGet);

            }
            //var revenueItem = _dataContext.FindRevenueById(client.ClientSetting.PayeeRevenueItem.Value);
            var revenueitems = _dataContext.GetAllRevenueItemByClientId(user.ClientId);
            var revenueItem = revenueitems.SingleOrDefault(a => a.Id == client.ClientSetting.WithholdingTaxRevenueItem.Value);

            if (revenueItem == null)
            {
                return Json(new { status = false, message = "Revenue has not been configured completely" }, JsonRequestBehavior.AllowGet);
            }

            decimal PercentageDeduction = (client.ClientSetting != null ? client.ClientSetting.PercentageDeduction.Value / 100 : 0);
            ViewBag.PercentageDeduction = PercentageDeduction;


            foreach (var webpay in model)
            {
                //webpay.GrossIncome = model.FoodAmount + model.DrinkAmount + model.RentalAmount + model.OtherAmount;
                webpay.Amount = webpay.WithholdingTaxLiability.Value;
                webpay.UserId = user.UserId;
                webpay.TerminalId = user.TerminalId.Value;
                webpay.RevenueItemId = revenueItem.Id;
            }

            if (model.Sum(a=>a.Amount) > 0)
            {
                var result = _dataContext.ProcessMultiWebTrancation(model);
                if (result != null && result.ResponseCode == "0000")
                {
                    //send cashinvoicetocashwox
                    CashWoxModel cashWoxModel = new CashWoxModel
                    {
                        accesskey = "qj9Kq6Atv5aA",
                        mdacode = "201",
                        invoices = new List<CashWoxInvoiceModel>()
                        {
                         new CashWoxInvoiceModel
                         {
                             customerfileno = user.TerminalCode,
                             customerid = user.UserId.ToString(),
                             customername = user.Name,
                             invoicedate = DateTime.Now.ToString(),
                             invoicelogid = result.RemittanceCode,
                             invoiceno = result.RemittanceCode,
                             isreversal = 0,
                             invoicevaliduntil = DateTime.Now.AddYears(1).ToString(),
                             originalamount = model.Sum(a=>a.Amount).ToString("0.##"),
                             items = new List<CashWoxItemModel>() {
                                new CashWoxItemModel
                                {
                                    itemamount = decimal.Parse(model.Sum(a=>a.Amount).ToString("0.##")),
                                    itemcode = revenueItem.Code,
                                    itemname =revenueItem.Name
                                }
                             }

                         }
                     }
                    };
                    var switchresult = _dataContext.SendInterswitchInvoice(cashWoxModel);

                    if (switchresult != null)
                    {
                        return Json(new { status = true, message = "Remittance Code : <b> #" + result.RemittanceCode + " </b> Payment posted successfully. Please proceed to any bank to pay using the remittance code.  <button class=\"btn btn-danger\" id=\"downloadinvoice\" transactioncode=\"" + result.TransactionCode + "\"> Download Invoice </button>" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { status = false, message = "Invoice Switch Not Available" }, JsonRequestBehavior.AllowGet);
                    }



                }
                else
                {

                    return Json(new { status = false, message = result.ResponseDescription }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { status = false, message = "So sorry, but you didnt pass some verification, please contact administrators" }, JsonRequestBehavior.AllowGet);
            }

        }

        private bool validatePayee(List<WebPayment> model)
        {

            foreach (var webpay in model)
            {
                var difference = Math.Round(webpay.Amount) - Math.Round(webpay.MonthlyTaxLiability.Value);
                if (webpay.Year == null || webpay.Month == null || webpay.MonthlyTaxLiability == null)
                {
                    return false;
                }
                else if (difference > 1 || difference < -1)
                {
                    return false;
                }
            }
            return true;
        }

        private decimal computePayeeAmount(WebPayment webpay)
        {
            webpay.AnnualIncome = webpay.MonthlyIncome * 12;
            webpay.AnnualPension = webpay.MonthlyPension * 12;
            webpay.AnnualNHFund = webpay.AnnualNHFund * 12;
            webpay.AnnualNHFund = webpay.MonthlyNHIS * 12;


            if (((decimal)webpay.AnnualIncome.Value * (1 / 100)) > 200000)
                webpay.ConsolidatedReliefs = (21 / 100) * ((decimal)webpay.AnnualIncome.Value);
            else
                webpay.ConsolidatedReliefs = ((webpay.AnnualIncome.Value * (20 / 100)) + 200000);

            webpay.TaxableIncome = webpay.AnnualIncome - webpay.AnnualPension - webpay.AnnualNHFund - webpay.AnnualNHIS - webpay.ConsolidatedReliefs;

            if (webpay.TaxableIncome < 300000)
            {
                webpay.ComputedAnnualTax = webpay.TaxableIncome * (7 / 100);
            }
            else if (webpay.TaxableIncome > 300000 && webpay.TaxableIncome <= 600000)
            {
                webpay.ComputedAnnualTax = 21000 + ((webpay.TaxableIncome - 300000) * (11 / 100));
            }
            else if (webpay.TaxableIncome > 600000 && webpay.TaxableIncome <= 1100000)
            {
                webpay.ComputedAnnualTax = 54000 + ((webpay.TaxableIncome - 600000) * (15 / 100));
            }
            else if (webpay.TaxableIncome > 1100000 && webpay.TaxableIncome <= 1600000)
            {
                webpay.ComputedAnnualTax = 129000 + ((webpay.TaxableIncome - 1100000) * (19 / 100));
            }
            else if (webpay.TaxableIncome > 1600000 && webpay.TaxableIncome <= 3200000)
            {
                webpay.ComputedAnnualTax = 224000 + ((webpay.TaxableIncome - 1600000) * (21 / 100));
            }
            else
            {
                webpay.ComputedAnnualTax = 560000 + ((webpay.TaxableIncome - 3200000) * (24 / 100));
            }

            webpay.MinimumTax = (1 / 100) * webpay.AnnualIncome;

            if (webpay.ComputedAnnualTax < webpay.MinimumTax)
            {
                webpay.AnnualTaxPayable = webpay.MinimumTax;
            }
            else
            {
                webpay.AnnualTaxPayable = webpay.ComputedAnnualTax;
            }

            webpay.MonthlyTaxLiability = (webpay.AnnualTaxPayable / 12);

            return webpay.MonthlyTaxLiability.Value;
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


            var transaction = _dataContext.FindTransactionByCode(id);
            if (transaction == null || transaction.TransactionId <= 0)
            {
                return Json(new { Status = false, Message = "No Record Found" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { Staus = true, TransactionDate = transaction.TransactionDate.ToString("dd/MM/yy hh:mm:ss tt"), Period = ("" + transaction.FromDate.Value.ToString("dd/MM/yy") + "-" + transaction.ToDate.Value.ToString("dd/MM/yy")), Transaction = transaction }, JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult TaxInvoicing()
        {
            return View();
        }

        [CustomAuthorize]
        [HttpGet]
        public ActionResult NonResidentTaxInvoicing()
        {
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


            var revenueitem = _dataContext.GetAllRevenueItemByClientId(user.ClientId);
            ViewBag.Status = true;
            ViewBag.RevenueItems = revenueitem;
            return View();
        }


        [CustomAuthorize]
        [HttpGet]
        public ActionResult GenerateTaxInvoice(int id)
        {
            try
            {
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

                var password = (string)Session["password"];
                var result = _dataContext.FindResident(new VerifyResidentRequest { AgentCode = user.ZoneCode, Password = password, ResidentId = "" + id, TerminalCode = user.TerminalCode, UserName = user.Email });

                var revenueitem = _dataContext.GetAllRevenueItemByClientId(user.ClientId);
                ViewBag.RevenueItems = revenueitem;

                if (result != null && result.ResponseCode == "OK")
                {
                    ViewBag.Status = true;
                    return View(result.Resident.First());
                }
                else
                {
                    RedirectToAction("Payments", "TaxInvoicing");
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Status = false;
                return View();

            }

        }


        [CustomAuthorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ResidentSearch(SearchRequest search)
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

            var password = (string)Session["password"];
            var result = _dataContext.FindResident(new VerifyResidentRequest { AgentCode = user.ZoneCode, Password = password, ResidentId = search.Id, TerminalCode = user.TerminalCode, UserName = user.Email });

            if (result != null && result.ResponseCode == "OK")
            {
                return Json(new { status = true, result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = false, message = "Cannot Find Resident With Specified Parameters" }, JsonRequestBehavior.AllowGet);
            }








        }

        [CustomAuthorize]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult InvoiceGeneration(WebPayment model)
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


            model.TerminalId = user.TerminalId.Value;

            var result = _dataContext.ProcessWebTrancation(model);
            if (result.ResponseCode == "0000")
            {
                return Json(new { status = true, result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { status = false, result }, JsonRequestBehavior.AllowGet);
            }







        }

        public class SearchRequest
        {
            public string Id { get; set; }
        }


    }
}