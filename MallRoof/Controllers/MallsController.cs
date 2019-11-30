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

namespace MallRoof.Controllers
{
    public class MallsController : Controller
    {
        private MallContext db = new MallContext();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public MallsController()
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

        // GET: Malls
        [Authorize]
        public ActionResult Index(string getMine)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var malls = db.Malls.AsQueryable();
            if ( user!=null &&  !string.IsNullOrEmpty(getMine) &&  bool.TrueString == getMine)
            {
                malls = malls.Where(m => m.UserId == user.Id);
                return View(malls.ToList());
            }
            else
            {
                return View(malls.ToList());
            }
            
        }

        

        // GET: Malls/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mall mall = db.Malls.Find(id);
            if (mall == null)
            {
                return HttpNotFound();
            }
            return View(mall);
        }

        // GET: Malls/Create
        public ActionResult Create()
        {
            return View();
        }
        

        // POST: Malls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MallId,Name,Address,UserId,NumberOfFloors")] Mall mall)
        {
            if (ModelState.IsValid)
            {
                mall.MallId = Guid.NewGuid();

                var user = UserManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    mall.UserId =  user.Id;
                }

                db.Malls.Add(mall);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mall);
        }

        // GET: Malls/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mall mall = db.Malls.Find(id);
            if (mall == null)
            {
                return HttpNotFound();
            }
            return View(mall);
        }

        // POST: Malls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MallId,Name,Address,UserId,NumberOfFloors")] Mall mall)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mall).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mall);
        }

        // GET: Malls/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mall mall = db.Malls.Find(id);
            if (mall == null)
            {
                return HttpNotFound();
            }
            return View(mall);
        }

        // POST: Malls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Mall mall = db.Malls.Find(id);
            db.Malls.Remove(mall);
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
