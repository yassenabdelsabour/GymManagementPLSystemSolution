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
    internal class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var Repo= _unitOfWork.GetRepository<Trainer>();
                if (IsEmailExist(createTrainer.Email) || IsPhoneExist(createTrainer.Phone))
                    return false;
                var trainer = new Trainer()
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Gender = createTrainer.Gender,
                    Specialties= createTrainer.Specialties,
                    Address = new Address()
                    {
                        BulidingNumber = createTrainer.BuildingNumber,
                        Street = createTrainer.Street,
                        City = createTrainer.City
                    }
                };
                Repo.Add(trainer);
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
            return Trainers.Select(X => new TrainerViewModel()
            {
                Name = X.Name,
                Email = X.Email,
                Phone = X.Phone,
                Id = X.Id,
                Specialties = X.Specialties.ToString(),
            });
        }

        public TrainerViewModel? GetTrainerDetails(int TrainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null)
                return null;
            return new TrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialties.ToString(),

            };
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(TrainerId);
            if (trainer == null)
                return null;
            return new TrainerToUpdateViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BulidingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Specialties
            };
        }

        public bool RemoveTrainer(int TrainerId)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToRemove = Repo.GetById(TrainerId);
            if (TrainerToRemove is null || HasActiveSessions(TrainerId))
                return false;
            Repo.Delete(TrainerToRemove);
            return _unitOfWork.SaveChanges() > 0;
        }

        private bool HasActiveSessions(int trainerId)
        {
            return _unitOfWork.GetRepository<Session>().GetAll(X=>X.TrainerId==trainerId && X.EndData > DateTime.Now).Any();
        }

        public bool UpdateTrainerDetails(int TrainerId, UpdateTrainerViewModel UpdatedTrainer)
        {
            var Repo = _unitOfWork.GetRepository<Trainer>();
            var TrainerToUpdate = Repo.GetById(TrainerId);

            if (TrainerToUpdate == null || IsEmailExist(UpdatedTrainer.Email) || IsPhoneExist(UpdatedTrainer.Phone)) { return false; }
            TrainerToUpdate.Email= UpdatedTrainer.Email;
                TrainerToUpdate.Phone= UpdatedTrainer.Phone;
                TrainerToUpdate.Address.BulidingNumber= UpdatedTrainer.BuildingNumber;
                TrainerToUpdate.Address.Street= UpdatedTrainer.Street;
                TrainerToUpdate.Address.City= UpdatedTrainer.City;
                TrainerToUpdate.Specialties= UpdatedTrainer.Specialties;
                TrainerToUpdate.UpdatedAt= DateTime.Now;
                Repo.Update(TrainerToUpdate);
            return _unitOfWork.SaveChanges() > 0;

        }

        #region HelperMethods
        private bool IsEmailExist(string Email)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Email == Email).Any();
        }
        private bool IsPhoneExist(string Phone)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(X => X.Phone == Phone).Any();
        }
        #endregion
    }


}
