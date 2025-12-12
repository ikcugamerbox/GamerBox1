using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamerBox.BusinessLayer.Concrete
{
    public class RatingManager : IRatingService
    {
        private readonly IRatingDal _ratingDal;

        public RatingManager(IRatingDal ratingDal)
        {
            _ratingDal = ratingDal ?? throw new ArgumentNullException(nameof(ratingDal));
        }

        // --- IGenericService İmplementasyonları ---

        public async Task AddAsyncB(Rating entity)
        {
            ValidateScore(entity.Score);
            // Add metodu genellikle doğrudan kullanılmaz, RateGame kullanılır.
            // Ancak yine de validasyon ekledik.
           await _ratingDal.AddAsync(entity);
        }

        public async Task UpdateAsyncB(Rating entity)
        {
            ValidateScore(entity.Score);
             await _ratingDal.UpdateAsync(entity);
        }

        public async Task DeleteAsyncB(Rating entity)
        {
            await _ratingDal.DeleteAsync(entity);
        }

        public async Task<Rating> GetByIdAsyncB(int id)
        {
            return await _ratingDal.GetByIdAsync(id);
        }

        public async Task<List<Rating>> GetAllAsyncB()
        {
            return  await _ratingDal.GetAllAsync();
        }

        // --- IRatingService Özel Metotları ---

        public async Task<Rating> RateGameAsyncB(int userId, int gameId, int score)
        {
            ValidateScore(score);

            // DÜZELTME: Tüm listeyi çekmek yerine (GetAll), DAL'daki özel metodu kullandık.
 
            bool alreadyRated = await _ratingDal.HasUserRatedGameAsync(userId, gameId);
            // bool a await kullanmadan atayamayız çünkü metod async.bitmesini bekliyoruz
            if (alreadyRated)
                throw new InvalidOperationException("You have already rated this game.");

            var rating = new Rating
            {
                UserId = userId,
                GameId = gameId,
                Score = score,
                RatedAt = DateTime.Now, // CreatedAtUtc yerine RatedAt kullandım (Entity ile uyumlu)
                CreatedAtUtc = DateTime.UtcNow
            };

            await _ratingDal.AddAsync(rating);
            return rating;
        }

        public async Task<List<Rating>> GetByGameIdAsyncB(int gameId)
        {
            return await _ratingDal.GetByGameIdAsync(gameId);
        }

        public async Task<List<Rating>> GetByUserIdAsyncB(int userId)
        {
            return await _ratingDal.GetByUserIdASync(userId);
        }
        public async Task<bool> HasUserRatedAsyncB(int userId, int gameId)
        {
            // DÜZELTME: Doğrudan veritabanı sorgusu
            return await _ratingDal.HasUserRatedGameAsync(userId, gameId);
        }

        public async Task<double> GetAverageRatingAsyncB(int gameId)
        {
            // DÜZELTME: Doğrudan veritabanından ortalama çekme
            return await _ratingDal.GetAverageRatingForGameAsync(gameId);
        }

        // --- Yardımcı Metotlar ---

        private void ValidateScore(int score)
        {
            if (score < 1 || score > 5)
                throw new InvalidOperationException("Score must be between 1 and 5.");
        }
    }
}