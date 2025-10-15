﻿using GymManagementDAL.Data.Contexts;
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
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;
        public GenericRepository(GymDbContext dbContext )
        {
            _dbContext = dbContext;

        }
        public void Add(TEntity entity)=> _dbContext.Set<TEntity>().Add(entity);


        public void Delete(TEntity entity)=> _dbContext.Set<TEntity>().Remove(entity);
 

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? condition = null)
        {
            if (condition is null)
                return _dbContext.Set<TEntity>().AsNoTracking().ToList();
            else
                return _dbContext.Set<TEntity>().AsNoTracking().Where(condition).ToList();

        }


        public TEntity? GetById(int id)=> _dbContext.Set<TEntity>().Find(id);


        public void Update(TEntity entity)=> _dbContext.Set<TEntity>().Update(entity);

    }
}
