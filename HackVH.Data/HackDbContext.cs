using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HackVH.Data
{
    public class HackDbContext : IdentityDbContext<IdentityUser>
    {
        public HackDbContext(DbContextOptions<HackDbContext> options) : base(options) { }
        
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<StockOrder> StockOrders { get; set; }
        public DbSet<Video> Videos { get; set; }
        public DbSet<Unit> Units { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Quiz>(o =>
                {
                    o.Property(q => q.Questions)
                        .HasConversion(
                            j => JsonSerializer.Serialize(j, null),
                            j => JsonSerializer.Deserialize<IEnumerable<Question>>(j, null)
                        );
                });

            builder.Entity<Unit>(o =>
            {
                o.Property(q => q.Quizzes)
                    .HasConversion(
                        j => JsonSerializer.Serialize(j, null),
                        j => JsonSerializer.Deserialize<IEnumerable<Quiz>>(j, null));

                o.Property(q => q.Videos)
                    .HasConversion(
                        j => JsonSerializer.Serialize(j, null),
                        j => JsonSerializer.Deserialize<IEnumerable<Video>>(j, null));
            });
            base.OnModelCreating(builder);
        }
    }
}