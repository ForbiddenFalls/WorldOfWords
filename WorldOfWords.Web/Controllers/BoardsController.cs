namespace WorldOfWords.Web.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using ViewsModels;

    public class BoardsController : BaseController
    {
        // GET: Boards
        public ActionResult Show()
        {
            var userWords = new[] {"мишка", "игла", "арка", "акула", "ластик"};

            var showBoardModel = new ShowBoardModel
            {
                Board = this.Data.Boards.All().FirstOrDefault(b => b.Name == "Varna"),
                UserWords = userWords,
                WordsPoints = userWords.ToList().Select(this.WordAssessor.GetPointsByWord).ToList()
            };

            return View(showBoardModel);
        }
    }
}