using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IRatingService : IGenericService<Rating>
    public interface IGameService : IGenericService<Game>
    {
        List<Game> GetByRating(int count);
        List<Game> RecommendByCategories(int userId, int count);
    }
}
