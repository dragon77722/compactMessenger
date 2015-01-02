using System.Data.Entity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Concurrent;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace Messenger.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            friendMessageOnly = false;
        }
        /*public ApplicationUser(string hometown)
        {
            Hometown = hometown;
            friendMessageOnly = true;
        }*/
        public string Realname { get; set; }
        public string Surname { get; set; }
        public string Age { get; set; }
        public string Hometown { get; set; }
        public bool friendMessageOnly { get; set; }

        //public List<string> requestsTo { get; set; } 

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public override string ToString()
        {
            string result = "";
            if (Surname != null)
            {
                result += Surname;
                result += " ";
            }
            if (Realname != null)
            {
                result += Realname;
                result += " ";
            }
            string dop = "(" + Email + ")";
            result += dop;
            return result;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            ApplicationUser p = obj as ApplicationUser;
            if ((System.Object)p == null)
                return false;
            int result = string.Compare(UserName, p.UserName);
            return result == 0;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);

        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<Messenger.Models.Conversation> Conversations { get; set; }

        public System.Data.Entity.DbSet<Messenger.Models.Message> Messages { get; set; }
        public System.Data.Entity.DbSet<requestPair> requests { get; set; }
        public System.Data.Entity.DbSet<friendPair> friends { get; set; }
    }
}
    