using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;
        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var Sessions = _sessionService.GetAllSessions();
            return View(Sessions);
        }

        #region Details Action
        public ActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
             return View(session);

        }
        #endregion

        #region Create Action
        public ActionResult Create()
        {
            LoadTrainerDropdowns();
            LoadCategorDropdowns();
            return View();

        }
        [HttpPost]
        public ActionResult Create(CreateSessionViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainerDropdowns();
                LoadCategorDropdowns();
                return View(viewModel);
            }
            var Result = _sessionService.CreateSession(viewModel);

            if (Result)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create session.";
                LoadTrainerDropdowns();
                LoadCategorDropdowns();
                return View(viewModel);
            }
            

        }
        #endregion


        #region Helpers
        private void LoadTrainerDropdowns()
        {
            //var Categories = _sessionService.GetAllCategoriesForDropdown();
            //ViewBag.Categories = new SelectList(Categories, "Id", "Name");
            var Trainers = _sessionService.GetAllTrainersForDropdown();
            ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }

        private void LoadCategorDropdowns()
        {
            var Categories = _sessionService.GetAllCategoriesForDropdown();
            ViewBag.Categories = new SelectList(Categories, "Id", "Name");
            //var Trainers = _sessionService.GetAllTrainersForDropdown();
            //ViewBag.Trainers = new SelectList(Trainers, "Id", "Name");
        }
        #endregion


        #region Edit Action
        public ActionResult Edit(int id)
        {
            if(id <= 0)
            { 
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionToUpdate(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            LoadTrainerDropdowns();
            return View(session);
        }

        [HttpPost]
        public ActionResult Edit([FromRoute] int id , UpdateSessionViewModel updateSession)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainerDropdowns();
                return View (updateSession);
            }

            var Result = _sessionService.UpdateSession(updateSession,id);

            if(Result)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
              
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update session.";
                
          
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete Action
        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Session Id.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);

            if (session == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.SessionId = id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var Result = _sessionService.RemoveSession(id);

            if (Result)
            {
                TempData["SuccessMessage"] = "Session deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete session.";
            }
            return RedirectToAction(nameof(Index));
        }
        #endregion
    }
}
