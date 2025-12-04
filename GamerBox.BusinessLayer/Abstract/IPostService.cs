using System.Collections.Generic;
using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Abstract
{
    public interface IPostService : IGenericService<Post>
    {
        List<Post> GetByUserId(int userId);
        List<Post> GetByGameId(int gameId);
        Post CreatePost(int userId, int? gameId, string content);
        List<string> ExtractHashtags(string content);
    }
}