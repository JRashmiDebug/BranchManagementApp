using System.ComponentModel.DataAnnotations;

namespace BranchManagementApp.Models
{
    public class Branch
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Branch Name is required")]
        [StringLength(100, ErrorMessage = "Branch Name cannot exceed 100 characters")]
        public string BranchName { get; set; }

        [Required(ErrorMessage = "Person Name is required")]
        [StringLength(100, ErrorMessage = "Person Name cannot exceed 100 characters")]
        public string PersonName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
        public int Age { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile Number must be 10 digits")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Complete Address is required")]
        public string CompleteAddress { get; set; }

        [Required(ErrorMessage = "Profession is required")]
        public string Profession { get; set; }
    }
}
