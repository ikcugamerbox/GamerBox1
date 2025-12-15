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
        public async Task<List<Game>> GetFilteredGamesAsync(string searchText, string genre, int minRating, int priceFilter, int sortOrder)
        {
            // Sorguyu başlatıyoruz (Henüz veritabanına gitmedi)
            var query = _context.Games.AsQueryable();

            // 1. Arama Metni Filtresi
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(g => g.Title.Contains(searchText));
            }

            // 2. Tür Filtresi
            if (!string.IsNullOrEmpty(genre) && genre != "Tümü")
            {
                query = query.Where(g => g.Genre == genre);
            }

            // 3. Puan Filtresi (minRating: UI'daki Index'e göre mantık kuruyoruz)
            // Index 1: 4+, Index 2: 3+, Index 3: 2+
            if (minRating == 1) query = query.Where(g => (g.AverageRating ?? 0) >= 4);
            else if (minRating == 2) query = query.Where(g => (g.AverageRating ?? 0) >= 3);
            else if (minRating == 3) query = query.Where(g => (g.AverageRating ?? 0) >= 2);

            // 4. Fiyat Filtresi
            // Index 1: Ücretsiz, Index 2: Ücretli
            if (priceFilter == 1) query = query.Where(g => g.Price == 0);
            else if (priceFilter == 2) query = query.Where(g => g.Price > 0);

            // 5. Sıralama
            switch (sortOrder)
            {
                case 1: query = query.OrderByDescending(g => g.ReleaseDate); break; // En Yeniler
                case 2: query = query.OrderByDescending(g => g.AverageRating ?? 0); break; // Puanı Yüksek
                case 3: query = query.OrderBy(g => g.Price); break; // Fiyatı Düşük
                default: query = query.OrderBy(g => g.Title); break; // Varsayılan (A-Z)
            }

            // Sorguyu çalıştır ve listeyi döndür (SQL burada çalışır)
            return await query.ToListAsync();
        }
        public async Task<List<string>> GetGenresAsync()
        {
            // SQL Karşılığı: SELECT DISTINCT Genre FROM Games ORDER BY Genre ASC
            return await _context.Games
                                 .Select(g => g.Genre) // Sadece Genre sütununu seç
                                 .Distinct()           // Tekrar edenleri kaldır (Örn: 50 tane RPG varsa 1 tane al)
                                 .OrderBy(g => g)      // Alfabetik sırala
                                 .ToListAsync();
        }
    }
}