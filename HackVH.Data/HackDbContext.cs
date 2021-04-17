using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HackVH.Data
{
    public class HackDbContext : IdentityDbContext<IdentityUser>
    {
        public HackDbContext(DbContextOptions<HackDbContext> options) : base(options)
        {
        }
    }
}