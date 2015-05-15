namespace Models
{
    using System.ComponentModel.DataAnnotations;

    public class LettersPoints
    {
        public int Id { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        public string Letters { get; set; }

        [Required]
        public string Points { get; set; }
    }
}
