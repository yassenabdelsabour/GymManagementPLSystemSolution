using GymManagementBLL.Service.Classes;
using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.PlanViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;
        public PlanController(IPlanService planService) 
        {
            _planService = planService;
        }
        public IActionResult Index()
        {
            var Plans = _planService.GetAllPlans();
            return View(Plans);
        }

        #region Plan Details
        public ActionResult Details(int id ,string ViewName= "Details")
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanDetails(id);

            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(ViewName,plan);
        }
        #endregion

        #region Edit Plan
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id";
                return RedirectToAction(nameof(Index));
            }
            var plan = _planService.GetPlanToUpdate(id);

            if (plan == null)
            {
                TempData["ErrorMessage"] = "Plan Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }
        [HttpPost]
        public ActionResult Edit([FromRoute]int id,UpdatePlanViewModel updatedPlan)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData","Check Data Validation");
                return View( updatedPlan);
            }
                
            var Result = _planService.UpdatePlan(id,updatedPlan);

            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Updated Successfully";
              
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Update , check Name Uniqueness ";
                
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Activate/Deactivate Plan
        [HttpPost]
        public ActionResult Activate(int id)
        {
            
            var Result = _planService.ToggleStatus(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Plan Activation Toggled Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Plan Failed To Toggle Activation ";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
