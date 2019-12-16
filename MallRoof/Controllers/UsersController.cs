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
namespace MallRoof.Controllers
{
    public class UsersController : Controller
    {
        private MallContext db = new MallContext();
        private ApplicationUserManager _userManager;
        private ApplicationRoleManager _roleManager;

        public UsersController()
        {
            var userStore = new UserStore<User>(db);
            //_userManager = new ApplicationUserManager(userStore);
            _userManager = ApplicationUserManager.CreateDefault(db);
            var roleStore = new RoleStore<IdentityRole>(db);
            _roleManager = new ApplicationRoleManager(roleStore);

        }
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();}
            private set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();}
            private set { _roleManager = value; }
        }

        // GET: Users
        public ActionResult Index()
        {
            return View(db.IdentityUsers.ToList());
        }

        public ActionResult RoleIndex()
        {
            return View(RoleManager.Roles);            
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.IdentityUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FirstName,SurName,Phone")] User user)
        public ActionResult Create([Bind(Include = "Email,FirstName,SurName,Phone")] User user, string password)
        {
            
            if (ModelState.IsValid)
            {
                //var userStore = new UserStore<User>(db);
                //var userManager  = new ApplicationUserManager(userStore);
                //var result = userManager.Create(user, password);
                user.UserName = user.Email;
                var result = this.UserManager.Create(user, password);
                

                //var adminRole = db.Roles.Where(r => r.Name == "Admin").FirstOrDefault();
                var enteredUser =  UserManager.FindByEmail(user.Email);
                UserManager.AddToRole(enteredUser.Id.ToString(), "Landlord");

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.IdentityUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            
            UserRoleEdit userRoleEdit = new UserRoleEdit();
            userRoleEdit.User = user;
            var userRoles = user.Roles.Select(r => RoleManager.FindById(r.RoleId));
            userRoleEdit.UserRoles = userRoles;
            //var allRoles = RoleManager.Roles.Where(r=>userRoles.Select(ur=>ur.Id).Contains( r.Id));
            //System.Collections.Generic.List < IdentityRole > allRoles = new List<IdentityRole>();

            List<IdentityRole> allRoles = new List<IdentityRole>(db.Roles.ToList());

            allRoles  = allRoles.Except(userRoles).ToList();

            //var allRolesF =
            //    from ur in userRoles
            //    join r in allRoles on ur.Id equals r.Id
            //    where ur.Id != r.Id
            //    select r;

            //allRoles2 = allRoles2.Where(r => userRoles.Select(ur => ur.Name).Contains(r.Name)).ToList();


            userRoleEdit.AllRoles = allRoles;
            

            return View(userRoleEdit);
        }

        public ActionResult AddRoleToUser(Guid UserId, Guid RoleId)
        {
            User user = UserManager.FindById(UserId.ToString());
            IdentityRole role = RoleManager.FindById(RoleId.ToString());
            var result = UserManager.AddToRole(UserId.ToString(), role.Name);
            if (result.Succeeded)
            {
                return RedirectToAction("Edit", new { id = UserId });
            }
            else
            {
                return RedirectToAction("Edit", new { id = UserId });
            }
        }

        public void AddRoleToUser2(Guid UserId, Guid RoleId)
        {
            User user = UserManager.FindById(UserId.ToString());
            IdentityRole role = RoleManager.FindById(RoleId.ToString());
            var result = UserManager.AddToRole(UserId.ToString(), role.Name);
        }


        public ActionResult DeleteRoleFromUser(Guid UserId, Guid RoleId)
        {
            User user = UserManager.FindById(UserId.ToString());
            IdentityRole role = RoleManager.FindById(RoleId.ToString());
            var result = UserManager.RemoveFromRole(UserId.ToString(), role.Name);

            if (result.Succeeded)
            {
                return RedirectToAction("Edit", new { id = UserId });
            }
            else
            {
                return RedirectToAction("Edit", new { id = UserId });
            }
        }


        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            //[Bind(Include = "Id,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,FirstName,SurName,Phone")]
            [Bind(Include = "Id,Email,FirstName,SurName,Phone")]
        User user)
        {
            if (ModelState.IsValid)
            {
                user.UserName = user.Email;
                var selectedUser = this.UserManager.FindById(user.Id);
                selectedUser.FirstName = user.FirstName;
                selectedUser.SurName = user.SurName;
                selectedUser.Phone = user.Phone;
                
                db.Entry(selectedUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.IdentityUsers.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            User user = db.IdentityUsers.Find(id);
            db.Users.Remove(user);
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

        public ActionResult CreateRole()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CreateRole([Bind(Include = "Name")] IdentityRole role)
        {

            if (ModelState.IsValid)
            {
                var result = RoleManager.Create(new IdentityRole() {Name = role.Name });

                db.SaveChanges();
                return RedirectToAction("RoleIndex");
            }
            return View(role);
        }

        public ActionResult EditRole(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IdentityRole role = RoleManager.FindById(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditRole([Bind(Include = "Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                RoleManager.Update(role);
                db.SaveChanges();
                return RedirectToAction("RoleIndex");
            }
            return View(role);
        }

        public ActionResult RoleDelete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            IdentityRole role = RoleManager.FindById(id);

            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }

        // POST: Users/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RoleDeleteConfirmed(string id)
        {
            IdentityRole role = RoleManager.FindById(id);
            RoleManager.Delete(role);            
            db.SaveChanges();
            return RedirectToAction("RoleIndex");
        }

    }

    

}

