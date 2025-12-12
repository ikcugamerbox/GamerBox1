using GamerBox.EntitiesLayer.Concrete;
using System.Collections.Generic;

namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IUserDal : IGenericDal<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<List<User>> GetFollowersAsync(int userId);
        Task<List<User>> GetFollowingAsync(int userId);
        Task<List<string>> GetFavoriteGenresAsync(int userId);
        Task<Game> GetGameByIdAsync(int gameId);
        Task<bool> IsFollowingAsync(int followerId, int targetUserId);
        void Follow(int followerId, int targetUserId);
        Task<List<User>> GetFollowedUsersAsync(int userId);
        Task<List<Game>> GetUserGamesAsync(int userId);


    }
}