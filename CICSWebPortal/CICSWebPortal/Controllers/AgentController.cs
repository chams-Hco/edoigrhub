using CICSWebPortal.Models;
using CICSWebPortal.Services;
using CICSWebPortal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CICSWebPortal.Controllers
{
    public class AgentController : Controller
    {
        private IDataService DataContext;

        public AgentController():this(MainContainer.DataService())
        {
            
        }

        public AgentController(IDataService DataContext)
        {
            this.DataContext = DataContext;
        }
        // GET: Client
        public ActionResult Index()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            if (RoleId < 3)
            {
                return View(DataContext.GetAllAgents());
            }

            else if(RoleId < 5 )
            {
                return View(DataContext.GetAllAgentsByClientId(UserTypeParentId));
            }
            else
            {
                return RedirectToAction("Index","Home");
            }
        }

        // GET: Client
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
            var model = new AgentViewModel()
                {
                    ddlClients=GetClients()
                };
            return View(model);
        }

        // POST: Admin/Level/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AgentViewModel Agent)
        {
            if (ModelState.IsValid)
            {
                var agentModel = new Agent()
                {
                    AgentId = Agent.AgentId,
                    ClientId = Agent.SelectedClientId,
                    Name = Agent.Name,
                    Company = Agent.Company,
                    Address = Agent.Address,
                    Email = Agent.Email,
                    PhoneNo1 = Agent.PhoneNo1,
                    PhoneNo2 = Agent.PhoneNo2,
                    Status = Agent.Status,
                    clientId = Convert.ToInt32(Session["ClientId"]),
                    userId = Convert.ToInt32(Session["UserId"])
                };

                DataContext.AddAgent(agentModel);
                return RedirectToAction("Index");
            }

            return View(Agent);
        }

        // GET: Admin/Level/Edit/5
        public ActionResult Edit(int id)
        {
            if (id <=0 )
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Agent Agent =DataContext.FindAgentById(id);
            if (Agent == null)
            {
                return HttpNotFound();
            }
            return View(new AgentViewModel()
            {
                AgentId = Agent.AgentId,
                ddlClients = GetClients(),
                SelectedClientId = Agent.ClientId,
                Name = Agent.Name,
                Address = Agent.Address,
                Company = Agent.Company,
                Email = Agent.Email,
                PhoneNo1 = Agent.PhoneNo1,
                PhoneNo2 = Agent.PhoneNo2,
                Status = Agent.Status
            });
        }

        // POST: Admin/Level/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AgentViewModel Agent)
        {
            if (ModelState.IsValid)
            {
                var agentModel = new Agent()
                {
                    AgentId = Agent.AgentId,
                    ClientId = Agent.SelectedClientId,
                    Name = Agent.Name,
                    Company = Agent.Company,
                    Address = Agent.Address,
                    Email = Agent.Email,
                    PhoneNo1 = Agent.PhoneNo1,
                    PhoneNo2 = Agent.PhoneNo2,
                    Status = Agent.Status,
                    clientId = Convert.ToInt32(Session["ClientId"]),
                    userId = Convert.ToInt32(Session["UserId"])
                };
                DataContext.UpdateAgent(agentModel);
                return RedirectToAction("Index");
            }
            return View("Index");         
        }

        public ActionResult AgentByClient(int id)
        {
            return View(DataContext.GetAllAgentsByClientId(id));
        }

        // GET: Admin/Level/Delete/5
        public ActionResult Delete(int id)
        {
            if (id <=0)
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

        // POST: Admin/Level/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataContext.DeleteAgent(id);
            return RedirectToAction("Index");
        }

    }
}
