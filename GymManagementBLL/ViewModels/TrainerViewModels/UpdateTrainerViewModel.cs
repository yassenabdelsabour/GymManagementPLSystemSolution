using GymManagementDAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.TrainerViewModels
{
   public class UpdateTrainerViewModel
    {
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Phone is Required")]
        [RegularExpression(@"^(010 || 011 || 012 || 015)\d{11}$", ErrorMessage = "Phone Number Must Be Vaild Egyption Phone Number.")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Date Of Birth Is Required ")]
        [Range(1, int.MaxValue, ErrorMessage = "Building Number Must be greater than 0")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street IS Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Street Must be Between 2 and 100 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Street  must contain only letters and spaces.")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "City IS Required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "City Must be Between 2 and 100 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City must contain only letters and spaces.")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = "Specialties IS Required")]
        public Specialties Specialties { get; set; } 
    }
}
