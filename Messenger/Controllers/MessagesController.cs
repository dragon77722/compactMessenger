using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Messenger.Models;
using System.Web;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Http.ModelBinding;

namespace Messenger.Controllers
{
    [Authorize]
    public class MessagesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public MessagesController()
        {
        }

        public MessagesController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Get conversation (list of messages) with specified user
        /// </summary>
        /// <param name="id">The ID of the user</param>
        /// <returns></returns>
        public IQueryable<Message> GetMessage(string id)
        {
            var uid = User.Identity.GetUserId();
            return db.Messages.Where(msg => ((msg.FromId == uid) && (msg.ToId == id)) || ((msg.FromId == id) && (msg.ToId == uid)));
        }

        /// <summary>
        /// Post a new message to given conversation.
        /// </summary>
        /// <param name="message">The message to post</param>
        /// <returns></returns>
        [ResponseType(typeof(Message))]
        public IHttpActionResult PostMessage(Message message)
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            message.FromId = user.Id;
            db.Messages.Add(message);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = message.Id }, message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MessageExists(int id)
        {
            return db.Messages.Count(e => e.Id == id) > 0;
        }
    }
}