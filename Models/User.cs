using Portfolium_Back.Models.Entities;

namespace Portfolium_Back.Models
{
    public class User : Entity
    {
        public Guid GuidID { get; set; }

        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String? Role { get; set; }
    }
}