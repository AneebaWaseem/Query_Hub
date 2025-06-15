using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class AppUser : IdentityUser
    {
        public int Rating { get; set; }
        public int Streak { get; set; }

        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
    }

}
