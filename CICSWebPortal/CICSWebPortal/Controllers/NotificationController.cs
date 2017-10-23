using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class NotificationController : Controller
    {
        private IDataService DataContext;

        public NotificationController():this(MainContainer.DataService())
        {

        }

        public NotificationController(IDataService DataContext)
        {
            this.DataContext = DataContext;
        }

        public ActionResult Index()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            if (RoleId > 2)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(DataContext.GetAllNotifications());
        }

    }
}