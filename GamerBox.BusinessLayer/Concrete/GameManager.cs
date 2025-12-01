using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.BusinessLayer.Concrete
{
    class GameManager : IGameService
    {
        private readonly IGameDal _gameDal;

        public GameManager(IGameDal game)
        {
            _gameDal = game;
        }

        public void TDelete(Game entity)
        {
            _gameDal.Delete(entity);
        }

        public List<Game> TGetAll()
        {
            return _gameDal.GetAll();
        }

        public Game TGetById(int id)
        {
           return _gameDal.GetById(id);
        }

        public void TInsert(Game entity)
        {
            _gameDal.Insert(entity);
        }

        public void TUpdate(Game entity)
        {
            _gameDal.Update(entity);
        }
    }
}
