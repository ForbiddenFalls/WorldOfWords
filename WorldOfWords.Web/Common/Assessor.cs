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

        public Assessor(string language, AppDbContext contex)
        {
            var data = new Data(contex);
            var letrtersPointsForLanguage = data.LettersPoints.All().FirstOrDefault(l => l.Language == language);
            if (letrtersPointsForLanguage == null)
            {
                throw new ArgumentException("Language is not found");
            }

            this.letters = letrtersPointsForLanguage.Letters;
            this.points = letrtersPointsForLanguage.Points
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