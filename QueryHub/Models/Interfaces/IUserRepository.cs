using Project.Models;

namespace Project.Models.Interfaces
{
    public interface IUserRepository
    {
        public Task<AppUser> GetUserByUsernameAsync(string UserName);
        public Task<List<AppUser>> GetAllUsersAsync();

        public Task<AppUser> UpdateRatingAsync();
    }
}
