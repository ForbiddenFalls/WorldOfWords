using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorldOfWords.Web.Controllers
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = User.Identity.GetUserId();
                var userStats = this.Data.Users.All()
                    .FirstOrDefault(u => u.Id == userId);
                ViewBag.userStats = userStats;
                
                return View();
            }
            else
            {
                var boards = this.Data.Boards.All().Select(b=>b);
                ViewBag.boards = boards;
                
                return View();
            }

            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}