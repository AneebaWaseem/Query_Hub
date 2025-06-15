using Project.Models;

namespace Project.Models.Interfaces
{
    public interface IAnswerRepository
    {
        Task<List<Answer>> GetAnswersByQuestionIdAsync(int questionId); 
        Task AddAnswerAsync(Answer answer);
        Task<Answer> GetAnswerByIdAsync(int id);
        Task DeleteAnswerAsync(int id);
    }
}
