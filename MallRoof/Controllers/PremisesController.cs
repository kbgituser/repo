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
using PagedList.Mvc;
using PagedList;
using System.Configuration;
using System.IO;

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
        public ActionResult Index(string mallId, string price, string area, string haswindow, string priceorder, string order, string getMine
            ,int? page
            )
        {            
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                ViewBag.Malls = user.Malls;
                ViewBag.User = user;
            }
            PremisesMallListModel premisesMallListModel = new PremisesMallListModel();

            var premises = db.Premises.AsQueryable();
            int priceint;
            if (Int32.TryParse(price, out priceint))
            {
                premises = premises.Where(p => p.Price <= priceint);
                premisesMallListModel.Price = price;
            }
            

            Guid mallIdg;
            if (!string.IsNullOrEmpty(mallId) && Guid.TryParse(mallId, out mallIdg))
            {
                premises = premises.Where(p => p.Mall.MallId == mallIdg);
                premisesMallListModel.MallId = mallId;
            }

            int areaint;
            if (Int32.TryParse(area, out areaint))
            {
                premises = premises.Where(p => p.Area <= areaint);
                premisesMallListModel.Area = area;
            }

            if (!string.IsNullOrEmpty(haswindow))
            {
                var haswindowb = bool.Parse(haswindow);
                if (haswindowb)
                {
                    premises = premises.Where(p => p.HasWindow == haswindowb);
                    premisesMallListModel.Haswindow = haswindowb.ToString();
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

            int pageSize = 10;
            string pSize = ConfigurationManager.AppSettings["pageSize"];
            if (!string.IsNullOrEmpty(pSize))
            {
                int.TryParse(pSize, out pageSize);
            }            
            int pageNumber = (page ?? 1);

            ViewBag.PriceSortParam = order == "price" ? "price_desc" : "price";
            ViewBag.AreaSortParam = order == "area" ? "area_desc" : "area";
            

            if (user != null && !string.IsNullOrEmpty(getMine) && bool.TrueString == getMine)
            {
                premisesMallListModel.Malls = user.Malls.ToList();
                //malls = malls.Where(m => m.UserId == user.Id);
                premises = premises.Where(p => p.Mall.UserId == user.Id);                

                premisesMallListModel.Premises = premises.ToPagedList(pageNumber, pageSize);
                return View("IndexLandlord", premisesMallListModel);
            }
            premisesMallListModel.Malls = db.Malls.ToList();
            premisesMallListModel.Premises = premises.ToPagedList(pageNumber, pageSize);
            return View(premisesMallListModel);


            //return View(db.Premises.ToList());
        }

        // index only for landlords
        //public ActionResult IndexLandlord(string mallId, string price, string area, string haswindow, string priceorder, string order)
        //{
        //    var user = UserManager.FindById(User.Identity.GetUserId());
        //    if (user != null)
        //    {
        //        ViewBag.Malls = user.Malls;
        //        ViewBag.User = user;
        //    }
        //    PremisesMallListModel premisesMallListModel = new PremisesMallListModel();

        //    var premises = db.Premises.AsQueryable();
        //    int priceint;
        //    if (Int32.TryParse(price, out priceint))
        //    {
        //        premises = premises.Where(p => p.Price <= priceint);
        //    }


        //    Guid mallIdg;
        //    if (!string.IsNullOrEmpty(mallId) && Guid.TryParse(mallId, out mallIdg))
        //    {
        //        premises = premises.Where(p => p.Mall.MallId == mallIdg);
        //    }

        //    int areaint;
        //    if (Int32.TryParse(area, out areaint))
        //    {
        //        premises = premises.Where(p => p.Area <= areaint);
        //    }

        //    if (!string.IsNullOrEmpty(haswindow))
        //    {
        //        var haswindowb = bool.Parse(haswindow);
        //        if (haswindowb)
        //        {
        //            premises = premises.Where(p => p.HasWindow == haswindowb);
        //        }

        //    }

        //    switch (order)
        //    {
        //        case "price":
        //            premises = premises.OrderBy(s => s.Price);
        //            break;
        //        case "price_desc":
        //            premises = premises.OrderByDescending(s => s.Price);
        //            break;
        //        case "area":
        //            premises = premises.OrderBy(s => s.Area);
        //            break;
        //        case "area_desc":
        //            premises = premises.OrderByDescending(s => s.Area);
        //            break;
        //        default:
        //            premises = premises.OrderBy(s => s.Price);
        //            break;
        //    }


        //    ViewBag.PriceSortParam = order == "price" ? "price_desc" : "price";
        //    ViewBag.AreaSortParam = order == "area" ? "area_desc" : "area";
        //    premisesMallListModel.Malls = db.Malls.ToList();

        //    premisesMallListModel.Premises = premises.ToPagedList(pageNumber, pageSize);
        //    return View(premisesMallListModel);
        //    //return View(db.Premises.ToList());
        //}

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

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                if (premise.Mall.UserId != user.Id)
                    return RedirectToAction("Index");
            }
            return View(premise);
        }

        // POST: Premises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PremiseId,Number,Floor,Area,IsLastFloor,HasWindow,Description,Price,IsSeen,MallId")] Premise premise)
        {

            if (ModelState.IsValid)
            {
                db.Entry(premise).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index" , new { getMine = true});
            }
            return View(premise);
        }

        // GET: Premises/Delete/5
        [Authorize]
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

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                if (premise.Mall.UserId != user.Id)
                    return RedirectToAction("Index");
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

        [Authorize]
        public ActionResult DeletePhoto(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photos.Find(id);
            
            if (photo == null)
            {
                return HttpNotFound();
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                if (photo.Premise.Mall.UserId != user.Id)
                    return RedirectToAction("Index");
            }
            string premiseId = photo.PremiseId.ToString();

            try
            {
                string fileName = ControllerContext.HttpContext.Server.MapPath(photo.Path);
                if (System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }

                db.Photos.Remove(photo);
                db.SaveChanges();

            }
            catch
            {
            }
            

            return RedirectToAction("Edit", "Premises", new { id = premiseId });
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, string PremiseId)
        {
            if (upload != null)
            {


                bool isValid = false;
                // Settings.  
                int allowedFileSize = Int32.Parse(ConfigurationManager.AppSettings["maxAllowedContentLength"]);

                // Initialization.  
                var fileSize = upload.ContentLength;

                // Settings.  
                isValid = fileSize <= allowedFileSize;

                if (isValid)
                {
                    Guid premiseId = Guid.Parse(PremiseId);
                    var premise = db.Premises.SingleOrDefault(p => p.PremiseId == premiseId);
                    int photosCount = premise.Photos.Count;
                    Photo photo = new Photo();
                    //context.Photos.Add();
                    // получаем имя файла
                    string fileExtension = System.IO.Path.GetExtension(upload.FileName);
                    string fileName = ("/Files/" + PremiseId + "_" + (photosCount + 1).ToString()) + fileExtension;
                    // сохраняем файл в папку Files в проекте
                    //fileName = ControllerContext.HttpContext.Server.MapPath(fileName);
                    upload.SaveAs(ControllerContext.HttpContext.Server.MapPath(fileName));
                    fileName = fileName.Replace("/", "\\");
                    photo.Path = fileName;
                    photo.Premise = premise;
                    db.Photos.Add(photo);
                    db.SaveChanges();
                }
                else
                {
                    var fsinMegaByte = (allowedFileSize / 1024f) / 1024f;
                    ViewBag.Error = "Загружаемая фотография должна быть меньше " + fsinMegaByte.ToString() + "Мб";
                    return View("Error");
                }
            }
            return RedirectToAction("Edit", "Premises", new { id = PremiseId });
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
