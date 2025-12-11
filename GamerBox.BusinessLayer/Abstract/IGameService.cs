using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IGameService
    {
        // Temel CRUD işlemleri
        void AddGame(Game game);
        void UpdateGame(Game game);
        void DeleteGame(Game game);
        Game GetGameById(int id);
        List<Game> GetAllGames();

        // Özel iş kuralları
        List<Game> GetByRating(int count);
        List<Game> RecommendByCategories(int userId, int count);

    }

}