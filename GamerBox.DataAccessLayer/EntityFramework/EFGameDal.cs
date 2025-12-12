using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using GamerBox.DataAccessLayer.Repositories;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GamerBox.DataAccessLayer.EntityFramework
{
    public class EFGameDal : GenericRepository<Game>, IGameDal
    {

        public async Task<List<Game>> GetByGenresAsync(List<string> genres, int count)
        {
            // SQL Karşılığı: SELECT TOP(@count) * FROM Games WHERE Genre IN (@genres) ...
            return await _context.Games
                .Where(g => genres.Contains(g.Genre)) // Türü, kullanıcının sevdikleri arasındaysa al
                .OrderByDescending(g => g.AverageRating ?? 0) // Puana göre sırala
                .Take(count) // İstenen adet kadar al
                .ToListAsync();
        }
        public EFGameDal(GamerBoxContext context) : base(context)
        {
            
        }
        public async Task<List<Game>> GetByRatingAsync(int count)
        {
            return await _context.Games
                // Veritabanı tarafında AverageRating'e göre çoktan aza sıralar
                // (?? 0) kısmı: Puanı null olanları 0 kabul et demektir.
                .OrderByDescending(g => g.AverageRating ?? 0)

                // Sadece istenen sayı kadar (örn: 10) veri alır (SQL TOP komutu)
                .Take(count)

                // Veriyi veritabanından çekip listeye çevirir
                .ToListAsync();
        }

        public List<Game> GetTopRatedGames(int count)
        {
            return _context.Games
                .Include(g => g.Ratings)
                .OrderByDescending(g => g.Ratings.Count)
                .Take(count)
                .ToList();
        }
        public List<Game> GetAllGames()
        {
            return _context.Games.ToList();
        }

        public List<Game> GetAllGamesForUser(int userId)
        {
            return _context.Games
                .Where(g => g.UserId == userId) // UserId farklıysa burayı düzenle
                .ToList();
        }



        public List<Game> GetRecommendedGamesForUser(int userId)
        {


            var postedGameIds = _context.Posts
                .Where(p => p.UserId == userId && p.GameId != null)
                .Select(p => p.GameId.Value)
                .ToList();

            var ratedGameIds = _context.Ratings
                .Where(r => r.UserId == userId)
                .Select(r => r.GameId)
                .ToList();



            var interactedGameIds = postedGameIds
                .Union(ratedGameIds)
                .ToList();


            return _context.Games
                .Include(g => g.Ratings)
                .Where(g => !interactedGameIds.Contains(g.Id))
                .OrderByDescending(g => g.Ratings.Count)
                .Take(5)
                .ToList();
        }
    }
}