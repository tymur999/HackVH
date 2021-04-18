using System.Collections.Generic;
using System.Threading.Tasks;
using HackVH.Data;
using Microsoft.AspNetCore.Identity;

namespace HackVH.Services.Interfaces
{
    public interface IQuizService
    {
        public Task<IEnumerable<Quiz>> GetQuizzesForUnitAsync(Unit unit);
        public Task<bool> IsCompletedQuizAsync(IdentityUser user, Quiz quiz);
        public Task<Quiz> FindByIdAsync(int id);
    }
}