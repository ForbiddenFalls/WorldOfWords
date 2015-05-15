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
            var context = new AppDbContext();
            this.Data = new Data(context);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>());
            this.WordAssessor = new Assessor(Language.ToLower(), context);
        }

        protected IData Data { get; set; }

        protected Assessor WordAssessor { get; set; }
    }
}