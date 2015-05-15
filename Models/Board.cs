namespace Models
{
    using System;
    using System.Collections.Generic;

    public class Board
    {
        private ICollection<Word> words;
        private ICollection<User> users;

        public Board()
        {
            this.words = new HashSet<Word>();
            this.users = new HashSet<User>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public string Content { get; set; }

        public DateTime ExpirationDate { get; set; }

        public virtual ICollection<User> Users
        {
            get { return this.users; }
        }

        public virtual ICollection<Word> Words
        {
            get { return this.words; }
        }
    }
}
