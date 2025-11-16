using System.ComponentModel.DataAnnotations;

namespace DMnDBCS.UI.Models
{
    public class UserInfoViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(100)]
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; } = string.Empty;

        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; } = string.Empty;

        [Required]
        public string Address { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateOnly DateOfBirth { get; set; }

        [DataType(DataType.ImageUrl)]
        public string? ProfilePicture { get; set; }

        [Display(Name = "Profile Picture")]
        [DataType(DataType.Upload)]
        public IFormFile? ProfilePictureFile { get; set; }
    }
}
