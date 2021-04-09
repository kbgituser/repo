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
using System.Configuration;

namespace MallRoof.Controllers
{
    public class ProposalsController : Controller
    {
        private MallContext db = new MallContext();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public ProposalsController() {
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

        // GET: Proposals
        [Authorize]
        public ActionResult Index(string getMine, int? page)
        {
            var proposals = db.Proposals.OrderByDescending(p => p.CreateDate).Include(p => p.Demand).Include(p => p.LandLordUser);
            var user = UserManager.FindById(User.Identity.GetUserId());

            int pageSize = 10;
            string pSize = ConfigurationManager.AppSettings["pageSize"];
            if (!string.IsNullOrEmpty(pSize))
            {
                int.TryParse(pSize, out pageSize);
            }
            int pageNumber = (page ?? 1);

            if (User.IsInRole("Admin"))
            {
                return View(proposals.ToPagedList(pageNumber, pageSize));
            }
            if (user != null )
            {
                proposals = user.Proposals.OrderByDescending(p => p.CreateDate).AsQueryable();
            }
            
            return View(proposals.ToPagedList(pageNumber, pageSize));
        }

        // GET: Proposals/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposal proposal = db.Proposals.Find(id);
            if (proposal == null)
            {
                return HttpNotFound();
            }
            return View(proposal);
        }

        // GET: Proposals/Create
        [Authorize]
        public ActionResult Create(string demandId)
        {
            Guid demId = new Guid();
            if(!(Guid.TryParse(demandId, out demId)))
                return RedirectToAction("Index", "Demands", new { getMine = true});
            var demand = db.Demands.FirstOrDefault(d => d.DemandId == demId);
            if (demand.DemandStatus != DemandStatus.Active)
                return RedirectToAction("Index", "Demands", new { getMine = true });

            var user = UserManager.FindById(User.Identity.GetUserId());
            var premises = db.Premises.Where(p => p.Mall.UserId == user.Id).ToList();

            ProposalCreateModel proposalCreateModel = new ProposalCreateModel();
            proposalCreateModel.DemandId = demandId;
            proposalCreateModel.Premises = db.Premises.Where(p => p.Mall.UserId == user.Id);
            proposalCreateModel.CanCreate = proposalCreateModel.Premises.Any();
            proposalCreateModel.Proposal = new Proposal();

            ViewBag.DemandId = demandId;
            ViewBag.Premises = premises;
            ViewBag.CanCreate = premises.Any();
            if (!premises.Any())
                Session["demandId"] = demandId;
            return View(proposalCreateModel);
        }

        // POST: Proposals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "ProposalId,DemandId,Price,Description,UserId,PremiseId")] Proposal proposal)
        {
            if (ModelState.IsValid)
            {

                if (Guid.Empty == proposal.DemandId )
                {
                    return RedirectToAction("Index","Demands");
                }

                if (Guid.Empty == proposal.PremiseId)
                {
                    TempData["Error"] = "Не выбрано помещение";
                    return RedirectToAction("Create", new { DemandId = proposal.DemandId });
                }

                
                var demand = db.Demands.FirstOrDefault(d => d.DemandId == proposal.DemandId);
                if (demand.DemandStatus != DemandStatus.Active)
                    return RedirectToAction("Index", "Demands", new { getMine = true });

                proposal.ProposalId = Guid.NewGuid();
                proposal.CreateDate = DateTime.Now;
                var user = UserManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    proposal.UserId = user.Id;
                }

                db.Proposals.Add(proposal);
                db.SaveChanges();
                return RedirectToAction("Index", "Proposals");
            }

            //ViewBag.DemandId = new SelectList(db.Demands, "DemandId", "UserId", proposal.DemandId);
            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Email", proposal.UserId);



            //return View(proposal);
            //return RedirectToAction("Create", new { demandId = proposal.DemandId });
            return RedirectToAction("Create", new { demandId = proposal.DemandId });
        }

        // GET: Proposals/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposal proposal = db.Proposals.Find(id);
            if (proposal == null)
            {
                return HttpNotFound();
            }
            //ViewBag.DemandId = new SelectList(db.Demands, "DemandId", "UserId", proposal.DemandId);
            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Email", proposal.UserId);
            return View(proposal);
        }

        // POST: Proposals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProposalId,Price,Description")] Proposal proposal)
        {
            if (ModelState.IsValid)
            {
                var proposalinDb = db.Proposals.FirstOrDefault(p => p.ProposalId == proposal.ProposalId);
                if (proposalinDb == null)
                {
                    return RedirectToAction("Edit",new { ProposalId = proposal.ProposalId});
                }
                proposalinDb.Price = proposal.Price;
                proposalinDb.Description = proposal.Description;
                db.Entry(proposalinDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.DemandId = new SelectList(db.Demands, "DemandId", "UserId", proposal.DemandId);
            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "Email", proposal.UserId);
            return View(proposal);
        }

        // GET: Proposals/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposal proposal = db.Proposals.Find(id);
            if (proposal == null)
            {
                return HttpNotFound();
            }
            return View(proposal);
        }

        // POST: Proposals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Proposal proposal = db.Proposals.Find(id);
            db.Proposals.Remove(proposal);
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
