using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HackVH.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace HackVH.Services
{
    public class MoneyService : IMoneyService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public MoneyService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<double> GetBalanceAsync(IdentityUser user)
        {
            var moneyClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Money");
            if (moneyClaim == null)
                return 0;

            if (!double.TryParse(moneyClaim.Value, out var money))
                return 0;

            return money;
        }
        
        public async Task SetBalanceAsync(IdentityUser user, double balance)
        {
            var moneyClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "Money");
            if (moneyClaim == null)
            {
                await _userManager.AddClaimAsync(user, new Claim("Money", 
                    balance.ToString(CultureInfo.InvariantCulture)));
                return;
            }

            await _userManager.ReplaceClaimAsync(user, moneyClaim,
                new Claim("Money", balance.ToString(CultureInfo.InvariantCulture)));
        }

        public async Task AddBalanceAsync(IdentityUser user, double balance)
        {
            await SetBalanceAsync(user, await GetBalanceAsync(user) + balance);
        }
    }
}