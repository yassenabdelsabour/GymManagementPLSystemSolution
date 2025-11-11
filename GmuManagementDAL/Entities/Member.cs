using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Entities
{
    public class Member: GymUser
    {
        //JoinDate = CreatedAt of BaseEntity by fault api
        public string Photo { get; set; } = null!;
        #region RelationShips
        #region Member - HealthRecored (1-1)
        public HealthRecord HealthRecord { get; set; } = null!;
        #endregion

        #region Member - MemberShip (M-M)
        public ICollection<MemberShip> MemberShips { get; set; } = null!;
        #endregion

        #region Member - Session (M-M)
        public ICollection<MemberSession> MemberSessions { get; set; } = null!;
        #endregion

        #endregion
    }
}
