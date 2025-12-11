using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Add(Rating entity)
        {
            ValidateScore(entity.Score);
            // Add metodu genellikle doğrudan kullanılmaz, RateGame kullanılır.
            // Ancak yine de validasyon ekledik.
            _ratingDal.Add(entity);
        }

        public void Update(Rating entity)
        {
            ValidateScore(entity.Score);
            _ratingDal.Update(entity);
        }

        public void Delete(Rating entity)
        {
            _ratingDal.Delete(entity);
        }

        public Rating GetById(int id)
        {
            return _ratingDal.GetById(id);
        }

        public List<Rating> GetAll()
        {
            return _ratingDal.GetAll();
        }

        // --- IRatingService Özel Metotları ---

        public Rating RateGame(int userId, int gameId, int score)
        {
            ValidateScore(score);

            // DÜZELTME: Tüm listeyi çekmek yerine (GetAll), DAL'daki özel metodu kullandık.
            // Bu, performansı 100 kat artırabilir.
            bool alreadyRated = _ratingDal.HasUserRatedGame(userId, gameId);

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

            _ratingDal.Add(rating);
            return rating;
        }

        public List<Rating> GetByGameId(int gameId)
        {
            // Eğer DAL'da GetRatingsByGameId yoksa mecburen GetAll ile filtreliyoruz.
            // İdealde DAL'a bu metodun eklenmesi gerekir. Şimdilik bu şekilde bırakıyoruz:
            return _ratingDal.GetAll()
                             .Where(r => r.GameId == gameId)
                             .ToList();
        }

        public List<Rating> GetByUserId(int userId)
        {
            return _ratingDal.GetAll()
                            .Where(r => r.UserId == userId)
                            .ToList();
        }

        public bool HasUserRated(int userId, int gameId)
        {
            // DÜZELTME: Doğrudan veritabanı sorgusu
            return _ratingDal.HasUserRatedGame(userId, gameId);
        }

        public double GetAverageRating(int gameId)
        {
            // DÜZELTME: Doğrudan veritabanından ortalama çekme
            return _ratingDal.GetAverageRatingForGame(gameId);
        }

        // --- Yardımcı Metotlar ---

        private void ValidateScore(int score)
        {
            if (score < 1 || score > 5)
                throw new InvalidOperationException("Score must be between 1 and 5.");
        }
    }
}