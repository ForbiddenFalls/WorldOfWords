namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using EntityFramework.Extensions;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(AppDbContext context)
        {
            context.Boards.Delete();
            var boardSofia = new Board()
            {
                Name = "Sofia",
                Size = 5,
                Content = "{\"content\":\"���� �    �    �    �    \",\"words\":[\"����\",\"�����\"]}",
                ExpairyTime = DateTime.Now,
            };

            var boardPlovdiv = new Board()
            {
                Name = "Plovdiv",
                Size = 5,
                Content = "{\"content\":\" ���� �  � �  � �    �   \",\"words\":[\"�����\",\"����\",\"���\"]}",
                ExpairyTime = DateTime.Now,
            };

            context.Boards.Add(boardSofia);
            context.Boards.Add(boardPlovdiv);
            context.SaveChanges();
        }
    }
}
