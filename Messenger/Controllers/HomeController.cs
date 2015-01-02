using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Messenger.Models;
using System.Text;
using System.Data;
using Microsoft.AspNet.Identity;
using Owin;
using Messenger.Controllers;

namespace Messenger.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser curUser = db.Users.Where(u => string.Compare(u.UserName, User.Identity.Name) == 0).FirstOrDefault();
            string curUserId = curUser.Id;
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser cur = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            List<ApplicationUser> friendsList = new List<ApplicationUser>();
            foreach (friendPair ur in db.friends.Where(u => u.friend1 == currentUserId || u.friend2 == currentUserId).ToList())
            {
                if (ur.friend1 == currentUserId)
                    friendsList.Add(db.Users.Find(ur.friend2));
                else if (ur.friend2 == currentUserId)
                    friendsList.Add(db.Users.Find(ur.friend1));
            }
            if (!friendsList.Contains(curUser))
            {
                friendPair ut = new friendPair(curUserId, currentUserId);
                db.friends.Add(ut);
                db.SaveChanges();
            }
            ViewBag.DB = db;
            ViewBag.I = curUser;
            return View();
        }
    }
}
