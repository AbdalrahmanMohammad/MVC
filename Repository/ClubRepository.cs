using Microsoft.EntityFrameworkCore;
using TeddySmith.Data;
using TeddySmith.Interfaces;
using TeddySmith.Models;

namespace TeddySmith.Repository
{
    public class ClubRepository : IClubRepository
    {
        private readonly MyDbContext _context;

        public ClubRepository(MyDbContext context)
        {
            this._context = context;
        }
        public bool Add(Club club)
        {
            _context.Add(club);
            return Save();
        }

        public bool Delete(Club club)
        {
            _context.Remove(club);
            return Save();
        }

        public async Task<IEnumerable<Club>> GetAll()
        {
            return await _context.Clubs.ToListAsync();
        }

        public async Task<Club> GetByIdAsync(int id)
        {
            return await _context.Clubs.Include(i=>i.Address).FirstOrDefaultAsync(i=>i.Id==id);
        }

        public async Task<Club> GetByIdAsyncNoTracking(int id)
        {
            return await _context.Clubs.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Club>> GetClubByCity(string city)
        {
            return await _context.Clubs.Where(i => i.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved>0?true:false;  
        }

        public bool Update(Club club)
        {
            _context.Update(club);
            return Save();
        }
    }
}
