namespace WorldOfWords.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using ViewsModels;

    public class BoardsController : BaseController
    {
        // GET: Boards
        public ActionResult Show()
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users
                .Include(u => u.WordsUsers)
                .Include("WordsUsers.Words")
                .First(u => u.Id == userId);

            var userWords = user.WordsUsers
                .Select(wu => wu.Word.Content)
                .ToList();

            var board = this.Data.Boards.First(b => b.Name == "Varna");
            if (board.Content == "")
            {
                board.Content = new String(' ', board.Size * board.Size);
                this.Data.SaveChanges();
            }

            var wordsPoints = userWords.ToList().Select(this.WordAssessor.GetPointsByWord).ToList();

            var showBoardModel = new ShowBoardModel
            {
                Board = board,
                UserWords = userWords,
                WordsPoints = wordsPoints
            };

            return View(showBoardModel);
        }

        public ActionResult AddWordToBoard()
        {

            return null;
        }
    }
}