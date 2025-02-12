using Microsoft.EntityFrameworkCore;
using TeddySmith.Data;
using TeddySmith.Interfaces;
using TeddySmith.Models;

namespace TeddySmith.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MyDbContext context;

        public UserRepository(MyDbContext context)
        {
            this.context = context;
        }
        public bool Add(AppUser user)
        {
            throw new NotImplementedException();
        }

        public bool Delete(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppUser>> GetAllUsers()
        {
            return await context.Users.ToListAsync();
        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await context.Users
                .Include(u => u.Address) 
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public bool Save()
        {
            var saved = context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(AppUser user)
        {
            context.Update(user);
            return Save();
        }
    }
}
