using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorldOfWords.Web.Controllers
{
    public class BoardsController : BaseController
    {
        // GET: Boards
        public ActionResult Show()
        {
            var board = this.Data.Boards.All().FirstOrDefault(b => b.Name == "Sofia");

            return View(board);
        }
    }
}