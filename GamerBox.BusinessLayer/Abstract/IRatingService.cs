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