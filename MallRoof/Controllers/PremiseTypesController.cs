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

namespace MallRoof.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PremiseTypesController : Controller
    {
        private MallContext db = new MallContext();

        // GET: PremiseTypes
        public ActionResult Index()
        {
            return View(db.PremiseTypes.ToList());
        }

        // GET: PremiseTypes/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PremiseType premiseType = db.PremiseTypes.Find(id);
            if (premiseType == null)
            {
                return HttpNotFound();
            }
            return View(premiseType);
        }

        // GET: PremiseTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PremiseTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PremiseTypeId,Name")] PremiseType premiseType)
        {
            if (ModelState.IsValid)
            {
                premiseType.PremiseTypeId = Guid.NewGuid();
                db.PremiseTypes.Add(premiseType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(premiseType);
        }

        // GET: PremiseTypes/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PremiseType premiseType = db.PremiseTypes.Find(id);
            if (premiseType == null)
            {
                return HttpNotFound();
            }
            return View(premiseType);
        }

        // POST: PremiseTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PremiseTypeId,Name")] PremiseType premiseType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(premiseType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(premiseType);
        }

        // GET: PremiseTypes/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PremiseType premiseType = db.PremiseTypes.Find(id);
            if (premiseType == null)
            {
                return HttpNotFound();
            }
            return View(premiseType);
        }

        // POST: PremiseTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PremiseType premiseType = db.PremiseTypes.Find(id);
            db.PremiseTypes.Remove(premiseType);
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
