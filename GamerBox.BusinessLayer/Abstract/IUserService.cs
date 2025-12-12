using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IUserService : IGenericService<User>
    {
        Task RegisterAsyncB(User user, string plainPassword);
        Task<User> LoginAsyncB(string userEmail, string password);
        // giriş başarılıysa user döner , başarısızsa hata .
        Task FollowAsyncB(int followerId, int targetUserId);
        Task SetThemeAsyncB(int userId, string theme);
        Task SetPreferredCategoriesAsyncB(int userId, List<string> categories);
        Task<List<User>> GetFollowedUsersAsyncB(int userId);
        Task<List<Game>> GetUserGamesAsyncB(int userId);
    }
}