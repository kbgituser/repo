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

        public List<City> GetCities()
        {
            var cities = db.Cities.OrderBy(c => c.Name).ToList();
            var nur = cities.FirstOrDefault(c => c.Name.Contains("Нур-Султан"));
            cities.Remove(nur);
            cities.Insert(0, nur);
            return cities;
        }

        // GET: Demands
        public ActionResult Index(string getMine, int? page
            , string priceFrom
            , string priceTo
            , string areaFrom
            , string areaTo
            , string hasWindow
            , string floorFrom
            , string floorTo
            )
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var Demands = db.Demands.OrderByDescending(d=>d.CreateDate).AsQueryable();            
            ViewBag.Cities = GetCities();
            int pageSize = 10;
            string pSize = ConfigurationManager.AppSettings["pageSize"];
            if (!string.IsNullOrEmpty(pSize))
            {
                int.TryParse(pSize, out pageSize);
            }
            int pageNumber = (page ?? 1);

            int priceFromint;
            if (Int32.TryParse(priceFrom, out priceFromint))
            {
                Demands = Demands.Where(p => p.PriceFrom >= priceFromint);
            }

            int priceToint;
            if (Int32.TryParse(priceTo, out priceToint))
            {
                Demands = Demands.Where(p => p.PriceTo <= priceToint);
            }

            int areaFromint;
            if (int.TryParse(areaFrom, out areaFromint))
            {
                Demands = Demands.Where(p => p.AreaFrom >= areaFromint);
            }

            int areaToint;
            if (int.TryParse(areaTo, out areaToint))
            {
                Demands = Demands.Where(p => p.AreaTo <= areaToint);
            }

            //bool hasWindowb ;
            //if (bool.TryParse(hasWindow, out hasWindowb))
            //{
            //    Demands = Demands.Where(p => p.HasWindow == hasWindowb);
            //}

            if (user != null && User.IsInRole("Admin"))
            {
                return View("IndexLandlord", Demands.ToPagedList(pageNumber, pageSize));
            }
            
            if (user != null && bool.TrueString == getMine)
            {
                //Demands = user.Demands.AsQueryable();
                Demands = Demands.Where(d => d.UserId == user.Id);
                return View("IndexLandlord", Demands.ToPagedList(pageNumber, pageSize));
            }
            return View(Demands.ToPagedList(pageNumber, pageSize));
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
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null && demand.UserId == user.Id)
            {
                ViewBag.IsOwner = true;
            }

            return View(demand);
        }

        // GET: Demands/Create
        [Authorize]
        public ActionResult Create()
        {
            var demand = new Demand();
            int demandDuration = Int32.Parse(ConfigurationManager.AppSettings["maxDemandDuration"]);
            demand.EndDate = DateTime.Now.AddDays(demandDuration);
            demand.HasWindow = true;
            ViewBag.Cities = GetCities();
            return View(demand);
        }

        // POST: Demands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DemandId,PriceFrom,PriceTo,AreaFrom,AreaTo,Message,Phone,CreateDate,EndDate,CityId,HasWindow")] Demand demand)
        {
            ViewBag.Cities = GetCities();
            if (ModelState.IsValid)
            {
                if (!CheckEndDate(demand.EndDate))
                {
                    ModelState.AddModelError("EndDate", "Дата окончания приема предложений должна быть между " + DateTime.Today.ToString("dd.MM.yyyy") + " и " + DateTime.Today.AddDays(7).ToString("dd.MM.yyyy"));
                    return View(demand);
                }
                    

                var user = UserManager.FindById(User.Identity.GetUserId());
                if (user != null)
                {
                    demand.UserId = user.Id;
                }
                demand.CreateDate = DateTime.Now;
                demand.DemandStatus = DemandStatus.Created;
                db.Demands.Add(demand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View(demand);
        }

        public bool CheckEndDate(DateTime endDate)
        {
            int demandDuration = Int32.Parse(ConfigurationManager.AppSettings["maxDemandDuration"]);
            if ( endDate >= DateTime.Today && endDate <= DateTime.Today.AddDays(demandDuration))
                return true;
            else
                return false;
        }
        

        // GET: Demands/Edit/5
        [Authorize]
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
            ViewBag.Cities = GetCities();
            var user = UserManager.FindById(User.Identity.GetUserId());
            if ((user.Id != demand.UserId && !User.IsInRole("Admin"))
                //||(demand.DemandStatus != DemandStatus.Created)
                )
            {
                return RedirectToAction("Index", new { getMine = true });
            }
            return View(demand);
        }

        // GET: Demands/Edit/5
        [Authorize]
        public ActionResult Activate(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Demand demand = db.Demands.Find(id);

            if (!CheckEndDate(demand.EndDate))
            {
                TempData["Error"] = "Дата окончания приема предложений должна быть между " + DateTime.Today.ToString("dd.MM.yyyy") + " и " + DateTime.Today.AddDays(7).ToString("dd.MM.yyyy");
                //ModelState.AddModelError("EndDate", "Дата окончания приема предложений должна быть между " + DateTime.Today.ToString("dd.MM.yyyy") + " и " + DateTime.Today.AddDays(7).ToString("dd.MM.yyyy"));
                return RedirectToAction("Edit", new { id = demand.DemandId }); 
            }

            demand.DemandStatus = DemandStatus.Active;

            db.Entry(demand).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { getMine = true });
        }

        // POST: Demands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult 
            Edit([Bind(Include = "DemandId,PriceFrom,PriceTo,AreaFrom,AreaTo,Message,Phone,EndDate,CityId,HasWindow", Exclude = "UserId,TenantUser,PossibleAddress,CreateDate,Proposals")] Demand demand)
            
        {
            ViewBag.Cities = GetCities();
            if (ModelState.IsValid)
            {
                if (!CheckEndDate(demand.EndDate))
                {
                    ModelState.AddModelError("EndDate", "Дата окончания приема предложений должна быть между " + DateTime.Today.ToString("dd.MM.yyyy") + " и " + DateTime.Today.AddDays(7).ToString("dd.MM.yyyy"));
                    return View(demand);
                }

                var demandInDB = db.Demands.Find(demand.DemandId);
                demandInDB.PriceFrom = demand.PriceFrom;
                demandInDB.PriceTo = demand.PriceTo;
                demandInDB.AreaFrom = demand.AreaFrom;
                demandInDB.AreaTo = demand.AreaTo;
                demandInDB.Message = demand.Message;
                demandInDB.Phone = demand.Phone;
                demandInDB.EndDate = demand.EndDate;
                demandInDB.CityId = demand.CityId;
                demandInDB.HasWindow = demand.HasWindow;

                var user = UserManager.FindById(User.Identity.GetUserId());
                //if (user != null)
                //{
                //    demand.UserId = user.Id;
                //}

                if (user.Id != demandInDB.UserId
                    || (demandInDB.DemandStatus != DemandStatus.Created)
                    )
                {
                    return RedirectToAction("Index", new { getMine = true });
                }

                db.Entry(demandInDB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new {getMine=true });
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(demand);
        }

        // GET: Demands/Delete/5
        [Authorize]
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
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id != demand.UserId && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Demands");
            }
            return View(demand);
        }

        // POST: Demands/Delete/5
        [Authorize] 
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Demand demand = db.Demands.Find(id);

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user.Id != demand.UserId && !User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Demands");
            }

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
