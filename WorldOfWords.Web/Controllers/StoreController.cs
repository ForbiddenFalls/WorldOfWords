using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.UI;
using Models;
using WorldOfWords.Web.BindingModels;

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
           return View();
        }

        public ActionResult Store(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var languageCode = "bg";
            ViewBag.Assessor = new Assessor(this.Data.Languages.FirstOrDefault(l => l.LanguageCode == languageCode).Id);

            var currentUser = User.Identity.GetUserId();
            var words = this.Data.StoreWords
                .Select(sw => new WordWithCount
                {
                    Id = sw.Word.Id,
                    Content = sw.Word.Content,
                    Quantity = sw.Quantity,
                    QuantityUser = sw.Word.Users.FirstOrDefault(u => u.UserId == currentUser && u.WordId == sw.Id).WordCount,
                    LanguageId = sw.Word.LanguageId,
                    DateAdded = sw.DateAdded
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
            return PartialView(words.ToPagedList(pageNumber, PageSize));
        }

        [HttpPost]
        public ActionResult Cart(List<ShopItem> shopList)
        {
            return Content("asd");
        }

        public ActionResult BuyWord(int id)
        {
            var storeWord = this.Data.StoreWords.FirstOrDefault(w => w.Word.Id == id);
            if (storeWord == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "This word is not available at the moment");
            }

            var currentUser = this.User.Identity.GetUserId();

            var userDb = this.Data.Users.FirstOrDefault(u => u.Id == currentUser);
            var balanceOfUser = userDb.Balance;

            var balanceNeededForWord = new Assessor(storeWord.Word.LanguageId).GetPointsByWord(storeWord.Word.Content);

            if (balanceOfUser < balanceNeededForWord)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Not enought balance");
            }

            userDb.Balance = userDb.Balance - balanceNeededForWord;

            storeWord.Quantity--;
            if (storeWord.Quantity == 0)
            {
                this.Data.StoreWords.Delete(storeWord);
            }

            var userWord = userDb.WordsUsers.FirstOrDefault(w => w.WordId == id);
            if (userWord != null)
            {
                userWord.WordCount++;
            }
            else
            {
                userDb.WordsUsers.Add(new WordsUsers()
                {
                    WordId = id,
                    WordCount = 1
                });
            }
            this.Data.SaveChanges();

            var userQuantity = userDb.WordsUsers.FirstOrDefault(w => w.WordId == id).WordCount;
            return Json(new { wordId = id, newQuantity = storeWord.Quantity, newUserQuantity = userQuantity }, JsonRequestBehavior.AllowGet);
        }
    }
}