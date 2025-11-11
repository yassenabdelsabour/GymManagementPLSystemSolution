using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Service.InterFaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreateTrainerViewModel createTrainer);
        bool UpdateTrainerDetails(int TrainerId, TrainerToUpdateViewModel UpdatedTrainer);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int TrainerId );
        bool RemoveTrainer(int TrainerId);
        TrainerViewModel? GetTrainerDetails(int TrainerId);

    }
}
