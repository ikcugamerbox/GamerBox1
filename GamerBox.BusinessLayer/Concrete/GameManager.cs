using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Concrete
{
    class GameManager : IGameService
    public class GameManager : IGameService
    {
        private readonly IGameDal _gameDal;
        private readonly IUserDal _userDal;

        public GameManager(IGameDal game)
        public GameManager(IGameDal gameDal, IUserDal userDal)
        {
            _gameDal = game;
            _gameDal = gameDal;
            _userDal = userDal;
        }

        public void TDelete(Game entity)
        // Oyunları puana göre sıralayıp belli sayıda getirir
        public List<Game> GetByRating(int count)
        {
            _gameDal.Delete(entity);
        }
            if (count <= 0)
                throw new InvalidOperationException("Count must be greater than zero.");

        public List<Game> TGetAll()
        {
            return _gameDal.GetAll();
            return _gameDal.GetAll()
                           .OrderByDescending(g => g.Rating)
                           .Take(count)
                           .ToList();
        }

        public Game TGetById(int id)
        // Kullanıcının tercih ettiği kategorilere göre öneri yapar
        public List<Game> RecommendByCategories(int userId, int count)
        {
            return _gameDal.GetById(id);
            if (count <= 0)
                throw new InvalidOperationException("Count must be greater than zero.");

            var user = _userDal.GetById(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            var preferredCategories = _userDal.GetPreferredCategories(userId);
            if (preferredCategories == null || !preferredCategories.Any())
                throw new InvalidOperationException("You must select a category before getting recommendations.");

            return _gameDal.GetAll()
                           .Where(g => preferredCategories.Contains(g.Category))
                           .OrderByDescending(g => g.Rating)
                           .Take(count)
                           .ToList();
        }

        public void TInsert(Game entity)
        // IGenericService<Game> implementasyonları
        public void Add(Game entity)
        {
            _gameDal.Insert(entity);
            if (entity.Price < 0)
                throw new InvalidOperationException("Game price cannot be negative.");
            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new InvalidOperationException("Game name cannot be empty.");

            _gameDal.Add(entity);
        }

        public void TUpdate(Game entity)
        public void Update(Game entity)
        {
            if (entity.Price < 0)
                throw new InvalidOperationException("Game price cannot be negative.");
            if (string.IsNullOrWhiteSpace(entity.Name))
                throw new InvalidOperationException("Game name cannot be empty.");

            _gameDal.Update(entity);
        }

        public void Delete(Game entity) => _gameDal.Delete(entity);
        public Game GetById(int id) => _gameDal.GetById(id);
        public List<Game> GetAll() => _gameDal.GetAll();
    }
}
