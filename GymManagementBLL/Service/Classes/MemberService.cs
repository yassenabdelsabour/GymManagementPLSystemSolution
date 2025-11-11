using AutoMapper;
using GymManagementBLL.Service.AttachmentService;
using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Entities.Enums;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork,IMapper mapper,IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }

        public bool CreateMember(CreateMemberViewModel CreateMember)
        {
            //// Check if Email Exist
            //var EmailExist = _memberRepository.GetAll().Any(X => X.Email == createMember.Email);
            //// Check if Phone Exist
            //var PhoneExist = _memberRepository.GetAll().Any(X => X.Phone == createMember.Phone);
            try
            {
                if (IsEmailExist(CreateMember.Email) || IsPhoneExist(CreateMember.Phone))
                   return false;

                var PhotoName=_attachmentService
                    .UploadPhoto("Members", CreateMember.PhotoFile);

                if(string.IsNullOrEmpty(PhotoName))return false;
               

                //var member = new Member()
                //{
                //    Name = createMember.Name,
                //    Email = createMember.Email,
                //    Phone = createMember.Phone,
                //    DateOfBirth = createMember.DateOfBirth,
                //    Gender = createMember.Gender,
                //    Address = new Address()
                //    {
                //        BulidingNumber = createMember.BuildingNumber,
                //        Street = createMember.Street,
                //        City = createMember.City
                //    },
                //    HealthRecord = new HealthRecord()
                //    {
                //        Height = createMember.HealthRecordViewModel.Height,
                //        Weight = createMember.HealthRecordViewModel.Wigth,
                //        BloodType = createMember.HealthRecordViewModel.BloodType,
                //        Note = createMember.HealthRecordViewModel.Note
                //    },
                //};
                var MappedMember = _mapper.Map<CreateMemberViewModel, Member>(CreateMember);
                MappedMember.Photo = PhotoName;
                _unitOfWork.GetRepository<Member>().Add(MappedMember);
                var IsCreated = _unitOfWork.SaveChanges() > 0;
               if(!IsCreated)
                {
                    _attachmentService.DeletePhoto(PhotoName, "Members");
                }
                return IsCreated;
            }
            catch
            {
                return false;
            }

        }

        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var Members = _unitOfWork.GetRepository<Member>().GetAll();
            if (Members == null || !Members.Any())
                return Enumerable.Empty<MemberViewModel>();
            #region Way 1 
            //1- Manul Mapping
            //var MemberViewModels = new List<MemberViewModel>();
            //foreach (var Member in Members)
            //{
            //    var memberViewModel = new MemberViewModel
            //    {
            //        Id = Member.Id,
            //        Name = Member.Name,
            //        Photo = Member.Photo,
            //        Email = Member.Email,
            //        Phone = Member.Phone,
            //        Gender = Member.Gender.ToString()
            //    };
            //    MemberViewModels.Add(memberViewModel);
            //}
            #endregion

            #region Way 2 
            //var MemberViewModels = Members.Select(X => new MemberViewModel
            //{
            //    Id = X.Id,
            //    Name = X.Name,
            //    Photo = X.Photo,

            //    Email = X.Email,
            //    Phone = X.Phone,
            //    Gender = X.Gender.ToString()
            //});
            #endregion
            var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(Members);
            return MemberViewModels;
        }

        public MemberViewModel? GetMemberDetials(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null)
                return null;
            //var ViewModel = new MemberViewModel()
            //{
            //    Id = member.Id,
            //    Name = member.Name,
            //    Photo = member.Photo,
            //    Email = member.Email,
            //    Phone = member.Phone,
            //    Gender = member.Gender.ToString(),
            //    DateOfBirth = member.DateOfBirth.ToShortDateString(),
            //    Address = $"{member.Address.BulidingNumber} - {member.Address.Street} - {member.Address.City}",

            //};
            var memberViewModel=_mapper.Map<MemberViewModel>(member);

            var ActivememberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.MemberId == MemberId && X.Status == "Active").FirstOrDefault();
            
            if (ActivememberShip != null)
            {
                memberViewModel.MemberShipStartDate = ActivememberShip.CreatedAt.ToShortDateString();
                memberViewModel.MemberShipEndDate = ActivememberShip.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActivememberShip.PlanId);
                memberViewModel.PlanName = plan?.Name;
            }
            return memberViewModel;
        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int MemberId)
        {
            var MemberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(MemberId);
            if (MemberHealthRecord == null)
                return null;
            //return new HealthRecordViewModel()
            //{
            //    Height = MemberHealthRecord.Height,
            //    Weight = MemberHealthRecord.Weight,
            //    BloodType = MemberHealthRecord.BloodType,
            //    Note = MemberHealthRecord.Note
            //};
            return _mapper.Map<HealthRecordViewModel>(MemberHealthRecord);
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int MemberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(MemberId);
            if (member == null)
                return null;
            //return new MemberToUpdateViewModel()
            //{
            //    Photo = member.Photo,
            //    Phone = member.Phone,
            //    Email = member.Email,
            //    Name = member.Name,
            //    BuildingNumber = member.Address.BulidingNumber,
            //    Street = member.Address.Street,
            //    City = member.Address.City,
            //};
            return _mapper.Map<MemberToUpdateViewModel>(member);
        }

        public bool UpdateMemberDetails(int MemberId, MemberToUpdateViewModel memberToUpdate)
        {
            try
            {
              var EmailExist = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Email == memberToUpdate.Email && X.Id != MemberId).Any();
                var PhoneExist = _unitOfWork.GetRepository<Member>()
                    .GetAll(X => X.Phone == memberToUpdate.Phone && X.Id != MemberId).Any();
                if (EmailExist || PhoneExist) return false;


                var MemberRepo= _unitOfWork.GetRepository<Member>();
                var member= MemberRepo.GetById(MemberId);
                if(member == null) return false;


                //member.Email = memberToUpdate.Email;
                //member.Phone = memberToUpdate.Phone;
                //member.Name = memberToUpdate.Name;
                //member.Address.BulidingNumber = memberToUpdate.BuildingNumber;
                //member.Address.Street = memberToUpdate.Street;
                //member.Address.City = memberToUpdate.City;
                //member.UpdatedAt = DateTime.Now;

                _mapper.Map(memberToUpdate,member);

                MemberRepo.Update(member);
                return _unitOfWork.SaveChanges() > 0;


            }

            catch
            {
                return false;
            }
        }

        #region HelperMethods
        private bool IsEmailExist(string Email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Email == Email ).Any();
        }
        private bool IsPhoneExist(string Phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(X => X.Phone == Phone ).Any();
        }
        #endregion

        public bool RemoveMember(int MemberId)
        {
            try
            {
                var MemberRepo = _unitOfWork.GetRepository<Member>();   
                var member = MemberRepo.GetById(MemberId);
                if (member == null) return false;

                var SessionIds = _unitOfWork.GetRepository<MemberSession>()
                    .GetAll(X => X.MemberId == MemberId).Select(X=>X.SessionId);

                var HasActiveMemberSession = _unitOfWork.GetRepository<Session>()
                    .GetAll(X => SessionIds.Contains(X.Id) && X.StartDate > DateTime.Now).Any();

                if (HasActiveMemberSession) return false;

                var MemberShipRepo= _unitOfWork.GetRepository<MemberShip>();
                var MemberShips = MemberShipRepo.GetAll(X => X.MemberId == MemberId);
                if (MemberShips.Any()) {
                    foreach (var memberShip in MemberShips)
                        MemberShipRepo.Delete(memberShip);
                }
                MemberRepo.Delete(member) ;
               var IsDeleted=_unitOfWork.SaveChanges() > 0;
                if (IsDeleted)
                {
                    _attachmentService.DeletePhoto(member.Photo, "Members");
                }
                return IsDeleted;
            }
            catch
            {
                return false;
            }
        }

       
    }

}
