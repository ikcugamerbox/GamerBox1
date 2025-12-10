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
        private readonly IRatingDal _ratingDal; // rating için database e ulaşmamızı sağlayacak DAL nesnesi.

        public RatingManager(IRatingDal ratingDal) // dışardan bir IRatingDal nesnesi alınır. Dependency Injection.
        {
            _ratingDal = ratingDal ?? throw new ArgumentNullException(nameof(ratingDal));  //verilen ratingDal null ise hata fırlatır,değilse atama yapar.
        }


        // BASIC CRUD (IGenericService<Rating> uyumu)

        public void Add(Rating entity) //yeni bir rating ekler.
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity)); //entity null ise hata fırlatır.
            if (entity.Score < 1 || entity.Score > 5)
                throw new InvalidOperationException("Score must be between 1 and 5.");

            _ratingDal.Add(entity); // uygunsa database e ekler.
        }

        public void Update(Rating entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.Score < 1 || entity.Score > 5)
                throw new InvalidOperationException("Score must be between 1 and 5.");

            _ratingDal.Update(entity);
        }

        public void Delete(Rating entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _ratingDal.Delete(entity);
        }

        public Rating GetById(int id) // id ye göre bir rating getirir.
        {
            return _ratingDal.GetById(id);
        }

        public List<Rating> GetAll() // tüm ratingleri getirir.
        {
            return _ratingDal.GetAll();
        }

        // T* metodları (IGenericService için)
        public void TInsert(Rating entity) => Add(entity);
        public void TUpdate(Rating entity) => Update(entity);
        public void TDelete(Rating entity) => Delete(entity);
        public Rating TGetById(int id) => GetById(id);
        public List<Rating> TGetAll() => GetAll();

        // CUSTOM BUSINESS METHODS

        public Rating RateGame(int userId, int gameId, int score)// kullanıcının bir oy vermesini sağlar. aynı oyuna 2 kez oy veremez.
        {
            if (score < 1 || score > 5)
                throw new InvalidOperationException("Score must be between 1 and 5.");

            // Aynı kullanıcı aynı oyunu daha önce puanladı mı kontrolü
            var alreadyRated = _ratingDal.GetAll()
                                        .Any(r => r.UserId == userId && r.GameId == gameId);
            if (alreadyRated)
                throw new InvalidOperationException("You have already rated this game.");

            var rating = new Rating // yeni bir rating nesnesi oluştur.
            {
                UserId = userId,
                GameId = gameId,
                Score = score,
                CreatedAtUtc = DateTime.UtcNow
            };

            _ratingDal.Add(rating);// rating i database e ekle.
            return rating;
        }

        public List<Rating> GetByGameId(int gameId) // bir oyunun tüm ratinglerini döner .
        {
            return _ratingDal.GetAll()
                             .Where(r => r.GameId == gameId)
                             .ToList();
        }

        public bool HasUserRated(int userId, int gameId) // kullanıcının belirli bir oyunu puanlayıp puanlamadığını döner.
        {
            return _ratingDal.GetAll()
                             .Any(r => r.UserId == userId && r.GameId == gameId);
        }

        public List<Rating> GetByUserId(int userId) // bir kullanıcının verdiği tüm ratingleri döner.
        {
            return _ratingDal.GetAll()
                             .Where(r => r.UserId == userId)
                             .ToList();
        }


        public double GetAverageRating(int gameId)// Bir oyunun ortalama puanını döner. Oy yoksa 0 döner.
        {
            var ratings = GetByGameId(gameId);// oyuna ait tüm ratingleri alır.
            return ratings.Count == 0 ? 0.0 : ratings.Average(r => r.Score); // rating yoksa 0 döner, varsa average döner .
        }
    }
}