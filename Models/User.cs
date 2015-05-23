﻿namespace Models
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
        private const int StartPoints = 1000;

        private ICollection<WordsUsers> wordsUsers;
        private ICollection<BoardsUsers> boardsUsers;

        public User()
        {
            this.RegisteredOn = DateTime.Now;
            this.EarnedPoints = 0;
            this.wordsUsers = new HashSet<WordsUsers>();
            this.boardsUsers = new HashSet<BoardsUsers>();
            this.Balance = StartPoints;
        }

        public long EarnedPoints { get; set; }

        public int Balance { get; set; }

        public DateTime RegisteredOn { get; set; }

        public virtual ICollection<WordsUsers> WordsUsers
        {
            get { return this.wordsUsers; }
        }

        public virtual ICollection<BoardsUsers> BoardsUsers
        {
            get { return this.boardsUsers; }
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
