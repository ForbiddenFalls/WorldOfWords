namespace Models
{
    using System.ComponentModel.DataAnnotations;

    public class LettersPoints
    {
        public int Id { get; set; }

        [Required]
        public int LanguageId { get; set; }

        [Required]
        public char Letter { get; set; }

        [Required]
        public int Points { get; set; }
    }
}
