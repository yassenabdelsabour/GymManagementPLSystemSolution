using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using Microsoft.AspNetCore.Mvc;
   
namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        #region Get All Member
        public ActionResult Index()
        {
            var members = _memberService.GetAllMembers();
            return View(members);
        }

        #endregion

        #region GetMember Data
        public ActionResult MemberDetalis(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id Becouse Id of Member can not be 0 or Negative Member";

                return RedirectToAction(nameof(Index));

            }
               
            var Member = _memberService.GetMemberDetials(id);

            if(Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Fount";

                return RedirectToAction(nameof(Index));

            }

            return View(Member);
        }

        public ActionResult HealthRecordDetalis(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid HealthRecord Becouse Id of HealthRecord can not be 0 or Negative Member";

                return RedirectToAction(nameof(Index));

            }

            var HealthRecord = _memberService.GetMemberHealthRecordDetails(id);
            if (HealthRecord == null)
            {
                TempData["ErrorMessage"] = "HealthRecord Not Fount";

                return RedirectToAction(nameof(Index));
            }
            return View(HealthRecord);
        }
        #endregion

        #region Add Member
       
        public ActionResult Create(int id)
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateMember(CreateMemberViewModel CreateMember)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataInValid", "Check Data And Missing Fields");
                return View(nameof(Create), CreateMember);
            }

           bool Result = _memberService.CreateMember(CreateMember);

            if (Result)
            {
                TempData["SuccessMessage"] = "Member Created Successfuly ";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Create , check Phone And Email ";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Update Member
        public ActionResult MemberEdit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id Becouse Id of Member can not be 0 or Negative Member";
                return RedirectToAction(nameof(Index));
            }
            var MemberToUpdate = _memberService.GetMemberToUpdate(id);

            if (MemberToUpdate == null)
            {
                TempData["ErrorMessage"] = "Member Not Fount";
                return RedirectToAction(nameof(Index));
            }
            return View(MemberToUpdate);
        }

        [HttpPost]
        public ActionResult MemberEdit([FromRoute]int id ,MemberToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(viewModel);
           
            var Result = _memberService.UpdateMemberDetails(id, viewModel);


            if (Result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfuly ";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Update , check Phone And Email ";
            }
            return RedirectToAction(nameof(Index));
        }


        #endregion

        #region Delete Member
        public ActionResult MemberDelete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Member Id Becouse Id of Member can not be 0 or Negative Member";
                return RedirectToAction(nameof(Index));
            }
            var Member = _memberService.GetMemberDetials(id);

            if (Member == null)
            {
                TempData["ErrorMessage"] = "Member Not Found ";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MemberId = id;
            return View();
            
        }

        [HttpPost]
        public ActionResult MemberDeleteConfirmed(int id)
        {
            var Result = _memberService.RemoveMember(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfuly ";
            }
            else
            {
                TempData["ErrorMessage"] = "Member Failed To Delete ";
            }
            return RedirectToAction(nameof(Index));
        }




        #endregion
    }
}
