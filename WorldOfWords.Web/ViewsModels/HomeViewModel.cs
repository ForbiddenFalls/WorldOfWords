namespace WorldOfWords.Web.ViewsModels
{
    using System;
    using System.Collections.Generic;
    using Models;

    public class HomeViewModel
    {
        public string UserName { get; set; }

        public long EarnedPoints { get; set; }

        public int Balance { get; set; }

        public DateTime RegisteredOn { get; set; }

        public virtual ICollection<WordsUsers> WordsUsers { get; set; }

        public virtual ICollection<BoardsUsers> BoardsUsers { get; set; }

        public List<Board> AllBoards { get; set; } 
    }
}