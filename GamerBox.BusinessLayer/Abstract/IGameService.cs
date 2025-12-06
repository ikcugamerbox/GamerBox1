
using System.Collections.Generic;

using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IGameService : IGenericService<Game>
    {
        List<Game> GetByRating(int count);
        List<Game> RecommendByCategories(int userId, int count);
    }
}