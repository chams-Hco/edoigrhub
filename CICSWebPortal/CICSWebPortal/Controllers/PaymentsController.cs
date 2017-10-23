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
    }
}