using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using Messenger.Models;
using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Concurrent;

namespace Messenger.Controllers
{
    public class DeleteController : Controller
    {
        const string adminEmail =  "admin@admin.ru";
        // GET: Delete
        [Authorize]
        public ActionResult Index()
        {
            if (string.Compare(User.Identity.Name, adminEmail) != 0)
                return RedirectToAction("Index", "Home");
            ApplicationDbContext db = new ApplicationDbContext();
            ViewBag.Users = db.Users;
            return View();
        }
        [Authorize]
        public ActionResult DeleteUser(string id)
        {
            if (string.Compare(User.Identity.Name, adminEmail) != 0)
                return RedirectToAction("Index", "Home");
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser cur = db.Users.Find(id);
            List<requestPair> deleteRequests = new List<requestPair>();
            string curUserId = User.Identity.GetUserId();
            foreach (requestPair curP in db.requests.Where(u => string.Compare(u.from, id) == 0 || string.Compare(u.to, id) == 0).ToList())
                deleteRequests.Add(curP);
            for (int i = 0; i < deleteRequests.Count; i++)
                db.requests.Remove(deleteRequests[i]);
            List<friendPair> deleteFriends = new System.Collections.Generic.List<friendPair>();
            foreach (friendPair curP in db.friends.Where(u => string.Compare(u.friend1, id) == 0 || string.Compare(u.friend2, id) == 0).ToList())
                deleteFriends.Add(curP);
            for (int i = 0; i < deleteFriends.Count; i++)
                db.friends.Remove(deleteFriends[i]);
            db.Users.Remove(cur);
            db.SaveChanges();
            return RedirectToAction("Index", "Delete");
        }
    }
}