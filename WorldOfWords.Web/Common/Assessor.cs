namespace WorldOfWords.Web.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Data;

    public class Assessor
    {
        private IList<int> points = null;
        private string letters = null;

        public Assessor(string language, AppDbContext context)
        {
            var data = new Data(context);
            var lettersPointsForLanguage = data.LettersPoints.All().FirstOrDefault(l => l.Language == language);
            if (lettersPointsForLanguage == null)
            {
                throw new ArgumentException("Language is not found");
            }

            this.letters = lettersPointsForLanguage.Letters;
            this.points = lettersPointsForLanguage.Points
                .Split(',')
                .Select(int.Parse)
                .ToList();
        }

        public int GetPointsByLetter(char letter)
        {
            var index = this.letters.IndexOf(letter);
            if (index < 0)
            {
                throw new ArgumentException("Letter " + letter +" is not fount", "letter");
            }

            return this.points[index];
        }

        public int GetPointsByWord(string word)
        {
            var wordPoints = word
                .ToCharArray()
                .ToList()
                .Select(this.GetPointsByLetter)
                .Sum();

            return wordPoints;
        }
    }
}