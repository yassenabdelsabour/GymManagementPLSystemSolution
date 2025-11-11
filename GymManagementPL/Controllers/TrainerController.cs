using GymManagementBLL.Service.InterFaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;
        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }

        #region Get All Trainers
        public ActionResult Index()
        {
            var trainers = _trainerService.GetAllTrainers();
            return View(trainers);

        }
        #endregion

        #region Create Trainer
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            if (!ModelState.IsValid)

                return View(nameof(Create), createTrainer);

            var Result = _trainerService.CreateTrainer(createTrainer);

            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Create , check Phone And Email ";
                return View(createTrainer);
            }
            }
        #endregion

        #region Trainer Details

        public ActionResult TrainerDetails(int id)
        {
            
            var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }
        #endregion

        #region Edit Trainer
        public ActionResult Edit(int id)
        {

            var trainer = _trainerService.GetTrainerToUpdate(id);
            if (trainer == null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found";
                return RedirectToAction(nameof(Index));
            }
            
            return View(trainer);

        }


        [HttpPost]
        public ActionResult Edit([FromRoute] int id, TrainerToUpdateViewModel UpdatedTrainer)
        {
            if (!ModelState.IsValid)
                return View(UpdatedTrainer);

            var Result = _trainerService.UpdateTrainerDetails(id, UpdatedTrainer);

            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Updated Successfully";
                
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Update , check Phone And Email ";
                
            }
           
            return RedirectToAction(nameof(Index));

        }
        #endregion

        #region Delete Trainer
        public ActionResult TrainerDelete(int id) {
           var trainer = _trainerService.GetTrainerDetails(id);
            if (trainer == null)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.TrainerId = id;
            return View();
        }
        [HttpPost]
        public ActionResult DeleteTrainerConfirmed(int id)
        {
            var Result = _trainerService.RemoveTrainer(id);
            if (Result)
            {
                TempData["SuccessMessage"] = "Trainer Deleted Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = "Trainer Failed To Delete ";
            }
            return RedirectToAction(nameof(Index));
        }

        #endregion
    }

}

