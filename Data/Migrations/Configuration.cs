namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using EntityFramework.Extensions;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<WorldOfWordsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WorldOfWordsDbContext context)
        {
            //Prevent loop Seed
            if (context.Words.Any()) return;
            if (context.Boards.Any()) return;
            if (context.Languages.Any()) return;

            AddLanguagesToDb(context);
            AddWordsToDbAndStore(context);
            AddRolesToDb(context);

            var sizeBoard = 10;

            var boardVarna = new Board()
            {
                Name = "Varna",
                Size = sizeBoard,
                Content = new String(' ', sizeBoard * sizeBoard),
                ExpirationDate = DateTime.Now,
            };

            sizeBoard = 5;
            var boardSofia = new Board()
            {
                Name = "Sofia",
                Size = sizeBoard,
                Content = "",
                ExpirationDate = DateTime.Now,
            };

            var boardPlovdiv = new Board()
            {
                Name = "Plovdiv",
                Size = 5,
                Content = "",
                ExpirationDate = DateTime.Now,
            };

            context.Boards.Add(boardSofia);
            context.Boards.Add(boardPlovdiv);
            context.Boards.Add(boardVarna);

            var language = context.Languages.FirstOrDefault(l => l.LanguageCode == "bg");

            var letters = "абвгдежзийклмнопрстуфхцчшщъьюя";
            var points =
                "1,2,1,3,2,1,4,4,1,5,2,2,2,1,1,1,1,1,1,5,10,5,8,5,8,10,3,10,10,5, 6".Split(',').ToList().Select(int.Parse).ToList();

            for (int i = 0; i < letters.Length; i++)
            {
                var l = new LettersPoints { LanguageId = language.Id, Letter = letters[i].ToString(), Points = points[i] };
                context.LettersPoints.Add(l);
            }

            context.SaveChanges();
        }

        private void AddRolesToDb(WorldOfWordsDbContext context)
        {
            var storeRole = new RoleStore<IdentityRole>(context);
            var managerRole = new RoleManager<IdentityRole>(storeRole);
            var adminRole = new IdentityRole { Name = "admin" };
            var userRole = new IdentityRole { Name = "user" };
            var moderatorRole = new IdentityRole { Name = "moderator" };
            managerRole.Create(adminRole);
            managerRole.Create(userRole);
            managerRole.Create(moderatorRole);

            var storeUser = new UserStore<User>(context);
            var managerUser = new UserManager<User>(storeUser);

            var admin = new User { UserName = "admin@admin.a", Email = "admin@admin.a" };
            var moderator = new User { UserName = "moderator@mod.m", Email = "moderator@mod.m" };
            var resultUser = managerUser.Create(admin, "Aa#123456");
            var resultmod = managerUser.Create(moderator, "Aa#123456");
            context.SaveChanges();

            var adminResult = context.Users.FirstOrDefault(x => x.UserName == "admin@admin.a");
            var moderatorResult = context.Users.FirstOrDefault(x => x.UserName == "moderator@mod.m");
            managerUser.AddToRole(adminResult.Id, "admin");
            managerUser.AddToRole(moderatorResult.Id, "moderator");
            context.SaveChanges();

        }

        private void AddLanguagesToDb(WorldOfWordsDbContext context)
        {
            var languageCodes = new string[] {"bg"};
            foreach (var languageCode in languageCodes)
            {
                context.Languages.Add(new Language()
                {
                    LanguageCode = languageCode
                });
            }

            context.SaveChanges();
        }

        private void AddWordsToDbAndStore(WorldOfWordsDbContext context)
        {
            var words = new string[]
            {
                "котка",
                "куче",
                "ехо",
                "ябълка",
                "круша",
                "слива",
                "дънки",
                "ластик",
                "жаба",
                "комбинезон",
                "абрихт",
                "комбинирам",
                "ягода",
                "щайга",
                "шайба",
            };

            var language = context.Languages.FirstOrDefault(l => l.LanguageCode == "bg");

            foreach (var word in words)
            {
                var wordEntity = new Word()
                {
                    Content = word,
                    DateAdded = DateTime.Now,
                    Language = language
                };
                context.Words.Add(wordEntity);
                context.StoreWords.Add(new StoreWord()
                {
                    DateAdded = DateTime.Now,
                    Word = wordEntity,
                    Quantity = 2
                });
            }

            context.SaveChanges();
        }
    }
}
