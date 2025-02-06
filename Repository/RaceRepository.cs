using Microsoft.EntityFrameworkCore;
using TeddySmith.Data;
using TeddySmith.Interfaces;
using TeddySmith.Models;

namespace TeddySmith.Repository
{
    public class RaceRepository : IRaceRepository
    {
        private readonly MyDbContext _context;

        public RaceRepository(MyDbContext context)
        {
            this._context = context;
        }
        public bool Add(Race race)
        {
            _context.Add(race);
            return Save();
        }

        public bool Delete(Race race)
        {
            _context.Remove(race);
            return Save();
        }

        public async Task<IEnumerable<Race>> GetAll()
        {
            return await _context.Races.ToListAsync();
        }

        public async Task<Race> GetByIdAsync(int id)
        {
            return await _context.Races.Include(i=>i.Address).FirstOrDefaultAsync(i=>i.Id==id);
        }

        public async Task<IEnumerable<Race>> GetRaceByCity(string city)
        {
            return await _context.Races.Where(i => i.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved=_context.SaveChanges();
            return saved>0?true:false;  
        }

        public bool Update(Race race)
        {
            _context.Update(race);
            return Save();
        }
    }
}
