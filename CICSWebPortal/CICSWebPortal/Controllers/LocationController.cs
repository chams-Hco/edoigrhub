using CICSWebPortal.Models;
using CICSWebPortal.Services;
using CICSWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class LocationController : Controller
    {
        private IDataService DataContext;

        public LocationController():this(MainContainer.DataService())
        {

        }

        public LocationController(IDataService DataContext)
        {
            this.DataContext = DataContext;
        }
        // GET: Location
        public ActionResult Index()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            if (RoleId < 3)
            {
                return View(DataContext.GetAllLocations());
            }

            else if (RoleId < 5)
            {
                return View(DataContext.GetAllLocationsByClientId(UserTypeParentId));
            }
            else
            {
                return View(DataContext.GetAllLocationsByAgentId(UserTypeParentId));
            }
        }

        // GET: Location
        public ActionResult Manage()
        {
            return View(DataContext.GetAllAgents());
        }

        // GET: Admin/Level/Details/5
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent Agent = DataContext.FindAgentById(id);
            if (Agent == null)
            {
                return HttpNotFound();
            }
            return View(Agent);
        }

        private IEnumerable<SelectListItem> GetClients()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            if (RoleId < 3)
            {
                var types = DataContext.GetAllClients().Select(x =>
                                    new SelectListItem
                                    {
                                        Value = x.ClientId.ToString(),
                                        Text = x.ClientName
                                    });

                return new SelectList(types, "Value", "Text");
            }
            else if (RoleId < 5)
            {
                var client = DataContext.FindClientById(UserTypeParentId);
                List<SelectListItem> types = new List<SelectListItem>{
                    new SelectListItem { Value = client.ClientId.ToString(),Text = client.ClientName} };

                return new SelectList(types, "Value", "Text");

            }
            else
            {
                return null;
            }
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
        public ActionResult Create(Location location)
        {
            int AgentId = Convert.ToInt32(Session["UserTypeParentId"]);
           int ClientId = Convert.ToInt32(Session["ClientId"]);

            if (ModelState.IsValid)
            {
                var locationModel = new Location()
                {
                    AgentId = AgentId,
                    ClientId = ClientId,
                    LocationCode = location.LocationCode,
                    LocationDescription = location.LocationDescription
                };

                DataContext.AddLocation(locationModel);
                return RedirectToAction("Index");
            }

            return View(location);
        }

        // GET: Admin/Level/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location Location = DataContext.FindLocationById(id);
            if (Location == null)
            {
                return HttpNotFound();
            }
            return View(new Location()
            {
                Id = Location.Id,
                LocationCode = Location.LocationCode,
                AgentId = Location.AgentId,
                ClientId = Location.ClientId,
                LocationDescription = Location.LocationDescription
            });
        }

        // POST: Admin/Level/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Location Location)
        {
            if (ModelState.IsValid)
            {
                var locationModel = new Location()
                {
                    Id = Location.Id,
                    LocationCode = Location.LocationCode,
                    AgentId = Location.AgentId,
                    ClientId = Location.ClientId,
                    LocationDescription = Location.LocationDescription
                };
                DataContext.UpdateLocation(locationModel);
                return RedirectToAction("Index");
            }
            return View("Index");
        }

        public ActionResult LocationByClient(int id)
        {
            return View(DataContext.GetAllLocationsByClientId(id));
        }

        // GET: Admin/Level/Delete/5
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location Location = DataContext.FindLocationById(id);
            if (Location == null)
            {
                return HttpNotFound();
            }
            return View(Location);
        }

        // POST: Admin/Level/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataContext.DeleteLocation(id);
            return RedirectToAction("Index");
        }
    }
}