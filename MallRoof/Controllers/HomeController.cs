using MallRoof.DAL;
using MallRoof.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MallRoof.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        MallContext context = new MallContext();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase upload, string PremiseId)
        {
            if (upload != null)
            {
                Guid premiseId = Guid.Parse(PremiseId);
                var premise = context.Premises.SingleOrDefault(p => p.PremiseId == premiseId);
                int photosCount = premise.Photos.Count;
                Photo photo = new Photo();                
                //context.Photos.Add();
                // получаем имя файла
                string fileExtension = System.IO.Path.GetExtension(upload.FileName);
                string fileName = ("/Files/" + PremiseId + "_" + (photosCount+1).ToString()) + fileExtension;
                // сохраняем файл в папку Files в проекте
                upload.SaveAs(fileName);
                photo.Path = fileName;
                photo.Premise = premise;
                context.Photos.Add(photo);
                context.SaveChanges();
            }
            return RedirectToAction("Edit","Premises", new { id = PremiseId });
        }
    }
}