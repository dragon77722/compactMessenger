using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Messenger.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public string ToId { get; set; }
        public string FromId { get; set; }
    }

    public class Conversation
    {
        public int Id { get; set; }
        public string UserAId { get; set; }
        public string UserBId { get; set; }
    }

    public class UserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}