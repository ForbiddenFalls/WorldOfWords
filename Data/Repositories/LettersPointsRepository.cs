namespace Data.Repositories
{
    using System.Data.Entity;
    using Models;

    public class LettersPointsRepository : GenericRepository<LettersPoints>
    {
        public LettersPointsRepository(DbContext context) : base(context)
        {
        }
    }
}
