using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IRatingDal : IGenericDal<Rating>
    {
        Task<bool> HasUserRatedGameAsync(int userId, int gameId);
        Task<double> GetAverageRatingForGameAsync(int gameId);
        Task<List<Rating>> GetByGameIdAsync(int gameId);
        Task<List<Rating>> GetByUserIdASync(int userId);
    }

}