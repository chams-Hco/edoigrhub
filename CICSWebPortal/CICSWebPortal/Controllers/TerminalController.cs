using CICSWebPortal.Models;
using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class TerminalController : Controller
    {
      private IDataService DataContext;

        public TerminalController():this(MainContainer.DataService())
        {
            
        }

        public TerminalController(IDataService DataContext)     
        {
            this.DataContext = DataContext;
        }
        // GET: Client
        public ActionResult Index()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            //System Admin // User
            if (RoleId==1 || RoleId==2)
            {
                return View(DataContext.GetAllTerminals());
            }

            //Client Admin / USser
            else if (RoleId ==3 || RoleId == 4)
            {
                return View(DataContext.GetAllTerminalsByClientId(UserTypeParentId));
            }

            //Agent Admin / User
            else if (RoleId == 5 || RoleId == 6)
            {
                return View(DataContext.GetAllTerminalsByAgentId(UserTypeParentId));
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        // GET: Admin/Level/Edit/5
        public ActionResult FindTerminal(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Terminal terminal = DataContext.FindTerminalById(id);
            if (terminal == null)
            {
                return HttpNotFound();
            }
            return View(terminal);
        }

        public ActionResult TerminalsByAgent(int id)
        {
            return View(DataContext.GetAllTerminalsByAgentId(id));
        }

    }
}
