using System.ComponentModel.DataAnnotations;

namespace Portfolium_Back.Models.ViewModels
{
    public class UserViewModel
    {
        public Guid GuidID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}