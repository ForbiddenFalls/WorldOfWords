namespace Data.Contracts
{
    using Models;
    using Data.Repositories;

    public interface IWorldOfWordsData
    {
        EFRepository<User> Users { get; }
        EFRepository<Board> Boards { get; }
        EFRepository<LettersPoints> LettersPoints { get; }
        EFRepository<Language> Languages { get; }
        EFRepository<Word> Words { get; }
        EFRepository<StoreWord> StoreWords { get; }


        void SaveChanges();
    }
}
