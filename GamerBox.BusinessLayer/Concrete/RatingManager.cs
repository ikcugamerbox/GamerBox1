using GamerBox.BusinessLayer.Abstract;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace GamerBox.BusinessLayer.Concrete
{
    public class RatingManager : IRatingService
    {
        private readonly IRatingDal _ratingDal;

        public RatingManager(IRatingDal rating)
        public RatingManager(IRatingDal ratingDal)
        {
            _ratingDal = rating;
            _ratingDal = ratingDal;
        }

        public void TDelete(Rating entity)
        public Rating RateGame(int userId, int gameId, int score)
        {
            throw new NotImplementedException();
        }
            if (score< 1 || score> 5)
                throw new InvalidOperationException("Score must be between 1 and 5.");

        public List<Rating> TGetAll()
        {
            throw new NotImplementedException();
            var alreadyRated = _ratingDal.GetAll().Any(r => r.UserId == userId && r.GameId == gameId);
            if (alreadyRated)
                throw new InvalidOperationException("You have already rated this game.");

            var rating = new Rating
            {
                UserId = userId,
                GameId = gameId,
                Score = score,
                CreatedAtUtc = DateTime.UtcNow
            };

            _ratingDal.Add(rating);
            return rating;
        }

        public Rating TGetById(int id)
        public List<Rating> GetByGameId(int gameId) =>
            _ratingDal.GetAll().Where(r => r.GameId == gameId).ToList();

        public double GetAverageRating(int gameId)
        {
            throw new NotImplementedException();
            var ratings = _ratingDal.GetAll().Where(r => r.GameId == gameId).ToList();
            return ratings.Count == 0 ? 0 : ratings.Average(r => r.Score);
        }

        public void TInsert(Rating entity)
        // IGenericService<Rating> implementasyonları
        public void Add(Rating entity)
        {
            throw new NotImplementedException();
            if (entity.Score < 1 || entity.Score > 5)
                throw new InvalidOperationException("Score must be between 1 and 5.");
            _ratingDal.Add(entity);
        }

        public void TUpdate(Rating entity)
        public void Update(Rating entity)
        {
            throw new NotImplementedException();
            if (entity.Score < 1 || entity.Score > 5)
                throw new InvalidOperationException("Score must be between 1 and 5.");
            _ratingDal.Update(entity);
        }

        public void Delete(Rating entity) => _ratingDal.Delete(entity);
        public Rating GetById(int id) => _ratingDal.GetById(id);
        public List<Rating> GetAll() => _ratingDal.GetAll();
    }
}