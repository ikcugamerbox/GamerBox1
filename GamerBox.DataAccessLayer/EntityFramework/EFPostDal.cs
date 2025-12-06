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
        private readonly GamerBoxContext _context;

        public EFPostDal(GamerBoxContext context) : base(context)
        {
            _context = context;
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
                .Where(p => EF.Functions.Like(p.Hashtags, $"%#{hashtag}%"))
                .OrderByDescending(p => p.CreatedAt)
                .ToList();
        }

        
        public List<Post> GetRecentPosts(int count)
        {
            return _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToList();
        }
    }
}