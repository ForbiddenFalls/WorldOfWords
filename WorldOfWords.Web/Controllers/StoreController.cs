using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PagedList;
using WorldOfWords.Web.ViewsModels;

namespace WorldOfWords.Web.Controllers
{
    [Authorize]
    public class StoreController : BaseController
    {
        private const int PageSize = 3;

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var currentUser = User.Identity.GetUserId();
            var words = this.Data.Words
                .Select(w => new WordWithCount
                {
                    Id = w.Id,
                    Content = w.Content,
                    Count = w.Users.FirstOrDefault(u => u.UserId == currentUser && u.WordId == w.Id).WordCount 
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
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not enough money");
            return this.Content("asd");
        }
    }
}