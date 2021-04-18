using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HackVH.Services.Interfaces
{
    public interface IMoneyService
    {
        public Task<double> GetBalanceAsync(IdentityUser user);
        public Task SetBalanceAsync(IdentityUser user, double balance);
        public Task AddBalanceAsync(IdentityUser user, double balance);
    }
}