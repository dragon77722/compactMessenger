using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Messenger.Models;

namespace Messenger.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Search
        public ActionResult Result(string Email, string RealName, string Surname, string Age)
        {
            List<ApplicationUser> Result = new List<ApplicationUser>();
            foreach (var curUser in db.Users)
            {
                bool check = true;
                if (string.Compare(curUser.Email, User.Identity.Name) == 0)
                    check = false;
                if (Email != String.Empty && string.Compare(curUser.Email, Email) != 0)
                    check = false; 
                if (RealName != String.Empty && string.Compare(curUser.Realname, RealName) != 0)
                    check = false;
                if (Surname != String.Empty && string.Compare(curUser.Surname, Surname) != 0)
                    check = false;
                if (Age != String.Empty && string.Compare(curUser.Age, Age) != 0)
                    check = false;
                if (check)
                    Result.Add(curUser);
            }
            ViewBag.SearchResult = Result;
            ViewBag.DB = db;
            return View();
        }
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}