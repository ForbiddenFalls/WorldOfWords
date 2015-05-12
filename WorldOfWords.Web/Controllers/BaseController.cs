namespace WorldOfWords.Web.Controllers
{
    using System.Data.Entity;
    using System.Web.Mvc;
    using Data;
    using Data.Migrations;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            var context = new AppDbContext();
            this.Data = new Data(context);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<AppDbContext, Configuration>());
        }

        public IData Data { get; set; }
    }
}