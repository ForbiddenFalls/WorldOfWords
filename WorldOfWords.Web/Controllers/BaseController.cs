﻿namespace WorldOfWords.Web.Controllers
{
    using System.Data.Entity;
    using System.Linq;
    using System.Threading;
    using System.Web.Mvc;
    using Common;
    using Data;
    using Data.Contracts;
    using Data.Migrations;

    public abstract class BaseController : Controller
    {
        protected BaseController()
        {
            var context = new WorldOfWordsDbContext();
            this.Data = new WorldOfWordsData(context);
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<WorldOfWordsDbContext, Configuration>());
            
            this.WordAssessor = new Assessor(Config.Language, this.Data);
        }

        protected IWorldOfWordsData Data { get; set; }

        protected Assessor WordAssessor { get; set; }
    }
}