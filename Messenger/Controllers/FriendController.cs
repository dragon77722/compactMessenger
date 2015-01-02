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
    public class FriendController : Controller
    {
        // GET: Friend
        [Authorize]
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            string currentUserName = User.Identity.GetUserName();
            string curUserId = User.Identity.GetUserId();
            ApplicationUser cur = db.Users.FirstOrDefault(x => x.UserName == currentUserName);
            List<ApplicationUser> friends = new List<ApplicationUser>();
            foreach(friendPair curP in db.friends.Where(u => string.Compare(u.friend1, curUserId) == 0 || string.Compare(u.friend2, curUserId) == 0).ToList())
            {
                if(!((string.Compare(curP.friend1,curUserId) == 0) && (string.Compare(curP.friend2,curUserId) == 0)))
                {
                    if (string.Compare(curP.friend1, curUserId) != 0)
                    {
                        friends.Add(db.Users.Find(curP.friend1));
                        continue;
                    }
                    friends.Add(db.Users.Find(curP.friend2));
                }
            }
            List<ApplicationUser> requestsFromThisUser = new List<ApplicationUser>();
            foreach (requestPair curP in db.requests.Where(u => string.Compare(u.from, curUserId) == 0).ToList())
            {
                if(curP.to != null)
                requestsFromThisUser.Add(db.Users.Find(curP.to));
            }
            List<ApplicationUser> requestsToThisUser = new List<ApplicationUser>();
            foreach (requestPair curP in db.requests.Where(u => string.Compare(u.to, curUserId) == 0).ToList())
            {
                if (curP.from != null)
                requestsToThisUser.Add(db.Users.Find(curP.from));
            }
            ViewBag.Friends = friends;
            ViewBag.RequestsFromThisUser = requestsFromThisUser;
            ViewBag.RequestsToThisUser = requestsToThisUser;
            return View();
        }
        [Authorize]
        public ActionResult AddRequest(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            bool isThereRequest = false;
            bool isFriend = false;
            string curUserId = User.Identity.GetUserId();// возможно, будут проблемы с типом id
            if (db.requests.Count(u => (string.Compare(u.from, curUserId) == 0) && (string.Compare(u.to, id) == 0)) != 0)
            {
                isThereRequest = true;
            }
            if (db.friends.Count(u => (((string.Compare(u.friend1, id) == 0) && (string.Compare(u.friend2, curUserId) == 0)) || (((string.Compare(u.friend2, id) == 0)) && (string.Compare(u.friend1, curUserId) == 0)))) > 0)
            {
                isFriend = true;
            }
            if(!isThereRequest && !isFriend)
            {
                requestPair ur = new requestPair(curUserId, id);
                db.requests.Add(ur);
                db.SaveChanges();
            }
            ViewBag.isFriend = isFriend;
            ViewBag.isThereRequest = isThereRequest;
            return View();
        }
        [Authorize]
        public ActionResult RemoveFriend(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<friendPair> deleteFriend = new List<friendPair>();
            string curUserId = User.Identity.GetUserId();
            foreach(friendPair curP in db.friends.Where(u => ((string.Compare(u.friend1, id) == 0) && (string.Compare(u.friend2, curUserId) == 0))  || ((string.Compare(u.friend2, id) == 0) && (string.Compare(u.friend1, curUserId) == 0))).ToList())
            {
                deleteFriend.Add(curP);
            }
            for(int i = 0; i < deleteFriend.Count; i++)
            {
                db.friends.Remove(deleteFriend[i]);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Friend");
        }
        [Authorize]
        public ActionResult CancelRequestToThisUser(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<requestPair> deleteRequestTo = new List<requestPair>();
            string curUserId = User.Identity.GetUserId();
            foreach (requestPair curP in db.requests.Where(u => string.Compare(u.from,id) == 0 && string.Compare(u.to, curUserId) == 0).ToList())
            {
                deleteRequestTo.Add(curP);
            }
            db.requests.Remove(deleteRequestTo[0]);
            db.SaveChanges();
            return RedirectToAction("Index", "Friend");
        }
        [Authorize]
        public ActionResult CancelRequestFromThisUser(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<requestPair> deleteRequestFrom = new List<requestPair>();
            string curUserId = User.Identity.GetUserId();
            foreach (requestPair curP in db.requests.Where(u => string.Compare(u.from, curUserId) == 0 && string.Compare(u.to, id) == 0).ToList())
            {
                deleteRequestFrom.Add(curP);
            }
            db.requests.Remove(deleteRequestFrom[0]);
            db.SaveChanges();
            return RedirectToAction("Index", "Friend");
        }
        [Authorize]
        public ActionResult ConfirmRequest(string id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<requestPair> deleteRequests = new List<requestPair>();
            string curUserId = User.Identity.GetUserId();
            foreach (requestPair curP in db.requests.Where(u => (string.Compare(u.from, curUserId) == 0 && string.Compare(u.to, id) == 0) || (string.Compare(u.from, id) == 0 && string.Compare(u.to, curUserId) == 0)).ToList())
            {
                deleteRequests.Add(curP);
            }
            for (int i = 0; i < deleteRequests.Count; i++)
            {
                db.requests.Remove(deleteRequests[i]);
            }
            friendPair buf = new friendPair(curUserId, id);
            db.friends.Add(buf);
            db.SaveChanges();
            return RedirectToAction("Index", "Friend");
        }
    }
}