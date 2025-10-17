using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.InterFaces
{
    internal interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel ? GetSessionById(int id);
        bool CreateSession(CreateSessionViewModel createsession);
        bool UpdateSession(UpdateSessionViewModel updateSession,int sessionId);
        bool RemoveSession(int sessionId);

    }
}
