using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackVH.Data;
using HackVH.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HackVH.Services
{
    public class QuizService : IQuizService
    {
        private readonly HackDbContext _context;

        public QuizService(HackDbContext context)
        {
            _context = context;
        }
        
        public Task<IEnumerable<Quiz>> GetQuizzesForUnitAsync(Unit unit)
        {
            var ids = unit.Quizzes.Select(q => q.Id);
            return Task.FromResult(_context.Quizzes.Where(q => ids.Contains(q.Id)).AsEnumerable());
        }
        
        public async Task<bool> IsCompletedQuizAsync(IdentityUser user, Quiz quiz)
        {
            var attempt = await _context.QuizAttempts.FirstOrDefaultAsync(q => q.Quiz.Id == quiz.Id);
            return attempt != null;
        }

        public async Task<Quiz> FindByIdAsync(int id)
        {
            return await _context.Quizzes.FindAsync(id);
        }
    }
}