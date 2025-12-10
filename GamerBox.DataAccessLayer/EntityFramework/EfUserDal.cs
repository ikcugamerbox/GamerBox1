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


        public List<Game> GetUserGames(int userId)
        {
            return _context.Games.Where(g => g.UserId == userId).ToList();
        }

        public Game GetGameById(int gameId)
        {
            return _context.Games
                .FirstOrDefault(g => g.Id == gameId);
        }

        public bool IsFollowing(int followerId, int targetUserId)
        {
            var follower = _context.Users
                .Include(u => u.Following)
                .FirstOrDefault(u => u.Id == followerId);
            if (follower == null)
                return false;
            return follower.Following.Any(u => u.Id == targetUserId);
        }

        public void Follow(int followerId, int targetUserId)
        {
            var follower = _context.Users
                .Include(u => u.Following)
                .FirstOrDefault(u => u.Id == followerId);
            var targetUser = _context.Users
                .FirstOrDefault(u => u.Id == targetUserId);
            if (follower == null || targetUser == null)
                return;
            follower.Following.Add(targetUser);
            _context.SaveChanges();

        }

        public List<User> GetFollowedUsers(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                return new List<User>();
            return user.Following.ToList();

        }

        public void AddFriend(int followerId, int targetUserId)
        {
            var follower = _context.Users
                .Include(u => u.Friends)
                .FirstOrDefault(u => u.Id == followerId);
            var targetUser = _context.Users
                .FirstOrDefault(u => u.Id == targetUserId);
            if (follower == null || targetUser == null)
                return;
            follower.Friends.Add(targetUser);
            _context.SaveChanges();
        }

    }
}