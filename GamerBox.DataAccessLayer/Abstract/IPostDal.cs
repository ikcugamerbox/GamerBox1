using GamerBox.EntitiesLayer.Concrete;
using System.Collections.Generic;

namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IPostDal : IGenericDal<Post>
    {
        List<Post> GetPostsByHashtag(string hashtag);
        Task<List<Post>> GetRecentPostsAsync(int count);
        Task<List<Post>> GetPostsByUserAsync(int userId);
        Task<List<Post>> GetPostsByGameIdAsync(int gameId);
        // void Add(Post post); -> SİLİNDİ (IGenericDal'dan miras alıyor)
    }
}