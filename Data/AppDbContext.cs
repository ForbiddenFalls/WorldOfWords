namespace Data
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<Board> Boards { get; set; }

        public DbSet<Word> Words { get; set; }

        public DbSet<WordsUsers> WordsUsers { get; set; }

        public DbSet<LettersPoints> LettersPoints { get; set; }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<User>().HasMany(x => x.Words).WithMany(x => x.Users)
        //        .Map(m => m.ToTable("WordsUsers"));
        //}
    }
}
