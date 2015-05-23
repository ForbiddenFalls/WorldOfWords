namespace WorldOfWords.Web.Controllers
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using Models;
    using ViewsModels;

    public class BoardsController : BaseController
    {
        // GET: Boards
        [HttpGet]
        [Authorize]
        public ActionResult Show(string name)
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.Data.Users
                .Include(u => u.WordsUsers)
                .Include("WordsUsers.Words")
                .First(u => u.Id == userId);

            var board = this.Data.Boards.First(b => b.Name == name);
            if (board.Content == "")
            {
                board.Content = new String(' ', board.Size * board.Size);
                this.Data.SaveChanges();
            }

            var userWords = user.WordsUsers
                .Where(wu => wu.Word.Boards.All(b => b.Name != name))
                .Select(wu => wu.Word.Content)
                .ToList();

            var wordsPoints = userWords
                .ToList()
                .Select(this.WordAssessor.GetPointsByWord)
                .ToList();

            var boardUser = board.BoardsUsers.FirstOrDefault(bu => bu.UserId == userId);
            if (boardUser == null)
            {
                board.BoardsUsers.Add(new BoardsUsers
                {
                    BoardId = board.Id,
                    UserId = userId,
                });

                this.Data.SaveChanges();
            } 

            var userPointsFromBoard = board.BoardsUsers.First().UserPoints;

            var showBoardModel = new ShowBoardModel
            {
                Board = board,
                UserWords = userWords,
                WordsPoints = wordsPoints,
                UserPointsFromBoard = userPointsFromBoard,
                EarnedUserPoints = user.EarnedPoints
            };

            return View(showBoardModel);
        }
    }
}