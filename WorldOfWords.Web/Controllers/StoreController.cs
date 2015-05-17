using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace WorldOfWords.Web.Controllers
{
    [Authorize]
    public class StoreController : BaseController
    {
        private const int PageSize = 3;

        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            var words = this.Data.Words.AsQueryable();

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

        public ActionResult BuyWord()
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "not enough money");
            return this.Content("asd");
        }
    }
}