using Project.Models;

namespace Project.Models.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetAllQuestionsAsync(); 
        Task<Question> GetQuestionByIdAsync(int id); 
        Task AddQuestionAsync(Question question);
        Task<List<Tag>> GetTagsByNameAsync(List<string> tagNames);
        Task<List<Tag>> getAllTagsAsync();
        Task<Question> GetQuestionWithUserAsync(int id);
        Task<List<Question>> GetQuestionsByTagNameAsync(string tagName);
        Task<Question> DeleteQuestionAsync(int id);
        Task<List<Question>> GetQuestionsByUserAsync(string user);
        Task<List<Question>> GetQuestionsByAnsUserAsync(string user);
    }
}
