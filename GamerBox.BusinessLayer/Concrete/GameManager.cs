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

        public async Task AddGameAsyncB(Game entity)
        {
            if (entity.Price < 0)
                throw new InvalidOperationException("Game price cannot be negative.");

            if (string.IsNullOrWhiteSpace(entity.Title)) // Name yerine Title kullandım (Entity ile uyumlu olması için)
                throw new InvalidOperationException("Game title cannot be empty.");

            await _gameDal.AddAsync(entity);
        }

        public async Task UpdateGameAsyncB(Game entity)
        {
            if (entity.Price < 0)
                throw new InvalidOperationException("Game price cannot be negative.");

            if (string.IsNullOrWhiteSpace(entity.Title))
                throw new InvalidOperationException("Game title cannot be empty.");

           await _gameDal.UpdateAsync(entity);
        }

        public async Task DeleteGameAsyncB(Game entity)
        {
            await _gameDal.DeleteAsync(entity);
        }

        public async Task<Game> GetGameByIdAsyncB(int id)
        {
            return await _gameDal.GetByIdAsync(id);
        }

        public async Task<List<Game>> GetAllGamesAsyncB()
        {
             return await _gameDal.GetAllAsync();
        }

        // --- CUSTOM BUSINESS METHODS ---

        public async Task<List<Game>> GetByRatingAsyncB(int count)
        {
            if (count <= 0)
                throw new InvalidOperationException("Count must be greater than zero.");

            // GetAllAsync ÇAĞIRMA! 
            // Doğrudan DAL'daki akıllı metodu çağır:
            return await _gameDal.GetByRatingAsync(count);
        }

        public async Task<List<Game>> RecommendByCategoriesAsyncB(int userId, int count)
        {
            if (count <= 0)
                throw new InvalidOperationException("Count must be greater than zero.");

            // DÜZELTME: GetGameById yerine GetById kullanıldı.
            var user = _userDal.GetByIdAsync(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            // Kullanıcının tercih ettiği kategori listesi
            var preferredCategories = await _userDal.GetFavoriteGenresAsync(userId);

            if (preferredCategories == null || !preferredCategories.Any())
                throw new InvalidOperationException("You must select a category before getting recommendations.");

            // Kategoriye göre filtreleme
            //return await _gameDal.GetAllAsync() // GetAllGames() yerine GetAll() kullanabiliriz, IGenericDal'dan gelir.
            //               .Where(g => preferredCategories.Contains(g.Genre)) // Category yerine Genre kullanıldı (Game entity'sinde Genre var)
            //               .OrderByDescending(g => g.AverageRating ?? 0)
            //               .Take(count)
            //               .ToList
            return await _gameDal.GetByGenresAsync(preferredCategories, count);
        }
    }
}