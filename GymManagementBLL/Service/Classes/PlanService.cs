using AutoMapper;
using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class PlanService : IPlanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PlanService(IUnitOfWork unitOfWork, IMapper mapper) 
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public IEnumerable<PlanViewModel> GetAllPlans()
        {
           var Plans = _unitOfWork.GetRepository<Plan>().GetAll();

            if (Plans == null || Plans.Any())
                return [];

            return Plans.Select(X => new PlanViewModel()
            {
                Id = X.Id,
                Name = X.Name,
                Description = X.Description,
                Price = X.Price,
                DurationDays = X.DurationDays,
                IsActive = X.IsActive
            });
            
        }

        public PlanViewModel? GetPlanDetails(int PlanId)
        {
           var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null)
                return null;
            return new PlanViewModel
            {
                Id = plan.Id,
                Name = plan.Name,
                Description = plan.Description,
                Price = plan.Price,
                DurationDays = plan.DurationDays,
                IsActive = plan.IsActive


            };
        }

        public UpdatePlanViewModel? GetPlanToUpdate(int PlanId)
        {
           var plan = _unitOfWork.GetRepository<Plan>().GetById(PlanId);
            if (plan == null || plan.IsActive == false || HasActiveMemberShip(PlanId))
                return null;


            //return new UpdatePlanViewModel
            //{
            //    PlanName = plan.Name,
            //    Description = plan.Description,
            //    Price = plan.Price,
            //    DurationDays = plan.DurationDays
            //};
            return _mapper.Map<UpdatePlanViewModel>(plan);
        }
        public bool UpdatePlan(int PlanId, UpdatePlanViewModel UpdatedPlan)
        {
            try
            {
                var PlanRepo = _unitOfWork.GetRepository<Plan>();
                var plan = PlanRepo.GetById(PlanId);
                if (plan == null || HasActiveMemberShip(PlanId))
                    return false;

                //(plan.Description, plan.DurationDays, plan.Price, plan.UpdatedAt) =
                //    (UpdatedPlan.Description, UpdatedPlan.DurationDays, UpdatedPlan.Price, DateTime.Now);

                _mapper.Map(UpdatedPlan, plan);

                PlanRepo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        public bool ToggleStatus(int planId)
        {
            var PlanRepo = _unitOfWork.GetRepository<Plan>();
            var plan = PlanRepo.GetById(planId);
            if (plan == null || HasActiveMemberShip(planId))
                return false;
            plan.IsActive = plan.IsActive == true ? false:true;
            try
            {
                PlanRepo.Update(plan);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        #region Helper Methods
        private bool HasActiveMemberShip(int PlanId)
        {
            return _unitOfWork.GetRepository<MemberShip>()
                .GetAll(X => X.PlanId==PlanId && X.Status=="Active").Any();
        }
        #endregion
    }
}
