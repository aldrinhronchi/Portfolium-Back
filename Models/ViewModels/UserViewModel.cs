using System.ComponentModel.DataAnnotations;

namespace Portfolium_Back.Models.ViewModels
{
    public class UserViewModel : IViewModel<User, UserViewModel>
    {
        public Guid? GuidID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? Role { get; set; }
        public string? Token { get; set; }
        public UserViewModel() { }
        public UserViewModel(User user)
        {
            GuidID = user.GuidID;
            Name = user.Name;
            Email = user.Email;
            Password = user.Password;
        }

        public UserViewModel ToViewModel(User entity)
        {
            return new UserViewModel(entity);
        }
        public User ToEntity()
        {
            return new User
            {
                GuidID = GuidID ?? Guid.NewGuid(),
                Name = Name,
                Email = Email,
                Password = Password,
                Role = Role
            };
        }

    }
}