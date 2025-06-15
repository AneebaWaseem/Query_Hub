using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<Answer> Answers { get; set; }
        public List<Tag> Tags { get; set; }
    }

}
