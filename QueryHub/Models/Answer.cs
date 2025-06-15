using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string UserId { get; set; }
        public AppUser AppUser { get; set; }
    }

}
