using GamerBox.DataAccessLayer.Abstract;
using GamerBox.DataAccessLayer.Context;
using GamerBox.DataAccessLayer.Repositories;
using GamerBox.EntitiesLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamerBox.DataAccessLayer.EntityFramework
{
    public class EFUserDal : GenericRepository<User>, IUserDal
    {
        public EFUserDal(GamerBoxContext context) : base(context)
        {
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<User>> GetFollowersAsync(int userId)
        {
            // Following listesi içinde userId'si olan kullanıcıları getir (Beni takip edenler)
            return await _context.Users
                .Where(u => u.Following.Any(f => f.Id == userId))
                .ToListAsync();
        }

        public async Task<List<User>> GetFollowingAsync(int userId)
        {
            //// Kullanıcının takip ettikleri (User.Following listesi)
            //var user = await _context.Users
            //    .Include(u => u.Following)
            //    .FirstOrDefaultAsync(u => u.Id == userId);

            //return user?.Following.ToList() ?? new List<User>();
            // SQL: Select * from Users where Id IN (Select FollowingId from ...)
            return await _context.Users
                .Where(u => u.Id == userId)
                .SelectMany(u => u.Following) // Takip ettiklerine geçiş yap
                .ToListAsync(); // Listeyi asenkron getir
        }

        public async Task<List<string>> GetFavoriteGenresAsync(int userId)
        {
            var genresString = await _context.Users
          .Where(u => u.Id == userId)
          .Select(u => u.FavoriteGenres)
          .FirstOrDefaultAsync();

            // 2. String boş mu kontrolü (Null check dahil)
            if (string.IsNullOrWhiteSpace(genresString))
                return new List<string>();

            // 3. Virgülle ayırıp listeye çevir
            return genresString
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries) // Boşlukları temizle
                .Select(g => g.Trim())
                .ToList();
        }

        public async Task<List<Game>> GetUserGamesAsync(int userId)
        {
            return await _context.Games.Where(g => g.UserId == userId).ToListAsync();
        }

        public async Task<Game> GetGameByIdAsync(int gameId)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Id == gameId);
        }

        public async Task<bool> IsFollowingAsync(int followerId, int targetUserId)
        {
            return await _context.Users
          .AnyAsync(u => u.Id == followerId && u.Following.Any(f => f.Id == targetUserId));
        }

        public void Follow(int followerId, int targetUserId)//async yapmaya çalış sonra
        {
            var follower = _context.Users
                .Include(u => u.Following)
                .FirstOrDefault(u => u.Id == followerId);

            var targetUser = _context.Users
                .FirstOrDefault(u => u.Id == targetUserId);

            if (follower == null || targetUser == null) return;

            // Eğer zaten takip etmiyorsa ekle
            if (!follower.Following.Any(u => u.Id == targetUserId))
            {
                follower.Following.Add(targetUser);
                _context.SaveChanges();
            }
        }

        public async Task<List<User>> GetFollowedUsersAsync(int userId)
        {
            return await GetFollowingAsync(userId);
        }
        public void Unfollow(int followerId, int targetUserId)
        {
            // Takip edeni, takip ettikleri listesiyle (Following) birlikte çekiyoruz
            var follower = _context.Users
                .Include(u => u.Following)
                .FirstOrDefault(u => u.Id == followerId);

            if (follower == null) return;

            // Listede hedef kullanıcı var mı kontrol et
            var targetUser = follower.Following.FirstOrDefault(u => u.Id == targetUserId);

            // Varsa listeden kaldır ve kaydet
            if (targetUser != null)
            {
                follower.Following.Remove(targetUser);
                _context.SaveChanges();
            }
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            // Kullanıcı adını (büyük/küçük harf duyarsız olabilir veritabanı ayarına göre) arıyoruz
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}