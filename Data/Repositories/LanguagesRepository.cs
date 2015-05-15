namespace Data.Repositories
{
    using System.Data.Entity;
    using Models;

    public class  LanguagesRepository : GenericRepository<Language>
    {
        public LanguagesRepository(DbContext context)
            : base(context)
        {
        }
    }
}
