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
    public class ClientController : Controller
    {

        private IDataService DataContext;

        public ClientController():this(MainContainer.DataService())
        {
            
        }

        public ClientController(IDataService DataContext)     
        {
            this.DataContext = DataContext;
        }
        // GET: Client
        public ActionResult Index()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            if (RoleId > 2)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(DataContext.GetAllClients());
        }

        // GET: Admin/Level/Details/5
        public ActionResult Details(int id)
        {
            if (id<=0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client Client = DataContext.FindClientById(id);
            if (Client == null)
            {
                return HttpNotFound();
            }
            return View(Client);
        }

        // GET: Admin/Level/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Level/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Client Client)
        {
            if (ModelState.IsValid)
            {
                Client.clientId = Convert.ToInt32(Session["ClientId"]);
                Client.userId = Convert.ToInt32(Session["UserId"]);
                DataContext.AddClient(Client);
                return RedirectToAction("Index");
            }

            return View(Client);
        }

        // GET: Admin/Level/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client Client = DataContext.FindClientById(id);
            if (Client == null)
            {
                return HttpNotFound();
            }
            return View(Client);
        }

        // POST: Admin/Level/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Client Client)
        {
            if (ModelState.IsValid)
            {
                Client.clientId = Convert.ToInt32(Session["ClientId"]);
                Client.userId = Convert.ToInt32(Session["UserId"]);
                DataContext.UpdateClient(Client);
                return RedirectToAction("Index");
            }
            return View(Client);
        }

        // GET: Admin/Level/Delete/5
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client Client =DataContext.FindClientById(id);
            if (Client == null)
            {
                return HttpNotFound();
            }
            return View(Client);
        }

        // POST: Admin/Level/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataContext.DeleteClient(id);
            return RedirectToAction("Index");
        }



    }
}
