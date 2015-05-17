namespace Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using EntityFramework.Extensions;
    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<WorldOfWordsDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(WorldOfWordsDbContext context)
        {
            if (!context.Words.Any())
            {
            
            addWordsToDb(context);

            var sizeBoard = 10;
            context.Boards.Delete();
            var boardVarna = new Board()
            {
                Name = "Varna",
                Size = sizeBoard,
                Content = "{\"content\":\""+ new String(' ', sizeBoard*sizeBoard) +"\",\"words\":[]}",
                ExpirationDate = DateTime.Now,
            };

            sizeBoard = 5;
            var boardSofia = new Board()
            {
                Name = "Sofia",
                Size = sizeBoard,
                Content = "{\"content\":\""+ new String(' ', sizeBoard*sizeBoard) +"\",\"words\":[]}",
                ExpirationDate = DateTime.Now,
            };

            var boardPlovdiv = new Board()
            {
                Name = "Plovdiv",
                Size = 5,
                Content = "{\"content\":\" куче о  х т  о к    а   \",\"words\":[\"котка\",\"куче\",\"ехо\"]}",
                ExpirationDate = DateTime.Now,
            };

            context.Boards.Add(boardSofia);
            context.Boards.Add(boardPlovdiv);
            context.Boards.Add(boardVarna);

            context.LettersPoints.Delete();
            context.Languages.Delete();

            var language = new Language
                {
                    LanguageCode = "bg"
                };

            context.Languages.Add(language);
            context.SaveChanges();

            var letters = "абвгдежзийклмнопрстуфхцчшщъью".ToCharArray();
            var points =
                "1,2,1,3,2,1,4,4,1,5,2,2,2,1,1,1,1,1,1,5,10,5,8,5,8,10,3,10,10,5".Split(',').ToList().Select(int.Parse).ToList();
            
            for (int i = 0; i < letters.Length; i++)
            {
                var l = new LettersPoints {LanguageCode = language.Id, Letter = letters[i], Points = points[i]};
                context.LettersPoints.Add(l);
            }

            context.SaveChanges();
            }
        }


        private void addWordsToDb(WorldOfWordsDbContext context)
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
                "кунбенизон",
                "абрихт",
            };

            foreach (var word in words)
            {
                var wordEntity = new Word()
                {
                    Content = word,
                    DateAdded = DateTime.Now
                };
                context.Words.Add(wordEntity);
            }
        }
    }
}
