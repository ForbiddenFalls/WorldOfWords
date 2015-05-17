namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        private const long StartPoints = 1000;

        private ICollection<WordsUsers> words;
        private ICollection<Board> boards;

        public User()
        {
            this.RegisteredOn = DateTime.Now;
            this.words = new HashSet<WordsUsers>();
            this.boards = new HashSet<Board>();
        }

        [DefaultValue(0)]
        public long EarnedPoints { get; set; }

        [DefaultValue(StartPoints)]
        public int Balance { get; set; }

        public DateTime RegisteredOn { get; set; }

        public virtual ICollection<WordsUsers> Words
        {
            get { return this.words; }
        }

        public virtual ICollection<Board> Boards
        {
            get { return this.boards; }
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
