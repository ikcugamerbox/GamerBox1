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
        double GetAverageRatingForGame(int gameId);
        bool HasUserRatedGame(int userId, int gameId);
    }

}