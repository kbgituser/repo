using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MallRoof.DAL;
using MallRoof.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


using Microsoft.AspNet.Identity.Owin;
using PagedList.Mvc;
using PagedList;


namespace MallRoof.Controllers
{
    public class DemandsController : Controller
    {
        private MallContext db = new MallContext();

        
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public DemandsController()
        {
            var userStore = new UserStore<User>(db);
            _userManager = ApplicationUserManager.CreateDefault(db);
            var roleStore = new RoleStore<IdentityRole>(db);
            _roleManager = new ApplicationRoleManager(roleStore);

        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }

        // GET: Demands
        public ActionResult Index()
        {
            return View(db.Demands.ToList());
        }

        // GET: Demands/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demand demand = db.Demands.Find(id);
            if (demand == null)
            {
                return HttpNotFound();
            }
            return View(demand);
        }

        // GET: Demands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Demands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DemandId,PriceFrom,PriceTo,AreaFrom,AreaTo,Message")] Demand demand)
        {
            if (ModelState.IsValid)
            {
                demand.DemandId = Guid.NewGuid();
                var user = UserManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    demand.UserId = user.Id;
                }
                db.Demands.Add(demand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(demand);
        }

        // GET: Demands/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demand demand = db.Demands.Find(id);
            if (demand == null)
            {
                return HttpNotFound();
            }
            return View(demand);
        }

        // POST: Demands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DemandId,PriceFrom,PriceTo,AreaFrom,AreaTo,Message")] Demand demand)
        {
            if (ModelState.IsValid)
            {

                //var demandInDB = db.Demands.Find(demand.DemandId);
                //demandInDB.PriceFrom = demand.PriceFrom;
                var user = UserManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    demand.UserId = user.Id;
                }

                db.Entry(demand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(demand);
        }

        // GET: Demands/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Demand demand = db.Demands.Find(id);
            if (demand == null)
            {
                return HttpNotFound();
            }
            return View(demand);
        }

        // POST: Demands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Demand demand = db.Demands.Find(id);
            db.Demands.Remove(demand);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
