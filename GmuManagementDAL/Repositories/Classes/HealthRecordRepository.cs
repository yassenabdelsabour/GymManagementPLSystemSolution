using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    internal class HealthRecordRepository : IHealthRecordRepository
    {
        private readonly GymDbContext _dbContext;
        public HealthRecordRepository(GymDbContext dbContext )
        {
         _dbContext = dbContext;    
        }
        public int Add(HealthRecord healthRecord)
        {
            _dbContext.Add(healthRecord);
            return _dbContext.SaveChanges();
        }

        public int Delete(HealthRecord healthRecord)
        {
            _dbContext.Remove(healthRecord);
            return _dbContext.SaveChanges();
        }

        public IEnumerable<HealthRecord> GetAll() => _dbContext.HealthRecords.ToList();


        public HealthRecord? GetById(int id) => _dbContext.HealthRecords.Find(id);


        public int Update(HealthRecord healthRecord)
        {
            _dbContext.Update(healthRecord);
            return _dbContext.SaveChanges();
        }
    }
}
