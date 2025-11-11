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
        public SessionRepository(GymDbContext dbContext):base (dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Session> GetAllSessionWithTrainersAndCateogries()
        {
          return _dbContext.Sessions
                .Include(s => s.SessionTrainer)
                .Include(s => s.SessionCategory)
                .ToList();
        }

        public int GetCountOfBookedSlots(int sessionId)
        {
            return _dbContext.MemberSessions
                .Count(sb => sb.SessionId == sessionId);
        }

        public Session? GetSessionByWithTrainersAndCateogries(int Id)
        {
            return _dbContext.Sessions
                .Include(s => s.SessionTrainer)
                .Include(s => s.SessionCategory)
                .FirstOrDefault(s => s.Id == Id);
        }
    }
}
