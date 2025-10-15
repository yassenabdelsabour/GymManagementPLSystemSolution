using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.ViewModels.MemberViewModel
{
    internal class MemberToUpdateViewModel
    {
        [Required(ErrorMessage = "Name is Required"), StringLength(50, MinimumLength = 2, ErrorMessage = "Name Must be Between 2 and 50 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name must contain only letters and spaces.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Email Must be Between 5 and 100 Char")]
        [DataType(DataType.EmailAddress), EmailAddress(ErrorMessage = "Invalid Email Format")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Phone is Required")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(010 || 011 || 012 || 015)\d{8}$", ErrorMessage = "Phone Number Must Be Vaild Egyption Phone Number.")]
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "BuildingNumber IS Required")]
        [Range(1, 1000, ErrorMessage = "Building Number Must be Between 1 and 1000 ")]
        public int BuildingNumber { get; set; }

        [Required(ErrorMessage = "Street IS Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Street Must be Between 2 and 30 Char")]
        public string Street { get; set; } = null!;
        [Required(ErrorMessage = "City IS Required")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "City Must be Between 2 and 30 Char")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "City must contain only letters and spaces.")]
        public string City { get; set; } = null!;
        [Required(ErrorMessage = "HealthRecordViewModel IS Required")]
        public string? Photo { get; set; }

    }
}
