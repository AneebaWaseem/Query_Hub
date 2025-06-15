using Project.Data;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Project.Models.Interfaces;

namespace Project.Models.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetAllQuestionsAsync()
        {
            var questions = await _context.Questions.Include(q => q.AppUser).Include(q => q.Tags).ToListAsync();
            Console.WriteLine("Fetched questions count: " + questions.Count);
            return questions;
        }

        public async Task<Question> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions.Include(q => q.Answers).ThenInclude(a => a.AppUser).Include(q => q.Tags).Include(q => q.AppUser).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task AddQuestionAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Tag>> GetTagsByNameAsync(List<string> tagNames)
        {
            if (tagNames == null)
                return new List<Tag>();

            return await _context.Tags.Where(t => tagNames.Contains(t.Name)).ToListAsync();
        }

        public async Task<List<Tag>> getAllTagsAsync()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Question> GetQuestionWithUserAsync(int id)
        {
            return await _context.Questions.Include(q => q.AppUser).FirstOrDefaultAsync(q => q.Id == id);
        }

        public async Task<List<Question>> GetQuestionsByTagNameAsync(string tagName)
        {
            return await _context.Questions.Include(q => q.AppUser).Include(q => q.Answers).Include(q => q.Tags).Where(q => q.Tags.Any(t => t.Name == tagName)).ToListAsync();
        }

        public async Task<Question> DeleteQuestionAsync(int id)
        {
            var question = await GetQuestionByIdAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }

            return question;
        }

        public async Task<List<Question>> GetQuestionsByUserAsync(string user)
        {
            return await _context.Questions.Include(q => q.AppUser).Include(q => q.Tags).Include(q => q.Answers).ThenInclude(a => a.AppUser).Where(q => q.AppUser.UserName == user).ToListAsync();
        }


        public async Task<List<Question>> GetQuestionsByAnsUserAsync(string ansUser)
        {
            return await _context.Questions.Include(q => q.AppUser).Include(q => q.Tags).Include(q => q.Answers).ThenInclude(a => a.AppUser).Where(q => q.Answers.Any(a => a.AppUser.UserName == ansUser)).ToListAsync();
        }
    }
}
