using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
namespace Messenger.Models
{
    public class requestPair
    {
        [Key]
        public string id { get; set; }
        public string from{ get; set;}
        public string to { get; set; }
        public requestPair()
        {

        }
        public requestPair(requestPair other)
        {
            id = string.Copy(other.id);
            from = string.Copy(other.from);
            to = string.Copy(other.to);
        }
        public requestPair(string From, string To)
        {
            from = From;
            to = To;
            id = from + to;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            requestPair p = obj as requestPair;
            if ((System.Object)p == null)
            {
                return false;
            }
            return from == p.from && to == p.to;
        }
    }
    public class friendPair
    {
        [Key]
        public string id { get; set; }
        public string friend1 { get; set; }
        public string friend2 { get; set; }
        public friendPair(string Friend1, string Friend2)
        {
            friend1 = Friend1;
            friend2 = Friend2;
            id = Friend1 + Friend2;
        }
        public friendPair()
        {

        }
        public friendPair(friendPair other)
        {
            id = string.Copy(other.id);
            friend1 = string.Copy(other.friend1);
            friend2 = string.Copy(other.friend2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            friendPair p = obj as friendPair;
            if ((System.Object)p == null)
            {
                return false;
            }
            return (friend1 == p.friend1 && friend2 == p.friend2) || (friend1 == p.friend2 && friend2 == p.friend1);
        }
    }
}
