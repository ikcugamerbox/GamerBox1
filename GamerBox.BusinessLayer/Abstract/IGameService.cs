using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IGameService
    {
        // Temel CRUD işlemleri
        Task AddGameAsyncB(Game game);
        Task UpdateGameAsyncB(Game game);
        Task DeleteGameAsyncB(Game game);
        Task<Game> GetGameByIdAsyncB(int id);
        Task<List<Game>> GetAllGamesAsyncB();

        // Özel iş kuralları
        Task<List<Game>> GetByRatingAsyncB(int count);
        Task<List<Game>> RecommendByCategoriesAsyncB(int userId, int count);

    }

}