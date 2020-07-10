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
using System.IO;

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
        //[Authorize]
        public ActionResult Index(string getMine, int? page, string forAdmin, string premiseCreate)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            var malls = db.Malls.OrderBy(m=>m.Name).AsQueryable();

            int pageSize = 10;
            string pSize = ConfigurationManager.AppSettings["pageSize"];
            if (!string.IsNullOrEmpty(pSize))
            {
                int.TryParse(pSize, out pageSize);
            }
            int pageNumber = (page ?? 1);

            if (user != null && !string.IsNullOrEmpty(premiseCreate) && bool.TrueString == premiseCreate)
            {
                malls = malls.Where(m => m.UserId == user.Id);
                return View("IndexPremiseCreate", malls);
            }

            if ( user!=null &&  !string.IsNullOrEmpty(getMine) &&  bool.TrueString == getMine)
            {
                malls = malls.Where(m => m.UserId == user.Id);
                return View("IndexLandlord", malls.ToPagedList(pageNumber, pageSize));
            }

            if (User.IsInRole("Admin"))
            {                
                return View("IndexLandlord", malls.ToPagedList(pageNumber, pageSize));
            }

            return View(malls.ToPagedList(pageNumber, pageSize));
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
            ViewBag.Cities =  GetCities();
            return View();
        }
        

        // POST: Malls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MallId,Name,Address,UserId,NumberOfFloors,CityId," +
            "ParkingExists,ParkingInsideExists,ParkingPayment,ParkingInsidePayment")] Mall mall)
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
                return RedirectToAction("Index", "Malls");
            }
            var errors = ModelState.Where(x => x.Value.Errors.Any())
                .Select(x => new { x.Key, x.Value.Errors });

            ViewBag.Cities = db.Cities.OrderBy(c => c.Name);
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
            ViewBag.Cities = GetCities();
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null && !User.IsInRole("Admin")
                )
            {
                if (mall.UserId != User.Identity.GetUserId())
                    return RedirectToAction("Index", "Malls");
            }

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
        public ActionResult Edit([Bind(Include = "MallId,Name,Address,UserId,NumberOfFloors,ParkingExists,ParkingInsideExists,ParkingPayment,ParkingInsidePayment,CityId,PhoneNumber")] Mall mall)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(mall).State = EntityState.Detached;
                var mallinDB = db.Malls.Find(mall.MallId);
                //mall.UserId = mallinDB.UserId;
                
                mallinDB.Name = mall.Name;
                mallinDB.Address = mall.Address;
                mallinDB.NumberOfFloors = mall.NumberOfFloors;
                mallinDB.ParkingExists = mall.ParkingExists;
                mallinDB.ParkingInsideExists= mall.ParkingInsideExists;
                mallinDB.ParkingPayment = mall.ParkingPayment;
                mallinDB.ParkingInsidePayment = mall.ParkingInsidePayment;
                mallinDB.CityId = mall.CityId;
                mallinDB.PhoneNumber = mall.PhoneNumber;

                db.Entry(mallinDB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { getMine = true });
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
            string fileName = "";
            Mall mall = db.Malls.Find(id);

            try
            {
                foreach (var premise in mall.Premises)
                {
                    foreach (var photo in premise.Photos)
                    {
                        fileName = ControllerContext.HttpContext.Server.MapPath(photo.Path);
                        if (System.IO.File.Exists(fileName))
                        {
                            System.IO.File.Delete(fileName);
                        }
                    }
                }
                db.Malls.Remove(mall);
                db.SaveChanges();
            }
            catch
            {
            }
            


            return RedirectToAction("Index", "Malls");
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, string MallId)
        {
            if (upload != null)
            {
                bool isValid = false;
                // Settings.  
                int allowedFileSize = Int32.Parse(ConfigurationManager.AppSettings["maxAllowedContentLength"]);
                int allowedPhotoCount = Int32.Parse(ConfigurationManager.AppSettings["mallPhotoCount"]);

                // Initialization.  
                var fileSize = upload.ContentLength;

                // Settings.  
                isValid = fileSize <= allowedFileSize;

                if (isValid)
                {
                    string folderName = "MallImages";                    
                    string path = ControllerContext.HttpContext.Server.MapPath("~/" + folderName);
                    //string path = Server.MapPath("~/"+ folderName);
                    
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Guid mallId = Guid.Parse(MallId);
                    var mall = db.Malls.SingleOrDefault(p => p.MallId == mallId);
                    int photosCount = mall.MallPhotos.Count;
                    if (photosCount >= allowedPhotoCount)
                    {
                        ViewBag.Error = "Количество фотографий должно быть не более " + allowedPhotoCount.ToString();
                        return RedirectToAction("Edit", "Malls", new { id = MallId });
                    }
                    MallPhoto photo = new MallPhoto();
                    //context.Photos.Add();
                    // получаем имя файла
                    string fileExtension = System.IO.Path.GetExtension(upload.FileName);
                    string fileName = ("/"+folderName+"/" + MallId + "_" + (photosCount + 1).ToString()) + fileExtension;
                    // сохраняем файл в папку Files в проекте
                    //fileName = ControllerContext.HttpContext.Server.MapPath(fileName);
                    upload.SaveAs(ControllerContext.HttpContext.Server.MapPath(fileName));
                    fileName = fileName.Replace("/", "\\");
                    photo.Path = fileName;
                    photo.Mall = mall;
                    db.MallPhotos.Add(photo);
                    db.SaveChanges();
                }
                else
                {
                    var fsinMegaByte = (allowedFileSize / 1024f) / 1024f;
                    ViewBag.Error = "Загружаемая фотография должна быть меньше " + fsinMegaByte.ToString() + "Мб";
                    return View("Error");
                }
            }
            return RedirectToAction("Edit", "Malls", new { id = MallId});
        }

        public List<City> GetCities()
        {
            var cities = db.Cities.OrderBy(c => c.Name).ToList();
            var nur = cities.FirstOrDefault(c => c.Name.Contains("Нур-Султан"));
            cities.Remove(nur);
            cities.Insert(0, nur);
            return cities;
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
