using Microsoft.Ajax.Utilities;
using Models;

namespace WorldOfWords.Web.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using Data;

    public class Assessor
    {
        private List<LettersPoints> lettersPoints;

        public Assessor(int languageId, WorldOfWordsDbContext context)
        {
            var data = new WorldOfWordsData(context);

            this.lettersPoints = data.LettersPoints.Where(l => l.LanguageId == languageId).ToList();
        }

        public Assessor(int languageId)
            : this(languageId, new WorldOfWordsDbContext())
        {
          
        }

        public int GetPointsByLetter(char letter)
        {
            var letterEntity = this.lettersPoints.FirstOrDefault(l => l.Letter == letter);
            if (letterEntity == null)
            {
                throw new InvalidOperationException("Invalid letter");
            }

            return letterEntity.Points;
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