using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Messenger.Models;

namespace Messenger.Controllers
{
    public class ProfileController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Profile
        [Authorize]
        public ActionResult Index()
        {
            ApplicationUser cur = new ApplicationUser();
            foreach (var curUser in db.Users)
                 if (User.Identity.Name == curUser.Email)
                     cur = curUser;
            ViewBag.MyUser = cur;
            return View();
        }
        [Authorize]
        public ActionResult EditBuf()
        {
            return View();
        }
        [Authorize]
        public ActionResult Edit(string RealName, string Surname, string Age)
        {
            ApplicationUser cur = new ApplicationUser();
            foreach (var curUser in db.Users)
                 if (User.Identity.Name == curUser.Email)
                     cur = curUser;
            if (RealName != null && RealName != String.Empty)
                cur.Realname = RealName;
            if (Surname != null && Surname != String.Empty)
                cur.Surname = Surname;
            if (Age != null && Age != String.Empty)
                cur.Age = Age;
            db.SaveChanges();
            return RedirectToAction("Index","Profile");
        }
    }
}