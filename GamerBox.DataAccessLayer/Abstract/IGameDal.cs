using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IGameDal : IGenericDal<Game>
    {

        List<Game> GetTopRatedGames(int count);

        List<Game> GetRecommendedGamesForUser(int userId);

        List<Game> GetAllGames();

        List<Game> GetAllGamesForUser(int userId);
        Task<List<Game>> GetByRatingAsync(int count);
        Task<List<Game>> GetByGenresAsync(List<string> genres, int count);

        Task<List<Game>> GetFilteredGamesAsync(string searchText, string genre, int minRating, int priceFilter, int sortOrder);
        Task<List<string>> GetGenresAsync();
    }
}