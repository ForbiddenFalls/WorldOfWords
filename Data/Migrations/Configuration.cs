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
            var boardVarna = new Board()
            {
                Name = "Varna",
                Size = 5,
                Content = "{\"content\":\"     \",\"words\":[]}",
                ExpairyTime = DateTime.Now,
            };

            var boardSofia = new Board()
            {
                Name = "Sofia",
                Size = 5,
                Content = "{\"content\":\"куче о    т    к    а    \",\"words\":[\"куче\",\"котка\"]}",
                ExpairyTime = DateTime.Now,
            };

            var boardPlovdiv = new Board()
            {
                Name = "Plovdiv",
                Size = 5,
                Content = "{\"content\":\" куче о  х т  о к    а   \",\"words\":[\"котка\",\"куче\",\"ехо\"]}",
                ExpairyTime = DateTime.Now,
            };

            context.Boards.Add(boardSofia);
            context.Boards.Add(boardPlovdiv);
            context.Boards.Add(boardVarna);

            context.LettersPoints.Delete();
            context.LettersPoints.Add(new LettersPoints()
            {
                Language = "bg",
                Letters = "абвгдежзийклмнопрстуфхцчшщъью€",
                Points = "1,2,1,3,2,1,4,4,1,5,2,2,2,1,1,1,1,1,1,5,10,5,8,5,8,10,3,10,10,5"
            });

            context.SaveChanges();
        }
    }
}
