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

        public async Task<List<Post>> GetPostsByUserAsync(int userId)
        {
            return await _context.Posts
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
        public async Task<List<Post>> GetPostsByGameIdAsync(int gameId)
        {
            // SQL: SELECT * FROM Posts WHERE GameId = @gameId
            return await _context.Posts
                                 .Where(p => p.GameId == gameId)
                                 .OrderByDescending(p => p.CreatedAt) // Yeniden eskiye
                                 .ToListAsync();
        }
        public List<Post> GetPostsByHashtag(string hashtag)
        {
            return _context.Posts
        .Include(p => p.Hashtags) // İlişkiyi dahil et
        .Where(p => p.Hashtags.Any(h => h.Tag == hashtag)) // "Herhangi bir etiketi aranan kelimeye eşit mi?"
        .OrderByDescending(p => p.CreatedAt)
        .ToList();
        
        }

        public async Task<List<Post>> GetRecentPostsAsync(int count)
        {
            return await _context.Posts
                .OrderByDescending(p => p.CreatedAt)
                .Take(count)
                .ToListAsync();
        }

        // Add metodu GenericRepository'den geliyor, tekrar yazmaya gerek yok.
    }
}