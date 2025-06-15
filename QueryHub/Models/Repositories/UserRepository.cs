using Project.Models;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models.Interfaces;

namespace Project.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<AppUser?> GetUserByUsernameAsync(string UserName)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == UserName);
        }

        public async Task<List<AppUser>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<AppUser> UpdateRatingAsync()
        {
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (string.IsNullOrEmpty(username))
                return null;

            var user = await GetUserByUsernameAsync(username);

            if (user == null)
                return null;

            user.Rating += 10;

            await _context.SaveChangesAsync();

            return user;
        }
    }
}
