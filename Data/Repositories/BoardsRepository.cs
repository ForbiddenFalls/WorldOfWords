namespace Data.Repositories
{
    using System.Data.Entity;
    using Models;

    public class BoardsRepository : GenericRepository<Board>
    {
        public BoardsRepository(DbContext context)
            : base(context)
        {
        }
    }
}
