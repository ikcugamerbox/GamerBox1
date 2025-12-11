using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
{

    public interface IRatingService : IGenericService<Rating>
    {
        List<Rating> GetByGameId(int gameId);
        //belirli bir oyunun tüm puanlarını getir.

        List<Rating> GetByUserId(int userId);
        // belirli bir kullanıcının verdiği tüm puanları getir.

        double GetAverageRating(int gameId);
        // oyunun ortalama puanını hesapla. 

        bool HasUserRated(int userId, int gameId);
        //kullanıcı bu oyuna puan vermiş mi




    }

}

