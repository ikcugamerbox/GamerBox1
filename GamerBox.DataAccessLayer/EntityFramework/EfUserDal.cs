using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using GamerBox.DataAccessLayer.Repositories;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GamerBox.DataAccessLayer.EntityFramework
{
    public class EFUserDal : GenericRepository<User>, IUserDal
    {
        private readonly GamerBoxContext _context;

        public EFUserDal(GamerBoxContext context) : base(context)
        {
            _context = context;
        }

        
        public User GetUserByEmail(string email)
        {
            return _context.Users
                .FirstOrDefault(u => u.Email == email);
        }

        
        public List<User> GetFollowers(int userId)
        {
            return _context.Users
                .Where(u => u.Following.Any(f => f.Id == userId))
                .ToList();
        }

        
        public List<User> GetFollowing(int userId)
        {
            return _context.Users
                .Where(u => u.Followers.Any(f => f.Id == userId))
                .ToList();
        }

        
        public List<string> GetFavoriteGenres(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null || string.IsNullOrWhiteSpace(user.FavoriteGenres))
                return new List<string>();

            return user.FavoriteGenres
                .Split(',')
                .Select(g => g.Trim())
                .ToList();
        }
    }
}