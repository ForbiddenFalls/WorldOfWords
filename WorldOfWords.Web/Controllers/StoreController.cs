using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace WorldOfWords.Web.Controllers
{
    [Authorize]
    public class StoreController : BaseController
    {
        private const int PageSize = 3;

        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            var words = this.Data.Words.OrderBy(w => w.Content).AsQueryable();

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(searchString))
            {
                words = words.Where(w => w.Content.Contains(searchString));
            }

            return View(words.ToPagedList(pageNumber, PageSize));
        }
    }
}