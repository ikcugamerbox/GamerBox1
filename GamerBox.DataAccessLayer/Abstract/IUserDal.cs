using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IUserDal : IGenericDal<User>
    {
        User GetUserByEmail(string email);
        List<User> GetFollowers(int userId);
        List<User> GetFollowing(int userId);

        List<string> GetFavoriteGenres(int userId);
        Game GetGameById(int gameId);
        bool IsFollowing(int followerId, int targetUserId);

        void Follow(int followerId, int targetUserId);

        List<User> GetFollowedUsers(int userId);

        List<Game> GetUserGames(int userId);

        void AddFriend(int followerId, int targetUserId);





    }
}
