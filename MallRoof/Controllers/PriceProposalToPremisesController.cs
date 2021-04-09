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
    [Authorize]
    public class PriceProposalToPremisesController : Controller
    {
        private MallContext db = new MallContext();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public PriceProposalToPremisesController()
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

        // GET: PriceProposalToPremises
        public ActionResult IndexForMe()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var priceProposalToPremises = db.PriceProposalToPremises.Include(p => p.Premise).Include(p => p.ProposingUser).Where(p=>p.Premise.Mall.UserId == user.Id)
                ;
            return View(priceProposalToPremises.ToList());
        }

        public ActionResult IndexFromMe()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var priceProposalToPremises = db.PriceProposalToPremises.Include(p => p.Premise).Include(p => p.ProposingUser).Where(p => p.ProposingUserId == user.Id)
                ;
            return View(priceProposalToPremises.ToList());
        }

        // GET: PriceProposalToPremises/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceProposalToPremise priceProposalToPremise = db.PriceProposalToPremises.Find(id);
            if (priceProposalToPremise == null)
            {
                return HttpNotFound();
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null && priceProposalToPremise.ProposingUserId == user.Id)
            {
                return View("OwnerDetails", priceProposalToPremise);
            }

            return View(priceProposalToPremise);
        }

        // GET: PriceProposalToPremises/Create
        public ActionResult Create(Guid PremiseId)
        {
            //ViewBag.PremiseId = new SelectList(db.Premises, "PremiseId", "Number");
            ViewBag.PremiseId = PremiseId;
            return View();
        }

        // POST: PriceProposalToPremises/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PriceProposalToPremiseId,ProposingUserId,PremiseId,Price,Comments")] PriceProposalToPremise priceProposalToPremise)
        {
            if (ModelState.IsValid)
            {
                priceProposalToPremise.PriceProposalToPremiseId = Guid.NewGuid();

                var user = UserManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    priceProposalToPremise.ProposingUserId = user.Id;
                }

                db.PriceProposalToPremises.Add(priceProposalToPremise);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            return View(priceProposalToPremise);
        }

        // GET: PriceProposalToPremises/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceProposalToPremise priceProposalToPremise = db.PriceProposalToPremises.Find(id);
            if (priceProposalToPremise == null)
            {
                return HttpNotFound();
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id != priceProposalToPremise.ProposingUserId && !User.IsInRole("Admin"))
            {
                ViewBag.ErrorMessage = "Редактировать может только владелец ценового предложения";
                return View("Error");
            }
            return View(priceProposalToPremise);
        }

        // POST: PriceProposalToPremises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PriceProposalToPremiseId,Price,Comments")] PriceProposalToPremise priceProposalToPremise)
        {
            if (ModelState.IsValid)
            {
                PriceProposalToPremise priceProposalToPremiseCur = db.PriceProposalToPremises.Find(priceProposalToPremise.PriceProposalToPremiseId);
                if (priceProposalToPremiseCur == null)
                {
                    return HttpNotFound();
                }

                var user = UserManager.FindById(User.Identity.GetUserId());
                if (user.Id != priceProposalToPremiseCur.ProposingUserId && !User.IsInRole("Admin"))
                {
                    ViewBag.ErrorMessage = "Редактировать может только владелец ценового предложения";
                    return View("Error");
                }

                priceProposalToPremiseCur.Price = priceProposalToPremise.Price;
                priceProposalToPremiseCur.Comments = priceProposalToPremise.Comments;

                db.Entry(priceProposalToPremiseCur).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = priceProposalToPremiseCur.PriceProposalToPremiseId });
            }
            
            return View(priceProposalToPremise);
        }

        // GET: PriceProposalToPremises/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PriceProposalToPremise priceProposalToPremise = db.PriceProposalToPremises.Find(id);
            if (priceProposalToPremise == null)
            {
                return HttpNotFound();
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id != priceProposalToPremise.ProposingUserId && !User.IsInRole("Admin"))
            {
                ViewBag.ErrorMessage = "Удалить может только владелец ценового предложения";
                return View("Error");
            }

            return View(priceProposalToPremise);
        }

        // POST: PriceProposalToPremises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PriceProposalToPremise priceProposalToPremise = db.PriceProposalToPremises.Find(id);
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id != priceProposalToPremise.ProposingUserId && !User.IsInRole("Admin"))
            {
                ViewBag.ErrorMessage = "Удалить может только владелец ценового предложения";
                return View("Error");
            }
            db.PriceProposalToPremises.Remove(priceProposalToPremise);
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
