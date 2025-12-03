using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Repositories;
using GamerBox.EntitiesLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.EntityFramework
{
    public class EFPostDal : GenericRepository<Post>, IPostDal
    {
        public List<Post> GetPostsByHashtag(string hashtag)
        {
            throw new NotImplementedException();
        }

        public List<Post> GetPostsByUser(int userId)
        {
            throw new NotImplementedException();
        }

        public List<Post> GetRecentPosts(int count)
        {
            throw new NotImplementedException();
        }
    }
}
