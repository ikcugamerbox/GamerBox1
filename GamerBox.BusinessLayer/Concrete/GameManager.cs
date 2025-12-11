using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GamerBox.BusinessLayer.Concrete
{
    public class GameManager : IGameService
    {
        private readonly IGameDal _gameDal;
        private readonly IUserDal _userDal;

        public GameManager(IGameDal gameDal, IUserDal userDal)
        {
            _gameDal = gameDal;
            _userDal = userDal;
        }

        // --- BASIC CRUD OPERATIONS ---

        public void AddGame(Game entity)
        {
            if (entity.Price < 0)
                throw new InvalidOperationException("Game price cannot be negative.");

            if (string.IsNullOrWhiteSpace(entity.Title)) // Name yerine Title kullandım (Entity ile uyumlu olması için)
                throw new InvalidOperationException("Game title cannot be empty.");

            _gameDal.Add(entity);
        }

        public void UpdateGame(Game entity)
        {
            if (entity.Price < 0)
                throw new InvalidOperationException("Game price cannot be negative.");

            if (string.IsNullOrWhiteSpace(entity.Title))
                throw new InvalidOperationException("Game title cannot be empty.");

            _gameDal.Update(entity);
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

        // --- CUSTOM BUSINESS METHODS ---

        public List<Game> GetByRating(int count)
        {
            if (count <= 0)
                throw new InvalidOperationException("Count must be greater than zero.");

            // Puanı yüksek olanları getirir
            return _gameDal.GetAll()
                           .OrderByDescending(g => g.AverageRating ?? 0)
                           .Take(count)
                           .ToList();
        }

        public List<Game> RecommendByCategories(int userId, int count)
        {
            if (count <= 0)
                throw new InvalidOperationException("Count must be greater than zero.");

            // DÜZELTME: GetGameById yerine GetById kullanıldı.
            var user = _userDal.GetById(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            // Kullanıcının tercih ettiği kategori listesi
            var preferredCategories = _userDal.GetFavoriteGenres(userId);

            if (preferredCategories == null || !preferredCategories.Any())
                throw new InvalidOperationException("You must select a category before getting recommendations.");

            // Kategoriye göre filtreleme
            return _gameDal.GetAll() // GetAllGames() yerine GetAll() kullanabiliriz, IGenericDal'dan gelir.
                           .Where(g => preferredCategories.Contains(g.Genre)) // Category yerine Genre kullanıldı (Game entity'sinde Genre var)
                           .OrderByDescending(g => g.AverageRating ?? 0)
                           .Take(count)
                           .ToList();
        }
    }
}