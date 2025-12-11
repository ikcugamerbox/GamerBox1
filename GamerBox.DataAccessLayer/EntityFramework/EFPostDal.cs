using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using GamerBox.DataAccessLayer.Repositories;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GamerBox.DataAccessLayer.EntityFramework
{
    public class EFPostDal : GenericRepository<Post>, IPostDal
    {
        

        public EFPostDal(GamerBoxContext context) : base(context)
        {
            
        }




        public List<Post> GetPostsByUser(int userId)
        {
            return _context.Posts
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }


        public List<Post> GetPostsByHashtag(string hashtag)
        {
            return _context.Posts
                .Where(p => p.Hashtags.Contains(hashtag)).OrderByDescending(p => p.CreatedAt).ToList();
        }





        public List<Post> GetRecentPosts(int count)
        {
            return _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToList();
        }

        public void Add(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }



    }
}