using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
{

    public interface IRatingService : IGenericService<Rating>
    {
        Task<List<Rating>> GetByGameIdAsyncB(int gameId);
        //belirli bir oyunun tüm puanlarını getir.

        Task<List<Rating>> GetByUserIdAsyncB(int userId);
        // belirli bir kullanıcının verdiği tüm puanları getir.

        Task<double>GetAverageRatingAsyncB(int gameId);
        // oyunun ortalama puanını hesapla. 

        Task<bool> HasUserRatedAsyncB(int userId, int gameId);

        Task<Rating> RateGameAsyncB(int userId, int gameId, int score);



    }

}

