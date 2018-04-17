using CICSWebPortal.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CICSWebPortal.Models;
using CICSWebPortal.ViewModels;
using CICSWebPortal.Helpers;

namespace CICSWebPortal.Controllers
{
    public class UserController : Controller
    {
        private IDataService DataContext;

        public UserController()
            : this(MainContainer.DataService())
        {

        }

        public UserController(IDataService DataContext)
        {
            this.DataContext = DataContext;
        }

        // GET: User
        public ActionResult Index()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            string RoleCode = (string)Session["RoleCode"];
            int ClientId = Convert.ToInt32(Session["ClientId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            if (RoleCode == "SA")
            {
                return View(DataContext.GetAllUsers());
            }

            else if (RoleCode == "CA")
            {
                return View(DataContext.GetUserAssesibleUsers(RoleId, ClientId));
            }
            else if (RoleCode == "AA")
            {
                return View(DataContext.GetUserAssesibleUsers(RoleId, UserTypeParentId));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User User = DataContext.FindUserById(id);
            if (User == null)
            {
                return HttpNotFound();
            }
            return View(User);
        }

        //[Route("[action]/{status?}/{message?}")]
        public ActionResult WebUser(int status = 0, string message = "")
        {
            var user = (UserDashBoardViewModel)Session["LoggedInUser"];
            if (user == null)
            {
                ViewBag.Status = false;
                ViewBag.Message = "Please, refresh your browser or reinitiate sign in, your current session has expired.";

                return View();
            }

            
            
             
           

            if (user.RoleCode == "SA")
            {
                var clients = DataContext.GetAllClientWithZones().Where(a => a.HasWebUsers == true);
                var users = DataContext.GetAllUsers().Where(a => a.RoleCode == "WU");
                ViewBag.Clients = clients;
                return View(users);

                
            }
            else if (user.RoleCode == "CA")
            {

                var clients = DataContext.FindClientWithZone(user.ClientId);
                if (clients.HasWebUsers == false)
                {
                    ViewBag.Status = false;
                    ViewBag.Message = "This Client has not been setup to have Web Users, Please contact super administrators";
                    return View();
                }

                var users = DataContext.GetAllUsersByClientId(user.ClientId).Where(a => a.RoleCode == "WA");
                ViewBag.Clients = clients;
                return View(users);
            }
            else
            {
                ViewBag.Status = false;
                ViewBag.Message = "You do not have sufficient permission to view this page, Please contact administrator";
                return View();
            }


        }

        public ActionResult CreateWebUser()
        {
            var user = (UserDashBoardViewModel)Session["LoggedInUser"];
            var model = new WebUserViewModel()
            {

            };
            if (user == null)
            {
                ViewBag.Status = false;
                ViewBag.Message = "Please, refresh your browser or reinitiate sign in, your current session has expired.";
                return View();
            }

            if (user.CanCreateWebUsers == false)
            {
                ViewBag.Status = false;
                ViewBag.Message = "You cannot create web users";

                return View();
            }

            if (user.RoleCode == "SA")
            {
                var clients = DataContext.GetAllClientWithZones().Where(a => a.HasWebUsers == true);
                //   var users = DataContext.GetAllUsers().Where(a => a.RoleCode == "WU");
                ViewBag.Status = true;
                ViewBag.Clients = clients;

                model.Clients = clients.Select(a=> new SelectListItem { Text = a.ClientName, Value = a.clientId.ToString() });
                model.Agents = new List<SelectListItem>();
                return View(model);


            }
            else if (user.RoleCode == "CA")
            {

                var clients = DataContext.FindClientWithZone(user.ClientId);
                if (clients.HasWebUsers == false)
                {
                    ViewBag.Status = false;
                    ViewBag.Message = "This Client has not been setup to have Web Users, Please contact super administrators";
                    return View();
                }

                // var users = DataContext.GetAllUsersByClientId(user.ClientId).Where(a => a.RoleCode == "WU");
                ViewBag.Status = true;
                ViewBag.Clients = clients;
                var clientList = new List<SelectListItem>();
                clientList.Add(new SelectListItem { Text = clients.ClientName, Value = clients.ClientId.ToString() });
                model.Clients = clientList;
                model.Agents = clients.Agents.Select(a => new SelectListItem { Text = a.Name, Value = a.AgentId.ToString() });
                
                return View(model);
            }
            else
            {
                ViewBag.Status = false;
                ViewBag.Message = "You do not have sufficient permission to view this page, Please contact administrator";
                return View();
            }
            
            
        }

        [HttpPost]
        public ActionResult CreateWebUser(WebUserViewModel model)
        {
            var user = (UserDashBoardViewModel)Session["LoggedInUser"];
            var agent = DataContext.FindAgentById(model.SelectedAgentId);
            var role = DataContext.FindRoleByCode("WA");

            if(!ModelState.IsValid)
            {
                return RedirectToAction("WebUser",new {status= false,  message="Invalid Parameters" });
            }

            if(agent == null)
            {
                return RedirectToAction("WebUser", new { status = false, message = "Please select appropriate zone" });
            }

            if (user == null)
            {
                return RedirectToAction("WebUser", new { status = false, message = "You need to Sign In" });
            }
            if (user.CanCreateWebUsers == false)
            {
                return RedirectToAction("WebUser", new { status = false, message = "You cannot create web users" });
            }
            if (role == null)
            {
                return RedirectToAction("WebUser", new { status = false, message = "Please contact Super Administrators" });
            }

            model.RoleId = role.RoleId;
            model.Status = true;
            model.AgentCode = agent.Code;
            model.AgentUsername = user.Email;
            model.createdby = user.UserId;
            

            var result = DataContext.AddWebUser(model);
            if(result != null)
            {
                return RedirectToAction("WebUser", new { status = true, message = "Saved Successfully" });
            }
            else
            {
                return RedirectToAction("WebUser", new { status = false, message = "Error saving Web User" });
            }


            
           


        }

        // GET: User/Create
        public ActionResult Create()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int ClientId = Convert.ToInt32(Session["ClientId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);


            IEnumerable<SelectListItem> clients = Utility.GetClients(DataContext, RoleId, ClientId);
            IEnumerable<SelectListItem> agents = Utility.GetAgents(DataContext, RoleId, UserTypeParentId);

            var model = new UserViewModel()
            {
                ddlRoles = Utility.GetRoles(DataContext, Convert.ToInt32(Session["RoleId"])),
                ddlClients = clients,
                ddlAgents = agents,
            };
            return View(model);
        }

        // POST: User/Create
        [HttpPost]
        public ActionResult Create(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                
                var User = new User
                {
                    UserId = userVM.UserId,
                    RoleId = userVM.SelectedRoleId,
                    UserTypeParentId = Utility.GetParentId(userVM.SelectedRoleId, userVM.SelectedClientId, userVM.SelectedAgentId),
                    ClientId = Convert.ToInt32(Session["ClientId"]),
                    Mobile = userVM.Mobile,
                    Email = userVM.Email,
                    Password = userVM.Password,
                    Status = userVM.Status,
                    clientId = Convert.ToInt32(Session["ClientId"]),
                    userId = Convert.ToInt32(Session["UserId"])
                };

                DataContext.AddUser(User);
                return RedirectToAction("Index");
            }

            return View(userVM);
        }

        // GET: User/Edit/5
        public ActionResult ChangeStatus(int id)
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int ClientId = Convert.ToInt32(Session["ClientId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User User = DataContext.FindUserById(id);
            if (User == null)
            {
                return HttpNotFound();
            }

            return View(new UserViewModel()
            {
                UserId = User.UserId,
                SelectedRoleId = User.RoleId,
                Email = User.Email,
                Mobile = User.Mobile,
                Password = User.Password,
                Status = User.Status

            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeStatus(UserViewModel userVM)
        {

            var User = new User()
            {
                UserId = userVM.UserId,
                RoleId = userVM.SelectedRoleId,
                Mobile = userVM.Mobile,
                Email = userVM.Email,
                Password = userVM.Password,
                Status = userVM.Status,
                clientId = Convert.ToInt32(Session["ClientId"]),
                userId = Convert.ToInt32(Session["UserId"])
            };
            DataContext.UpdateUserStatus(User);

            return RedirectToAction("Index");
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult ChangePassword(ChangeUserPasswordModel vals)
        {

            vals.clientId = Convert.ToInt32(Session["ClientId"]);
            vals.userId = Convert.ToInt32(Session["UserId"]);

            DataContext.ChangeUserPassword(vals);

            return RedirectToAction("Index", "Home");
        }

        // GET: User/Delete/5
        public ActionResult ResetPassword(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User User = DataContext.FindUserById(id);
            if (User == null)
            {
                return HttpNotFound();
            }

            return View(new UserViewModel()
            {
                UserId = User.UserId,
                SelectedRoleId = User.RoleId,
                Email = User.Email,
                Mobile = User.Mobile,
                Password = User.Password,
                Status = User.Status

            });
        }

        // POST: User/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(UserViewModel user)
        {
            try
            {
                // TODO: Add delete logic here
                DataContext.ResetUserPassword(new ResetPasswordModel
                {
                    UserId = user.UserId,
                    clientId = Convert.ToInt32(Session["ClientId"]),
                    userId = Convert.ToInt32(Session["UserId"])

                });

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}