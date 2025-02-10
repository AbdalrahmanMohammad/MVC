using TeddySmith.Data.Enum;
using TeddySmith.Models;

namespace TeddySmith.ViewModels
{
    public class EditClubViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile? Image { get; set; }
        public String? URL { get; set; }
        public ClubCategory ClubCategory { get; set; }
        public int AddressId { get; set; }
        public Address Address {  get; set; }
        public string? AppUserId { get; set; }

    }
}
