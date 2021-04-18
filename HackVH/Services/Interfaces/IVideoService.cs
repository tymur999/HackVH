using System.Collections.Generic;
using System.Threading.Tasks;
using HackVH.Data;
using HackVH.Models;

namespace HackVH.Services.Interfaces
{
    public interface IVideoService
    {
        public Task<IEnumerable<Video>> GetAllVideosAsync();
        public Task<IEnumerable<Video>> GetVideosForUnitAsync(Unit unit);
        public Task<Video> FindByIdAsync(int id);
        public Task<IEnumerable<Video>> SearchVideosAsync(string query);

        public Task CreateVideoAsync(VideoModel model);
    }
}