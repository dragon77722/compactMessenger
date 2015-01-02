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

namespace Messenger.Controllers
{
    [Authorize]
    public class ConversationsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private ApplicationUserManager _userManager;

        public ConversationsController()
        {
        }

        public ConversationsController(ApplicationUserManager userManager)
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
        /// Get list of conversations of current user.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Conversation> GetConversations()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            return db.Conversations.Where(conv => (conv.UserAId == user.Id) || (conv.UserBId == user.Id));
        }

        /// <summary>
        /// Get a conversation of current user.
        /// </summary>
        /// <param name="id">The ID of the conversation</param>
        /// <returns></returns>
        [ResponseType(typeof(Conversation))]
        public IHttpActionResult GetConversation(int id)
        {
            Conversation conversation = db.Conversations.Find(id);
            if (conversation == null)
            {
                return NotFound();
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
            if ((conversation.UserBId != user.Id) && (conversation.UserAId != user.Id))
            {
                return NotFound();
            }

            return Ok(conversation);
        }

        /// <summary>
        /// Create new conversation with specified user.
        /// </summary>
        /// <param name="conversation">The conversation data</param>
        /// <returns></returns>
        [ResponseType(typeof(Conversation))]
        public IHttpActionResult PostConversation(Conversation conversation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = UserManager.FindById(User.Identity.GetUserId());
            conversation.UserAId = user.Id;
            db.Conversations.Add(conversation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = conversation.Id }, conversation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConversationExists(int id)
        {
            return db.Conversations.Count(e => e.Id == id) > 0;
        }
    }
}