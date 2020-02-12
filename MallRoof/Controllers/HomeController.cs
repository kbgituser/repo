using MallRoof.DAL;
using MallRoof.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace MallRoof.Controllers
{
    
    public class HomeController : Controller
    {
        MallContext context = new MallContext();

        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public HomeController()
        {
            var userStore = new UserStore<User>(context);
            _userManager = ApplicationUserManager.CreateDefault(context);
            var roleStore = new RoleStore<IdentityRole>(context);
            _roleManager = new ApplicationRoleManager(roleStore);

        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            string content = "Проект является бесплатной площадкой для бизнес и торговых центров, с помощью которой вы можете:"
                + "вести реестр, учет своих помещений"
                + "выставлять на показ / скрывать информацию о помещении для соискателей"
                + "размещать детальную информацию о центре и помещениях";
            ViewBag.Message = "";
            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";
            return View();
        }

        public ActionResult Instruction()
        {
            //ViewBag.Message = "Your contact page.";
            return View();
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
                    var premise = context.Premises.SingleOrDefault(p => p.PremiseId == premiseId);
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
                    context.Photos.Add(photo);
                    context.SaveChanges();
                }
                else
                {
                    var fsinMegaByte = (allowedFileSize / 1024f) / 1024f;
                    ViewBag.Error = "Загружаемая фотография должна быть меньше " + fsinMegaByte.ToString() + "Мб";
                    return View("Error");
                }
            }
            return RedirectToAction("Edit","Premises", new { id = PremiseId });
        }

        public List<string> GetMenu() {
            List<string> menu = new List<string>();
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null )
            {
            }
            var roles = context.Roles;
            foreach (var role in roles)
            {
                if (User.IsInRole(role.Name))
                {

                }
            }           

            return menu;
        }

    }
}