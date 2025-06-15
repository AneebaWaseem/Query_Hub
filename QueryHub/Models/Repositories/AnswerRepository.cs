using Project.Data;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Project.Models.Interfaces;

namespace Project.Models.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public AnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Answer>> GetAnswersByQuestionIdAsync(int qid)
        {
            var ans = await _context.Answers.Where(a => a.QuestionId == qid).ToListAsync();
            return ans;
        }

        public async Task AddAnswerAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
        }

        public async Task<Answer> GetAnswerByIdAsync(int id)
        {
            return await _context.Answers.FindAsync(id);
        }

        public async Task DeleteAnswerAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
            }
        }
    }
}
