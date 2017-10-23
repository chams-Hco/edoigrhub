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
   public class RevenueController:Controller
    {
        private IDataService DataContext;

        public RevenueController():this(MainContainer.DataService())
        {
            
        }

        public RevenueController(IDataService DataContext)     
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

            return View(DataContext.GetAllRevenues());
        }

        public ActionResult Ministries()
        {
            int ClientId = Convert.ToInt32(Session["ClientId"]);

            return View(DataContext.GetAllMinistryByClientId(ClientId));
        }

        public ActionResult RevenueHeads()
        {
            int ClientId = Convert.ToInt32(Session["ClientId"]);

            return View(DataContext.GetAllRevenueHeadByClientId(ClientId));
        }

        public ActionResult RevenueHeadByMinistry(int id)
        {
            return View(DataContext.GetAllRevenueHeadByMinistryId(id));
        }

        public ActionResult RevenueCategories()
        {
            int ClientId = Convert.ToInt32(Session["ClientId"]);

            return View(DataContext.GetAllRevenueCategoryByClientId(ClientId));
        }

        public ActionResult CategoryByRevenueHead(int id)
        {
            return View(DataContext.GetAllRevenueCategoryByRevenueHeadId(id));
        }

        public ActionResult RevenueItems()
        {
            int ClientId = Convert.ToInt32(Session["ClientId"]);

            return View(DataContext.GetAllRevenueItemByClientId(ClientId));
        }

        public ActionResult RevenueByCategory(int id)
        {
            return View(DataContext.GetAllRevenueItemByCategoryId(id));
        }

        public ActionResult RevenueByMinistry(int id)
        {
            return View(DataContext.GetAllRevenueItemByMinistryId(id));
        }

        public ActionResult RevenueByRevenueHead(int id)
        {
            return View(DataContext.GetAllRevenueItemByRevenueHeadId(id));
        }


        // GET: Admin/Level/Create
        public ActionResult Create()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            int ClientId = Convert.ToInt32(Session["ClientId"]);

            var model = new RevenueViewModel()
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
        public ActionResult Create(RevenueViewModel revenue)
        {
            if (ModelState.IsValid)
            {
                var revenueModel = new Revenue()
                {
                    RevenueId = revenue.RevenueId,
                    Code = revenue.Code,
                    MDA = revenue.MDA,
                    Name = revenue.Name,
                    Amount = revenue.Amount,
                    ClientId = revenue.SelectedClientId,
                    Status = revenue.Status
                };
                DataContext.AddRevenue(revenueModel);
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
            Revenue Revenue = DataContext.FindRevenueById(id);
            if (Revenue == null)
            {
                return HttpNotFound();
            }
            return View(Revenue);
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
            Revenue Revenue = DataContext.FindRevenueById(id);
            if (Revenue == null)
            {
                return HttpNotFound();
            }
            return View(new RevenueViewModel()
            {
                RevenueId = Revenue.RevenueId,
                ddlClients = Utility.GetClients(DataContext,RoleId,ClientId),
                SelectedClientId = Revenue.ClientId,
                Name = Revenue.Name,
                Code = Revenue.Code,
                Amount = Revenue.Amount,
                MDA = Revenue.MDA,
                Status = Revenue.Status,
   
            });
        }

        // POST: Admin/Level/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(RevenueViewModel revenue)
        {
            if (ModelState.IsValid)
            {
                var revenueModel = new Revenue()
                {
                    RevenueId = revenue.RevenueId,
                    Code = revenue.Code,
                    MDA = revenue.MDA,
                    Name = revenue.Name,
                    Amount = revenue.Amount,
                    ClientId = revenue.SelectedClientId,
                    Status = revenue.Status
                };
                DataContext.UpdateRevenue(revenueModel);
                return RedirectToAction("Index");
            }
            return View(revenue);
        }

        // GET: Admin/Level/Delete/5
        public ActionResult Delete(int id)
        {
            if (id <=0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Revenue Revenue = DataContext.FindRevenueById(id);
            if (Revenue == null)
            {
                return HttpNotFound();
            }
            return View(Revenue);
        }

        // POST: Admin/Level/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DataContext.DeleteRevenue(id);
            return RedirectToAction("Index");
        }

    }
}
