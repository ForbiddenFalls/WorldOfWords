using System.Web.UI;
using Models;

namespace WorldOfWords.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Web.Helpers;
    using System.Web.Mvc;
    using Microsoft.AspNet.Identity;
    using PagedList;
    using WorldOfWords.Web.Common;
    using WorldOfWords.Web.ViewsModels;

    [Authorize]
    public class StoreController : BaseController
    {
        private const int PageSize = 3;

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var languageCode = "bg";
            ViewBag.Assessor = new Assessor(this.Data.Languages.FirstOrDefault(l => l.LanguageCode == languageCode).Id);

            var currentUser = User.Identity.GetUserId();
            var words = this.Data.Words
                .Select(w => new WordWithCount
                {
                    Id = w.Id,
                    Content = w.Content,
                    Quantity = w.Users.FirstOrDefault(u => u.UserId == currentUser && u.WordId == w.Id).WordCount,
                    LanguageId = w.LanguageId,
                    DateAdded = w.DateAdded
                })
                .AsQueryable();

            #region Search
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                words = words.Where(w => w.Content.Contains(searchString));
            }
            #endregion

            #region Sorting
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    words = words.OrderByDescending(w => w.Content);
                    break;
                case "Date":
                    words = words.OrderBy(w => w.DateAdded);
                    break;
                case "date_desc":
                    words = words.OrderByDescending(w => w.DateAdded);
                    break;
                default:  // Name ascending 
                    words = words.OrderBy(s => s.Content);
                    break;
            }
            #endregion

            int pageNumber = (page ?? 1);
            return View(words.ToPagedList(pageNumber, PageSize));
        }

        public ActionResult BuyWord(int id)
        {
            var currentUser = this.User.Identity.GetUserId();

            var userDb = this.Data.Users.FirstOrDefault(u => u.Id == currentUser);
            var balanceOfUser = userDb.Balance;

            var word = this.Data.Words.FirstOrDefault(w => w.Id == id);
            var balanceNeededForWord = new Assessor(word.LanguageId).GetPointsByWord(word.Content);

            if (balanceOfUser < balanceNeededForWord)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not enought balance");
            }

            userDb.Balance = userDb.Balance - balanceNeededForWord;

            var userWord = userDb.Words.FirstOrDefault(w => w.WordId == id);
            if (userWord != null)
            {
                userWord.WordCount++;
            }
            else
            {
                userDb.Words.Add(new WordsUsers()
                {
                    WordId = id,
                    WordCount = 1
                });
            }
            this.Data.SaveChanges();

            var count = userDb.Words.FirstOrDefault(w => w.WordId == id).WordCount;
            return Json(new { wordId = id, newQuantity = count }, JsonRequestBehavior.AllowGet);
        }
    }
}