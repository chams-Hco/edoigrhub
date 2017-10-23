using CICSWebPortal.Services;
using CICSWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using CICSWebPortal.Helpers;
using CICSWebPortal.Models;
using System.Net;

namespace CICSWebPortal.Controllers
{
   public class IdentityController: Controller
    {
        private IDataService DataContext;

        public IdentityController():this(MainContainer.DataService())
        {
            
        }

        public IdentityController(IDataService DataContext)     
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

            return View(DataContext.GetAllIdentity());
        }


        // GET: Admin/Level/Create
        public ActionResult Create()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int ClientId = Convert.ToInt32(Session["ClientId"]);

            var model = new IdentityViewModel()
            {
                ddlClients = Utility.GetClients(DataContext,RoleId,ClientId)
            };
            return View(model);
        }

        // POST: Admin/Level/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IdentityViewModel identity)
        {
            if (ModelState.IsValid)
            {
                var identityModel = new Identity()
                {
                    IdentityId = identity.IdentityId,
                    ClientId = identity.SelectedClientId,
                    URL = identity.URL,
                    UserName = identity.UserName,
                    Password = identity.Password,
                    Status = identity.Status,
                    clientId = Convert.ToInt32(Session["ClientId"]),
                    userId = Convert.ToInt32(Session["UserId"])
                };
                DataContext.AddIdentity(identityModel);
                return RedirectToAction("Index");
            }

            return View();
        }



        // GET: Admin/Level/Details/5
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Identity Identity = DataContext.FindIdentityId(id);
            if (Identity == null)
            {
                return HttpNotFound();
            }
            return View(Identity);
        }


        // GET: Admin/Level/Edit/5
        public ActionResult Edit(int id)
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int ClientId = Convert.ToInt32(Session["ClientId"]);

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Identity Identity = DataContext.FindIdentityId(id);
            if (Identity == null)
            {
                return HttpNotFound();
            }
            return View(new IdentityViewModel()
            {
                IdentityId = Identity.IdentityId,
                ddlClients = Utility.GetClients(DataContext,RoleId,ClientId),
                SelectedClientId = Identity.ClientId,
                URL = Identity.URL,
                UserName = Identity.UserName,
                Password = Identity.Password,
                Status = Identity.Status
            });
        }

        // POST: Admin/Level/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IdentityViewModel identity)
        {
            if (ModelState.IsValid)
            {
                var _identity = new Identity()
                {
                    IdentityId = identity.IdentityId,
                    ClientId = identity.SelectedClientId,
                    URL = identity.URL,
                    UserName = identity.UserName,
                    Password = identity.Password,
                    Status = identity.Status,                    
                    clientId = Convert.ToInt32(Session["ClientId"]),
                    userId = Convert.ToInt32(Session["UserId"])
                };
                DataContext.UpdateIdentity(_identity);
                return RedirectToAction("Index");
            }
            return View(identity);
        }

        // GET: Admin/Level/Delete/5
        public ActionResult Delete(int id)
        {
            if (id <=0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Identity Identity = DataContext.FindIdentityId(id);
            if (id <=0)
            {
                return HttpNotFound();
            }
            return View(Identity);
        }

        // POST: Admin/Level/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataContext.DeleteIdentity(id);
            return RedirectToAction("Index");
        }

    }
}
