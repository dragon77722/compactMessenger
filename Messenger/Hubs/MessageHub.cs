using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Collections.Concurrent;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Messenger.Models;
using Messenger.Controllers;

namespace Messenger.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        private static readonly ConcurrentDictionary<string, List<string>> _userConnections = new ConcurrentDictionary<string, List<string>>();

        private ApplicationDbContext db = new ApplicationDbContext();
        
        public void Send(string userBId, string message)
        {
                var userAName = Context.User.Identity.Name;
                var mgr = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            try
            {
                var userAId = mgr.FindByEmail(userAName).Id;
                
                ApplicationUser _BUser = mgr.FindById(userBId);
                var donotSend = _BUser.friendMessageOnly; //&& !_BUser.FriendBase.Contains(userAId);

                ApplicationUser curUser = db.Users.Where(u => u.UserName == Context.User.Identity.Name).FirstOrDefault();
                string curUserId = curUser.Id;

                string currentUserId = Context.User.Identity.GetUserId();
                ApplicationUser cur = db.Users.FirstOrDefault(x => x.Id == currentUserId);
                List<ApplicationUser> friendsList = new List<ApplicationUser>();
                foreach (friendPair ur in db.friends.Where(u => u.friend1 == currentUserId || u.friend2 == currentUserId).ToList())
                {
                    if (ur.friend1 == currentUserId)
                        friendsList.Add(db.Users.Find(ur.friend2));
                    else if (ur.friend2 == currentUserId)
                        friendsList.Add(db.Users.Find(ur.friend1));
                }
                if (friendsList.Contains(_BUser))
                    donotSend = false;

                if (donotSend)
                {
                    Message msg1 = new Message() { Text = "You cannot send messages to this user due to their privacy settings.", FromId = userBId, ToId = userAId };
                    msg1 = db.Messages.Add(msg1);
                    List<string> connectionsP;
                    if (_userConnections.TryGetValue(userAName, out connectionsP))
                    {

                        Clients.Clients(connectionsP).addMessage(msg1);
                    }
                    throw new Exception();
                }
                Message msg4 = new Message() { Text = message, FromId = userAId, ToId = userBId };
                db.Messages.Add(msg4);

                db.SaveChanges();

                var userBName = mgr.FindById(userBId).Email;

                List<string> connections;
                if (_userConnections.TryGetValue(userAName, out connections))
                {

                    Clients.Clients(connections).addMessage(msg4);
                }
               if(string.Compare(userAName,userBName) != 0)
                    if (_userConnections.TryGetValue(userBName, out connections))
                    {
                    Clients.Clients(connections).addMessage(msg4);
                    }
            }
            catch(Exception)
            {
                Message msg = new Message() { Text = "Этому пользователю отправлять сообщения могут только друзья", FromId = mgr.FindByEmail(userAName).Id, ToId = mgr.FindByEmail(userAName).Id };
                msg = db.Messages.Add(msg);
            }
        }

        public override Task OnConnected()
        {
            var connections = _userConnections.GetOrAdd(Context.User.Identity.Name, _ => new List<string>());
            lock (connections)
            {
                connections.Add(Context.ConnectionId);
            }
            return base.OnConnected();
        }
        
        public override Task OnDisconnected(bool stopCalled)
        {
            List<string> connections;
            if (_userConnections.TryGetValue(Context.User.Identity.Name, out connections))
            {
                lock (connections)
                {
                    connections.Remove(Context.ConnectionId);
                }
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}