using TeddySmith.Data.Enum;
using TeddySmith.Models;

namespace TeddySmith.ViewModels
{
    public class EditRaceViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public String? URL { get; set; }
        public RaceCategory ClubCategory { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public RaceCategory RaceCategory { get; internal set; }
        public string? AppUserId { get; set; }

    }
}
