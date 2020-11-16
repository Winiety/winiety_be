using Shared.Core.BaseModels.Entities;

namespace Profile.Core.Models.Entities
{
    public class UserProfile : BaseEntity
    {
        public int UserId { get; set; }

        public int BirthYear { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string FlatNumber { get; set; }
        public string Zip { get; set; }
    }
}
