using AutoMapper;
using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public SessionService(IUnitOfWork unitOfWork,IMapper mapper)
        { 
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createsession)
        {
            try
            {
                if (!IsTranierExist(createsession.TrainerId)) return false;
                if (!IsCategoryExist(createsession.CategoryId)) return false;
                if (!IsValidDateRange(createsession.StartDate, createsession.EndDate)) return false;

                var MappingSession = _mapper.Map<CreateSessionViewModel, Session>(createsession);
                _unitOfWork.SessionRepository.Add(MappingSession);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }



        public UpdateSessionViewModel? GetSessionToUpdate  (int sessionId)
        {
            var Session = _unitOfWork.SessionRepository.GetById(sessionId);
            if (!IsSessionAvailableForUpdateing(Session!)) return null;
            return _mapper.Map<UpdateSessionViewModel>(Session);
        }

        public bool UpdateSession(UpdateSessionViewModel updateSession, int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForUpdateing(Session!)) return false;
                if (!IsTranierExist(updateSession.TrainerId) ) return false;
                if (!IsValidDateRange(updateSession.StartDate, updateSession.EndDate)) return false;
                _mapper.Map(updateSession, Session);
                Session!.UpdatedAt = DateTime.Now;
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;
            }
        }
        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Session = _unitOfWork.SessionRepository.GetById(sessionId);
                if (!IsSessionAvailableForRemove(Session!)) return false;
                _unitOfWork.SessionRepository.Delete(Session!);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            {
                return false;
            }

        }

        #region Helpers
        private bool IsTranierExist(int TrainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetById(TrainerId) is not null;

        }
        private bool IsCategoryExist(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetById(CategoryId) is not null;

        }
        private bool IsValidDateRange(DateTime StartDate, DateTime EndDate)
        {
            return StartDate < EndDate && StartDate > DateTime.Now;
        }
        private bool IsSessionAvailableForUpdateing(Session session)
        {
           if(session == null) return false;
            if (session.EndDate < DateTime.Now) return false;
            if (session.StartDate <= DateTime.Now ) return false;
           var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id)>0;
            if(HasActiveBooking) return false;
              return true;

        }
        private bool IsSessionAvailableForRemove(Session session)
        {
            if (session == null) return false;
            if (session.EndDate < DateTime.Now) return true;
            if (session.StartDate <= DateTime.Now) return false;
            if( session.EndDate > DateTime.Now) return false;
            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking) return false;
            return true;

        }

        #endregion

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var Sessions = _unitOfWork.SessionRepository.GetAllSessionWithTrainersAndCateogries();
            if (Sessions == null || !Sessions.Any())
                return [];
            #region Manual Mapping
            //return Sessions.Select(X => new SessionViewModel()
            //{
            //    Id = X.Id,
            //    Description = X.Description,
            //    TrainerName = X.SessionTrainer.Name,
            //    CategoryName = X.SessionCategory.CategoryName,
            //    StartDate = X.StartData,
            //    EndDate = X.EndData,
            //    Capacity = X.Capacity,
            //    AvailableSlots = X.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(X.Id)
            //});
            #endregion


            #region Auto Matic Mapping
            var MappingSessions = _mapper.Map<IEnumerable<Session> , IEnumerable<SessionViewModel>>(Sessions);
            return MappingSessions;

            #endregion


        }

        public SessionViewModel? GetSessionById(int id)
        {
            var Session = _unitOfWork.SessionRepository.GetSessionByWithTrainersAndCateogries(id);
            if (Session == null) return null;
           //return new SessionViewModel()
           // {
           //     Id = Session.Id,
           //     Description = Session.Description,
           //     TrainerName = Session.SessionTrainer.Name,
           //     CategoryName = Session.SessionCategory.CategoryName,
           //     StartDate = Session.StartData,
            //    EndDate = Session.EndData,
            //    Capacity = Session.Capacity,
            //    AvailableSlots = Session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(Session.Id)
            //};
        var MappingSession = _mapper.Map<Session,SessionViewModel>(Session);
            return MappingSession;
        }

        public IEnumerable<TrainerSelectViewModel> GetAllTrainersForDropdown()
        {
            var Trainers= _unitOfWork.GetRepository<Trainer>().GetAll();


            return _mapper.Map<IEnumerable<Trainer>,IEnumerable<TrainerSelectViewModel>>(Trainers);
        }

        public IEnumerable<CategorySelectViewModel> GetAllCategoriesForDropdown()
        {
           var Categories= _unitOfWork.GetRepository<Category>().GetAll();
              return _mapper.Map<IEnumerable<Category>, IEnumerable<CategorySelectViewModel>>(Categories);
        }

        //string? ISessionService.GetSessionToUpdate(int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
