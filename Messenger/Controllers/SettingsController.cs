using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Messenger.Models;

namespace Messenger.Controllers
{
    public class SettingsController : Controller
    {

        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Settings
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
        [HttpPost]
        [Authorize]
        public ActionResult Edit(bool FriendOnly)
        {
            ApplicationUser cur = new ApplicationUser();
            foreach (var curUser in db.Users)
                if (User.Identity.Name == curUser.Email)
                    cur = curUser;
            cur.friendMessageOnly = FriendOnly;
            db.SaveChanges();
            ViewBag.MyUser = cur;
            return View();
        }
    }
    
}