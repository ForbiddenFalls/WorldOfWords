namespace Data
{
    using global::Data.Repositories;

    public interface IData
    {
        UsersRepository Users { get; }

        BoardsRepository Boards { get; }

        int SaveChanges();
    }
}
