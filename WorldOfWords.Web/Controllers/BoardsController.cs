using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorldOfWords.Web.Controllers
{
    using global::Models.ViewsModels;

    public class BoardsController : BaseController
    {
        // GET: Boards
        public ActionResult Show()
        {
            var showBoardModel = new ShowBoardModel
            {
                Board = this.Data.Boards.All().FirstOrDefault(b => b.Name == "Varna"),
                UserWords = new[] {"мишка", "игла", "арка", "акула", "ластик"}
            };

            return View(showBoardModel);
        }
    }
}