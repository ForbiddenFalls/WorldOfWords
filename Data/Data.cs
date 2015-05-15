namespace Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using Models;
    using Repositories;

    public class Data : IData 
    {
        private DbContext context;
        private IDictionary<Type, object> repositories;

        public Data(AppDbContext context)
        {
            this.context = context;
            this.repositories = new Dictionary<Type, object>();
        }

        public UsersRepository Users
        {
            get { return (UsersRepository) this.GetRepository<User>(); }
        }

        public BoardsRepository Boards
        {
            get { return (BoardsRepository)this.GetRepository<Board>(); }
        }

        public LettersPointsRepository LettersPoints
        {
            get { return (LettersPointsRepository)this.GetRepository<LettersPoints>(); }
        }

        public int SaveChanges()
        {
            return this.context.SaveChanges();
        }

        private IRepository<T> GetRepository<T>() where T : class
        {
            var type = typeof(T);
            if (!this.repositories.ContainsKey(type))
            {
                var typeOfRepository = typeof(GenericRepository<T>);

                if (type.IsAssignableFrom(typeof(User)))
                {
                    typeOfRepository = typeof(UsersRepository);
                }

                if (type.IsAssignableFrom(typeof (Board)))
                {
                    typeOfRepository = typeof (BoardsRepository);
                }

                if (type.IsAssignableFrom(typeof(LettersPoints)))
                {
                    typeOfRepository = typeof(LettersPointsRepository);
                }

                var repository = Activator.CreateInstance(typeOfRepository, this.context);
                this.repositories.Add(type, repository);
            }

            return (IRepository<T>)this.repositories[type];
        }
    }
}
