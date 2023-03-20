namespace Portfolium_Back.Models
{
    public class Entity
    {
        public Int32 ID { get; set; }
        public Guid GuidID { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateUpdated { get; set; }
        public Boolean IsActive { get; set; }
        public String? UserCreated { get; set; }
        public String? UserUpdated { get; set; }
    }
}