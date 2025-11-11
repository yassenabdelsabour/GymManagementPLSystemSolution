using GymManagementBLL.ViewModels.MemberViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.InterFaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel>GetAllMembers();
        bool CreateMember(CreateMemberViewModel CreateMember);
        MemberViewModel? GetMemberDetials(int MemberId);
        HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId);
        MemberToUpdateViewModel? GetMemberToUpdate(int MemberId);
        bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel member);
        bool RemoveMember(int MemberId);
        
    }
}
