using AutoMapper;
using AutoMapper.Execution;
using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Repo= _unitOfWork.GetRepository<Trainer>();
                if (IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone))
                    return false;
                //var trainer = new Trainer()
                //{
                //    Name = createTrainer.Name,
                //    Email = createTrainer.Email,
                //    Phone = createTrainer.Phone,
                //    DateOfBirth = createTrainer.DateOfBirth,
                //    Gender = createTrainer.Gender,
                //    Specialties= createTrainer.Specialties,
                //    Address = new Address()
                //    {
                //        BulidingNumber = createTrainer.BuildingNumber,
                //        Street = createTrainer.Street,
                //        City = createTrainer.City
                //    }
                //};
                var MappedTrainer=_mapper.Map<Trainer>(createTrainer);
                Repo.Add(MappedTrainer);
                return _unitOfWork.SaveChanges()>0;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if(Trainers == null || !Trainers.Any())
                return [];
            //return Trainers.Select(X => new TrainerViewModel()
            //{
            //    Name = X.Name,
            //    Email = X.Email,
            //    Phone = X.Phone,
            //    Id = X.Id,
            //    Specialties = X.Specialties.ToString(),
            //});
            var TrainerViewModel = _mapper.Map<IEnumerable<Trainer>, IEnumerable<TrainerViewModel>>(Trainers);
            return TrainerViewModel;
        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null)
                return null;
            //return new TrainerViewModel()
            //{
            //    Name = trainer.Name,
            //    Email = trainer.Email,
            //    Phone = trainer.Phone,
            //    Specialties = trainer.Specialties.ToString(),
            //    DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
            //    Address = $"{trainer.Address.BulidingNumber}, {trainer.Address.Street}, {trainer.Address.City}",

            //};
            var TrainerViewModel = _mapper.Map<TrainerViewModel>(trainer);
            return TrainerViewModel;
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null)
                return null;
            //return new TrainerToUpdateViewModel()
            //{
            //    Name = trainer.Name,
            //    Email = trainer.Email,
            //    Phone = trainer.Phone,
            //    BuildingNumber = trainer.Address.BulidingNumber,
            //    Street = trainer.Address.Street,
            //    City = trainer.Address.City,
            //    Specialties = trainer.Specialties
            //};
            return _mapper.Map<TrainerToUpdateViewModel>(trainer);
        }

        public bool RemoveTrainer(int TrainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(TrainerId);
            if (TrainerToRemove is null || HasActiveSession(TrainerId))
                return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        

        public bool UpdateTrainerDetails(int TrainerId, TrainerToUpdateViewModel UpdatedTrainer)
        {
            var EmailExist = _unitOfWork.GetRepository<Trainer>()
                .GetAll(X => X.Email == UpdatedTrainer.Email && X.Id != TrainerId).Any();
            var PhoneExist = _unitOfWork.GetRepository<Trainer>()
                .GetAll(X => X.Phone == UpdatedTrainer.Phone && X.Id != TrainerId).Any();
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = Repo.GetById(TrainerId);

            if (TrainerToUpdate is null || EmailExist || PhoneExist)return false;

            TrainerToUpdate.Email = UpdatedTrainer.Email;
            TrainerToUpdate.Phone = UpdatedTrainer.Phone;
            TrainerToUpdate.Address.BulidingNumber = UpdatedTrainer.BuildingNumber;
            TrainerToUpdate.Address.Street = UpdatedTrainer.Street;
            TrainerToUpdate.Address.City = UpdatedTrainer.City;
            TrainerToUpdate.Specialties = UpdatedTrainer.Specialties;
            TrainerToUpdate.UpdatedAt = DateTime.Now;
            Repo.Update(TrainerToUpdate);
            return _unitOfWork.SaveChanges() > 0;

        }

        #region HelperMethods
        private bool IsEmailExist(string email)
        {
            var existing = _unitOfWork.GetRepository<GymManagementDAL.Entities.Member>().GetAll(
                m => m.Email == email).Any();
            return existing;
        }

        private bool IsPhoneExist(string phone)
        {
            var existing = _unitOfWork.GetRepository<GymManagementDAL.Entities.Member>().GetAll(
                m => m.Phone == phone).Any();
            return existing;
        }

        private bool HasActiveSession(int Id)
        {
            var activeSessions = _unitOfWork.GetRepository<Session>().GetAll(
               s => s.TrainerId == Id && s.StartDate > DateTime.Now).Any();
            return activeSessions;
        }










        #endregion
    }


}
