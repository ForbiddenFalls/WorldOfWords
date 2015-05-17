using Data.Contracts;

namespace WorldOfWords.Web.Controllers
{
    using System.Data.Entity;
    using System.Web.Mvc;
    using Common;
    using Data;
    using Data.Migrations;

    public abstract class BaseController : Controller
    {
        private const string Language = "bg";

        protected BaseController()
        {
            var context = new WorldOfWordsDbContext();
            this.Data = new WorldOfWordsData(context);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WorldOfWordsDbContext, Configuration>());
            //this.WordAssessor = new Assessor(Language.ToLower(), context);
        }

        protected IWorldOfWordsData Data { get; set; }

        protected Assessor WordAssessor { get; set; }
    }
}