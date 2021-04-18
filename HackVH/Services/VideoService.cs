using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HackVH.Data;
using HackVH.Models;
using HackVH.Services.Interfaces;

namespace HackVH.Services
{
    public class VideoService : IVideoService
    {
        private readonly HackDbContext _context;

        public VideoService(HackDbContext context)
        {
            _context = context;
        }
        
        public Task<IEnumerable<Video>> GetAllVideosAsync()
        {
            return Task.FromResult(_context.Videos.AsEnumerable());
        }

        public Task<IEnumerable<Video>> GetVideosForUnitAsync(Unit unit)
        {
            var ids = unit.Videos.Select(v => v.Id);
            return Task.FromResult(_context.Videos.Where(v => ids.Contains(v.Id)).AsEnumerable());
        }

        public async Task<Video> FindByIdAsync(int id)
        {
            return await _context.Videos.FindAsync(id);
        }

        public async Task<IEnumerable<Video>> SearchVideosAsync(string query)
        {
            if (int.TryParse(query, out var id))
            {
                return new [] { await _context.Videos.FindAsync(id) };
            }
            return _context.Videos.Where(v => v.Name.Contains(query) || query.Contains(v.Name)).AsEnumerable();
        }

        public async Task CreateVideoAsync(VideoModel model)
        {
            await _context.Videos.AddAsync(new Video
                {Name = model.Name, UserId = model.User.Id, VideoUrl = model.VideoUrl});
            await _context.SaveChangesAsync();
        }
    }
}