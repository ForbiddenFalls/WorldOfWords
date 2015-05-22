namespace WorldOfWords.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Common;
    using Data;
    using Data.Contracts;
    using Data.Migrations;

    public abstract class BaseController : Controller
    {
        private const string Language = "bg";

        protected BaseController()
        {
            var context = new WorldOfWordsDbContext();
            this.Data = new WorldOfWordsData(context);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WorldOfWordsDbContext, Configuration>());
            var languageId = this.Data.Languages
                .First(l => l.LanguageCode == Language).Id;

            this.WordAssessor = new Assessor(languageId, context);
        }

        protected IWorldOfWordsData Data { get; set; }

        protected Assessor WordAssessor { get; set; }
    }
}