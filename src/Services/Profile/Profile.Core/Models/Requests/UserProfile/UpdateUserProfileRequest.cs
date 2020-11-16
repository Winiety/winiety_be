namespace Profile.Core.Models.Requests.UserProfile
{
    public class UpdateUserProfileRequest
    {
        public int BirthYear { get; set; }
        public string Phone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string FlatNumber { get; set; }
        public string Zip { get; set; }
    }
}
