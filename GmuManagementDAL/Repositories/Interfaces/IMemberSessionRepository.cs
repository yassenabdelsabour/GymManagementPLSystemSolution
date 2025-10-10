using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    internal interface IMemberSessionRepository
    {
        IEnumerable<MemberSession> GetAll();
        MemberSession? GetById(int memberId, int sessionId);
        int Add(MemberSession memberSession);
        int Update(MemberSession memberSession);
        int Delete(MemberSession memberSession);
    }
}
