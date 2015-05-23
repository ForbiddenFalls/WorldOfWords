﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorldOfWords.Web.Controllers
{
    using System.Data.Entity;
    using Microsoft.AspNet.Identity;
    using Models;
    using ViewsModels;

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            var homeInfo = new HomeViewModel();

            //take all boards
            var boardsDb = this.Data.Boards.All()
                .Select(b => b)
                .ToList();

            if (boardsDb.Count != 0)
            {
                homeInfo.AllBoards = new List<Board>();
                foreach (var board in boardsDb)
                {
                    homeInfo.AllBoards.Add(new Board
                    {
                        Name = board.Name,
                        ExpirationDate = board.ExpirationDate,
                        Size = board.Size
                    });
                }
            }

            //take User info
            if (this.User.Identity.IsAuthenticated)
            {
                string userId = this.User.Identity.GetUserId();
                var userStats = this.Data.Users.All()
                    .FirstOrDefault(u => u.Id == userId);

                homeInfo.UserName = User.Identity.Name;
                homeInfo.Balance = userStats.Balance;
                homeInfo.RegisteredOn = userStats.RegisteredOn;
                homeInfo.EarnedPoints = userStats.EarnedPoints;

                if (userStats.BoardsUsers != null)
                {
                    homeInfo.BoardsUsers = userStats.BoardsUsers;
                }

                if (userStats.WordsUsers != null)
                {
                    homeInfo.WordsUsers = userStats.WordsUsers;
                }
            }

            return View(homeInfo);
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