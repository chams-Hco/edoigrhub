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
            int ClientId = Convert.ToInt32(Session["ClientId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);

            if (RoleId == 1 )
            {
                return View(DataContext.GetAllUsers());
            }

            else if (RoleId == 3)
            {
                return View(DataContext.GetUserAssesibleUsers(RoleId,ClientId));
            }
            else if (RoleId == 5)
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

        // GET: User/Create
        public ActionResult Create()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int ClientId = Convert.ToInt32(Session["ClientId"]);
            int UserTypeParentId = Convert.ToInt32(Session["UserTypeParentId"]);


            IEnumerable<SelectListItem> clients = Utility.GetClients(DataContext,RoleId,ClientId);
            IEnumerable<SelectListItem> agents = Utility.GetAgents(DataContext,RoleId,UserTypeParentId);

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

            return RedirectToAction("Index","Home");
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
                DataContext.ResetUserPassword(new ResetPasswordModel{
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