namespace WorldOfWords.Web.ViewsModels
{
    using System;

    public class WordWithCount
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateAdded { get; set; }
        public int? Count { get; set; }
        public int LanguageId { get; set; }
    }
}
