using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using GamerBox.DataAccessLayer.Repositories;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GamerBox.DataAccessLayer.EntityFramework
{
    public class EFRatingDal : GenericRepository<Rating>, IRatingDal
    {
        

        public EFRatingDal(GamerBoxContext context) : base(context)
        {
            
        }

        public async Task<List<Rating>> GetByGameIdAsync(int gameId)
        {
            return await _context.Ratings
                .Where(r => r.GameId == gameId)
                //  Eğer puanı kimin verdiğini de ekranda gösterirsek ilerde
                // .Include(r => r.User) // Bunu eklersen User tablosunu da joinler
                .OrderByDescending(r => r.RatedAt) // En yeniden en eskiye
                .ToListAsync();
        }


        public async Task<double> GetAverageRatingForGameAsync(int gameId)
        {
            // Veriyi çekmeden (ToList yapmadan) doğrudan veritabanında hesaplatıyoruz.

      
            var average = await _context.Ratings
                .Where(r => r.GameId == gameId)
                .AverageAsync(r => (double?)r.Score);
            return average ?? 0.0;
        }


        public async Task<bool> HasUserRatedGameAsync(int userId, int gameId)
        {
            return await _context.Ratings
                .AnyAsync(r => r.UserId == userId && r.GameId == gameId);
        }

        public Task<List<Rating>> GetByUserIdASync(int userId)
        {
            return _context.Ratings
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.RatedAt)
                .ToListAsync();
        }
    }
}