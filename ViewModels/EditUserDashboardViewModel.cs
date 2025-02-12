using TeddySmith.Models;

namespace TeddySmith.ViewModels
{
    public class EditUserDashboardViewModel
    {
        public string Id { get; set; }
        public int? Pace { get; set; }
        public int? Mileage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public int? AddressId { get; set; }
        public Address Address { get; set; }
        public IFormFile? Image { get; set; }
    }
}
