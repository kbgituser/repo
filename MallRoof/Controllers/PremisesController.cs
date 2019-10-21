using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MallRoof.DAL;
using MallRoof.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace MallRoof.Controllers
{
    public class PremisesController : Controller
    {
        private MallContext db = new MallContext();

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public PremisesController()
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

        // GET: Premises
        public ActionResult Index(string mallId, string price, string area, string haswindow, string priceorder, string order)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                ViewBag.Malls = user.Malls;
            }
            PremisesMallListModel premisesMallListModel = new PremisesMallListModel();

            var premises = db.Premises.AsQueryable();
            int priceint;
            if (Int32.TryParse(price, out priceint))
            {
                premises = premises.Where(p => p.Price <= priceint);
            }
            

            Guid mallIdg;
            if (!string.IsNullOrEmpty(mallId) && Guid.TryParse(mallId, out mallIdg))
            {
                premises = premises.Where(p => p.Mall.MallId == mallIdg);
            }

            int areaint;
            if (Int32.TryParse(area, out areaint))
            {
                premises = premises.Where(p => p.Area <= areaint);
            }

            if (!string.IsNullOrEmpty(haswindow))
            {
                var haswindowb = bool.Parse(haswindow);
                if (haswindowb)
                {
                    premises = premises.Where(p => p.HasWindow == haswindowb);
                }
                
            }

            switch (order)
            {
                case "price":
                    premises = premises.OrderBy(s => s.Price);
                    break;
                case "price_desc":
                    premises = premises.OrderByDescending(s => s.Price);
                    break;
                case "area":
                    premises = premises.OrderBy(s => s.Area);
                    break;
                case "area_desc":
                    premises = premises.OrderByDescending(s => s.Area);
                    break;
                default:
                    premises = premises.OrderBy(s => s.Price);
                    break;
            }


            ViewBag.PriceSortParam = order == "price" ? "price_desc" : "price";
            ViewBag.AreaSortParam = order == "area" ? "area_desc" : "area";
            premisesMallListModel.Malls = db.Malls.ToList();
            premisesMallListModel.Premises = premises.ToList();
            return View(premisesMallListModel);
            //return View(db.Premises.ToList());
        }
           

    // GET: Premises/Details/5
    public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premise premise = db.Premises.Find(id);
            if (premise == null)
            {
                return HttpNotFound();
            }
            return View(premise);
        }

        // GET: Premises/Create        

        public ActionResult Create(Guid Id)
        {
            ViewBag.MallId = Id;
            return View();
        }

        // POST: Premises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PremiseId,Number,Floor,Area,IsLastFloor,HasWindow,Description,Price,IsSeen,MallId")] Premise premise)
        {
            if (ModelState.IsValid)
            {
                premise.PremiseId = Guid.NewGuid();
                db.Premises.Add(premise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(premise);
        }

        // GET: Premises/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premise premise = db.Premises.Find(id);
            if (premise == null)
            {
                return HttpNotFound();
            }
            return View(premise);
        }

        // POST: Premises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PremiseId,Number,Floor,Area,IsLastFloor,HasWindow,Description,Price,IsSeen")] Premise premise)
        {
            if (ModelState.IsValid)
            {
                db.Entry(premise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(premise);
        }

        // GET: Premises/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Premise premise = db.Premises.Find(id);
            if (premise == null)
            {
                return HttpNotFound();
            }
            return View(premise);
        }

        // POST: Premises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Premise premise = db.Premises.Find(id);
            db.Premises.Remove(premise);
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
