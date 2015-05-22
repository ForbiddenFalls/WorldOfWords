namespace WorldOfWords.Web.Hubs
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using Controllers;
    using Data.Contracts;
    using Microsoft.AspNet.Identity;
    using Models;
    using Newtonsoft.Json;

    public class BoardsHub : BaseHub
    {
        public void JoinBoard(string name)
        {
            this.Groups.Add(this.Context.ConnectionId, name);
        }

        public string AddWordToBoard(string boardName, string addedWord, int catchedLetterId, bool isVertical, int dropCellId)
        {
            Board board = null;

            try
            {
                if (!Context.User.Identity.IsAuthenticated)
                {
                    throw new AccessViolationException("User is not logged!");
                }

                var userId = Context.User.Identity.GetUserId();

                board = Data.Boards.FirstOrDefault(b => b.Name == boardName);
                if (board == null)
                {
                    // throw new ApplicationException(String.Format("Board with name {0} is not found.", boardName));
                    return String.Format("Board with name {0} is not found.", boardName);
                }

                var word = this.Data.WordsUsers
                    .FirstOrDefault(w => w.Word.Content == addedWord);
                if (word == null)
                {
                    throw new ApplicationException("User has no such word.");
                }

                var startPosition = this.GetStartPosition(board.Size, addedWord.Count(), catchedLetterId, isVertical,
                    dropCellId);

                if (startPosition < 0)
                {
                    throw new ApplicationException("The word is out board.");
                }


                var newContent = AddWordInBoardContent(board, addedWord, catchedLetterId, isVertical, dropCellId,
                    startPosition);

                if (newContent == null)
                {
                    throw new ApplicationException("Illegal crossing of words.");
                }

                if (board.Words.All(w => w.Content != addedWord))
                {
                    board.Words.Add(word.Word);
                    board.Content = newContent;
                    if (word.WordCount > 1)
                    {
                        word.WordCount--;
                    }
                    else
                    {
                        this.Data.WordsUsers.Delete(word);
                    }

                    this.Data.SaveChanges();
                }
                else
                {
                    throw new ApplicationException(string.Format("The word \"{0}\" is already  embedded",
                        word.Word.Content));
                }
            }
            catch (ApplicationException ex)
            {
                return ex.Message;
            }
            finally
            {
                var json = JsonConvert.SerializeObject(new
               {
                   content = board.Content,
                   words = board.Words.Select(w => w.Content).ToList()
               });

                this.Clients.Group(boardName).loadBoard(json);
            }

            return null;
        }


        private int GetStartPosition(int boardSize, int wordLength, int catchedLetterId, bool isVertical, int dropCellId)
        {
            int start = 0;
            int end = 0;

            if (isVertical)
            {
                start = dropCellId - catchedLetterId * boardSize;
                end = start + (wordLength - 1) * boardSize;
                if (start < 0 || end >= boardSize * boardSize)
                {
                    return -1;
                }
            }
            else
            {
                start = dropCellId - catchedLetterId;
                end = start + wordLength - 1;

                if ((start / boardSize) != (end / boardSize))
                {
                    return -1;
                }
            }

            return start;
        }

        private string AddWordInBoardContent(Board board, string addedWord, int catchedLetterId, bool isVertical, int dropCellId, int startPosition)
        {
            var s = board.Size;
            var cells = board.Content.ToCharArray();

            if (!isVertical)
            {
                s = 1;
            }

            for (var i = 0; i < addedWord.Count(); i++)
            {
                var currentCell = cells[startPosition + i * s];
                if (currentCell != addedWord[i] && currentCell != ' ')
                {
                    return null;
                }
            }

            for (var i = 0; i < addedWord.Count(); i++)
            {
                cells[startPosition + i * s] = addedWord[i];
            }

            return string.Join("", cells);
        }


    }
}