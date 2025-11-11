using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AnalyticsService(IUnitOfWork unitOfWork)
        { 
            _unitOfWork = unitOfWork;
        }
        public AnalyticsViewModel GetAnalyticsData()
        {
            //var sessions=_unitOfWork.SessionRepository.GetAll();
            return new AnalyticsViewModel()
            {
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.Status == "Active").Count(),
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = _unitOfWork.GetRepository<Session>().GetAll().Count(X => X.StartDate > DateTime.Now),
                OngoingSessions = _unitOfWork.GetRepository<Session>().GetAll().Count(X => X.StartDate <= DateTime.Now && X.EndDate > DateTime.Now),
                CompletedSessions = _unitOfWork.GetRepository<Session>().GetAll().Count(X => X.EndDate < DateTime.Now)
            };

        }
    }
}
