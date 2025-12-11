using GamerBox.EntitiesLayer.Concrete;
using System.Collections.Generic;

namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IPostDal : IGenericDal<Post>
    {
        List<Post> GetPostsByUser(int userId);
        List<Post> GetPostsByHashtag(string hashtag);
        List<Post> GetRecentPosts(int count);

        // void Add(Post post); -> SİLİNDİ (IGenericDal'dan miras alıyor)
    }
}