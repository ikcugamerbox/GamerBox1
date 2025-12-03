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
        public List<Game> GetGamesByCategory(string category)
        {
            throw new NotImplementedException();
        }

        public List<Game> GetRecommendedGamesForUser(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Game> GetTopRatedGames(int count)
        {
            throw new NotImplementedException();
        }
    }
}
