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
    internal class MemberShipRepository : IMemberShipRepository
    {
        private readonly GymDbContext _dbContext;
        public MemberShipRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public int Add(MemberShip memberShip)
        {
            _dbContext.Add(memberShip);
            return memberShip.Id;
        }

        public int Delete(MemberShip memberShip)
        {
           _dbContext.Remove(memberShip);
              return memberShip.Id;
        }

        public IEnumerable<MemberShip> GetAll() => _dbContext.MemberShips.ToList();


        public MemberShip? GetById(int memberId, int planId)=> _dbContext.MemberShips.FirstOrDefault(ms=>ms.MemberId==memberId && ms.PlanId==planId);


        public int Update(MemberShip memberShip)
        {
            _dbContext.Update(memberShip);
            return memberShip.Id;
        }
    }
}
