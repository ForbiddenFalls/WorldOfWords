namespace Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Board
    {
        private ICollection<Word> words;
        private ICollection<BoardsUsers> users;

        public Board()
        {
            this.words = new HashSet<Word>();
            this.users = new HashSet<BoardsUsers>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int Size { get; set; }

        public string Content { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public virtual ICollection<Word> Words
        {
            get { return this.words; }
        }

        public virtual ICollection<BoardsUsers> Users
        {
            get { return this.users; }
        }
    }
}
