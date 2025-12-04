using System.Collections.Generic;
using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        void Register(User user, string plainPassword);
        User Login(string userEmail, string password);
        void Follow(int followerId, int targetUserId);
        void SetTheme(int userId, string theme);
        void SetPreferredCategories(int userId, List<string> categories);
        List<User> GetFollowedUsers(int userId);
        List<Game> GetUserGames(int userId);
    }
}