using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork 
    {
        private readonly GymDbContext _dbContext;
        public UnitOfWork(GymDbContext dbContext, ISessionRepository SessionRepository)
        { 
            _dbContext = dbContext;
            SessionRepository = SessionRepository;
        }
        private readonly Dictionary<Type, object> _repositories = new ();

        public ISessionRepository SessionRepository { get; } 

        public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
           var EntityType = typeof(TEntity);
            if (_repositories.ContainsKey(EntityType))
           
            return (IGenericRepository<TEntity>)_repositories[EntityType];

            var NewRepo = new GenericRepository<TEntity>(_dbContext);
            _repositories[EntityType] = NewRepo;
            return NewRepo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

       
    }
}
