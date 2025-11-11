using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.InterFaces
{
   public interface  ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel ? GetSessionById(int id);
        bool CreateSession(CreateSessionViewModel createsession);
        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        bool UpdateSession(UpdateSessionViewModel updateSession,int sessionId);
        bool RemoveSession(int sessionId);
        IEnumerable<TrainerSelectViewModel> GetAllTrainersForDropdown();
        IEnumerable<CategorySelectViewModel> GetAllCategoriesForDropdown();
       
    }
}
