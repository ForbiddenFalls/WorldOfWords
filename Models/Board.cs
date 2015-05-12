namespace Models
{
    using System;

    public class Board
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Size { get; set; }

        public string Content { get; set; }

        public DateTime ExpairyTime { get; set; }
    }
}
