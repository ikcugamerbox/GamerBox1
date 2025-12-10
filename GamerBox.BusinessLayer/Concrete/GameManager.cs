using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamerBox.BusinessLayer.Concrete
{
    public class GameManager : IGameService
    //GameManager sınıfı IGameService interfaceini implement eder .
    {
        private readonly IGameDal _gameDal; //game tablosuyla ilgili database işlemlerini yapar.
        private readonly IUserDal _userDal;  //user verileri (kategori tercihi vs.)için DAL.

        public GameManager(IGameDal gameDal, IUserDal userDal) //nesne oluşturulurken gameDal ve userDal dışarıdan verilir.Dependency Injection.
        {
            _gameDal = gameDal;
            _userDal = userDal;
        }

        //   BASIC CRUD OPERATIONS

        public void AddGame(Game entity)
        {
            if (entity.Price < 0)
                throw new InvalidOperationException("Game price cannot be negative.");

            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new InvalidOperationException("Game name cannot be empty.");

            _gameDal.Add(entity);// uygunsa database e ekler.
        }
            if (count <= 0)
                throw new InvalidOperationException("Count must be greater than zero.");

        public void UpdateGame(Game entity)
        {
            if (entity.Price < 0)
                throw new InvalidOperationException("Game price cannot be negative.");

            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new InvalidOperationException("Game name cannot be empty.");

            _gameDal.Update(entity);// uygunsa database de oyun güncellenir.
        }

        public void DeleteGame(Game entity)
        {
            _gameDal.Delete(entity);
        }

        public Game GetGameById(int id)
        {
            return _gameDal.GetById(id);
        }

        public List<Game> GetAllGames()
        {
            return _gameDal.GetAll();
        }

        //   CUSTOM BUSINESS METHODS

        public List<Game> GetByRating(int count)//en yüksek puanlı oyunları getirir. count kaç tane oyun geleceğini belirtir.
        {
            if (count <= 0)
                throw new InvalidOperationException("Count must be greater than zero.");

            return _gameDal.GetAll()
                           .OrderByDescending(g => g.AverageRating ?? 0)//rating e göre büyükten küçüğe sıralar.
                           .Take(count)
                           .ToList();
        }
        public List<Game> RecommendByCategories(int userId, int count)// Kullanıcı tercih ettiği kategorilere göre oyun önerir
        {
            if (count <= 0)
                throw new InvalidOperationException("Count must be greater than zero.");

            var user = _userDal.GetGameById(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var preferredCategories = _userDal.GetFavoriteGenres(userId); // kullanıcının tercih ettiği kategori listesi database den alınır. 
            if (preferredCategories == null || !preferredCategories.Any())
                throw new InvalidOperationException("You must select a category before getting recommendations.");

            return _gameDal.GetAllGames()
                           .Where(g => preferredCategories.Contains(g.Category))//kullanıcının tercih ettiği kategoride olan oyunları getir.
                           .OrderByDescending(g => g.AverageRating ?? 0)//yüksek puanlı olandan başlayarak sırala.
                           .Take(count)
                           .ToList();

        }
        //    public void TgetTopRatedGames(int count) {
        //        if count <= 0)
        //            GetByRating(count); }// TgetTopRatedGames çağrıldığında GetByRating çalışır.
    }
}