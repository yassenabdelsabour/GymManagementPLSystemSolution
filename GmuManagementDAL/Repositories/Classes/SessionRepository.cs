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
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;
        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Session> GetAllSessionsWithTrainersAndCateagries()
        {
            return _dbContext.Sessions.Include(X => X.SessionTrainer)
                .Include(s => s.SessionCategory)
                .ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions.Count(ms => ms.SessionId == sessionId);
        }

        public Session? GetSessionWithTrainerAndCategoryById(int id)
        {
            return _dbContext.Sessions.Include(X => X.SessionTrainer)
                .Include(X => X.SessionCategory)
                .FirstOrDefault(X => X.Id == id);
        }
    }


}
