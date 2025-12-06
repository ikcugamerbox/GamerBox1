using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GamerBox.DataAccessLayer.Abstract
{
    public interface IPostDal : IGenericDal<Post>
    {
        List<Post> GetPostsByUser(int userId); 
        List<Post> GetPostsByHashtag(string hashtag); 
        List<Post> GetRecentPosts(int count); 
    }
}
