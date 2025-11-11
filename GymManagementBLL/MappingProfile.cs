using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapSession();
            MapMember();
            MapTrainer();
            MapPlan();
        }
        private void MapSession() {
            CreateMap<Session, SessionViewModel>()
            .ForMember(dest => dest.TrainerName, Options => Options.MapFrom(src => src.SessionTrainer.Name))
            .ForMember(dest => dest.CategoryName, Options => Options.MapFrom(src => src.SessionCategory.CategoryName))
            .ForMember(dest => dest.AvailableSlots, Options => Options.Ignore());
            CreateMap<CreateSessionViewModel, Session>().ReverseMap();
            CreateMap<Session, UpdateSessionViewModel>().ReverseMap();
            CreateMap<Trainer, TrainerSelectViewModel>();
            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
        }
        private void MapMember()
        {
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));


            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(dest => dest.BulidingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street));

            CreateMap<HealthRecordViewModel, HealthRecord>().ReverseMap();
           
            CreateMap<Member, MemberViewModel>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BulidingNumber} - {src.Address.Street}- {src.Address.City}"));

            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BulidingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));

            CreateMap<MemberToUpdateViewModel, Member>()
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Address.BulidingNumber = src.BuildingNumber;
                    dest.Address.Street = src.Street;
                    dest.Address.City = src.City;
                    dest.UpdatedAt = DateTime.Now;
                });
            

        }
        private void MapTrainer()
        {
            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => new Address
                {
                    BulidingNumber= src.BuildingNumber,
                    Street = src.Street,
                    City = src.City
                }));

            CreateMap<Trainer, TrainerViewModel>();

            CreateMap<Trainer, TrainerToUpdateViewModel>()
                .ForMember(dist => dist.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dist => dist.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dist => dist.BuildingNumber, opt => opt.MapFrom(src => src.Address.BulidingNumber));

            CreateMap<TrainerToUpdateViewModel, Trainer>()
            .ForMember(dest => dest.Name, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Address.BulidingNumber = src.BuildingNumber;
                dest.Address.City = src.City;
                dest.Address.Street = src.Street;
                dest.UpdatedAt = DateTime.Now;
            });

        
        }
        private void MapPlan()
        {
            CreateMap<Plan, PlanViewModel>();
            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));
            CreateMap<UpdatePlanViewModel, Plan>()
           .ForMember(dest => dest.Name, opt => opt.Ignore())
           .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now));

        }

    }
}
