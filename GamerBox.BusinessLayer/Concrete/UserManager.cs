using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GamerBox.BusinessLayer.Abstract;
using GamerBox.DataAccessLayer.Abstract;
using GamerBox.EntitiesLayer.Concrete;

namespace GamerBox.BusinessLayer.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;

        public UserManager(IUserDal userDal)
        {
            _userDal = userDal;
        }

        // --- GÜVENLİK ÖNLEMİ (IGenericService) ---

        public async Task AddAsyncB(User entity)
        {
            // Kullanıcı eklerken şifre hashleme zorunluluğu olduğu için
            // Add metodunu kapatıyor ve Register'a yönlendiriyoruz.
            throw new InvalidOperationException("Please use 'Register' method to add a new user with secure password hashing.");
        }

        public async  Task UpdateAsyncB(User entity)
        {
            // Not: Şifre güncelleme işlemleri için ayrı bir metot (ChangePassword) yazılmalı.
            // Bu metot sadece profil bilgilerini (Bio, Theme vb.) güncellemek için kullanılmalı.
            await _userDal.UpdateAsync(entity);
        }

        public async Task DeleteAsyncB(User entity) => await _userDal.DeleteAsync(entity);
        public async Task<User> GetByIdAsyncB(int id) => await _userDal.GetByIdAsync(id);
        public async Task<List<User>> GetAllAsyncB() => await _userDal.GetAllAsync();

        // --- IUserService Özel Metotları ---

        public async Task RegisterAsyncB(User user, string plainPassword)
        {
            if (!IsValidEmail(user.Email))
                throw new InvalidOperationException("Invalid email format.");

            if (!IsStrongPassword(plainPassword))
                throw new InvalidOperationException("Password must be at least 8 characters long and contain uppercase, lowercase, digit, and special character.");

            // Email kontrolü
            var existingUser = await _userDal.GetUserByEmailAsync(user.Email);
            if (existingUser != null)
                throw new InvalidOperationException("A user with this email already exists.");

            user.PasswordHash = HashPassword(plainPassword);
            user.CreatedAtUtc = DateTime.UtcNow;

            await _userDal.AddAsync(user);
        }

        public async Task<User> LoginAsyncB(string userEmail, string password)
        {
           
            if (!IsValidEmail(userEmail))
                throw new InvalidOperationException("Invalid email format .");

            // Kullanıcıyı bul
            var user = await _userDal.GetUserByEmailAsync(userEmail);
            bool isPasswordVerified = (user != null) && VerifyPassword(password, user.PasswordHash);

            if (!isPasswordVerified)
                throw new InvalidOperationException("The email address or password is incorrect.");

            user.LastLoginUtc = DateTime.UtcNow;
            await _userDal.UpdateAsync(user);

            return user;
        }

        public async Task FollowAsyncB(int followerId, int targetUserId)
        {
            if (followerId == targetUserId)
                throw new InvalidOperationException("You cannot follow yourself.");

            var already = await _userDal.IsFollowingAsync(followerId, targetUserId);
            if (already)
                throw new InvalidOperationException("This user is already being followed.");

            _userDal.Follow(followerId, targetUserId);
        }

        // --- Kişiselleştirme Metotları ---

        public async Task SetThemeAsyncB(int userId, string theme)
        {
            if (theme != "dark" && theme != "light")
                throw new InvalidOperationException("Theme must be either 'dark' or 'light'.");

            var user = await _userDal.GetByIdAsync(userId);
            if (user == null) throw new InvalidOperationException("User not found.");

            user.ThemePreference = theme; // Theme değil ThemePreference (Entity'de öyle tanımlı)
            await _userDal.UpdateAsync(user);
        } 

        public async Task SetPreferredCategoriesAsyncB(int userId, List<string> categories)
        {
            if (categories == null || categories.Count == 0)
                throw new InvalidOperationException("You must select at least one category.");

            var user = await _userDal.GetByIdAsync(userId);
            if (user == null) throw new InvalidOperationException("User not found.");

            // List<string> -> string çevrimi (Virgülle ayırma)
            user.FavoriteGenres = string.Join(",", categories.Distinct());
            await _userDal.UpdateAsync(user);
        }

        public async Task<List<User>> GetFollowedUsersAsyncB(int userId) => await _userDal.GetFollowedUsersAsync(userId);
        public async Task<List<Game>> GetUserGamesAsyncB(int userId) =>await _userDal.GetUserGamesAsync(userId);


        // --- Helper Methods (Private) ---

        private string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[16];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            var key = pbkdf2.GetBytes(32);

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        private bool VerifyPassword(string password, string hash)
        {
            var parts = hash.Split('.', 2);
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var key = Convert.FromBase64String(parts[1]);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            var keyToCheck = pbkdf2.GetBytes(32);

            return CryptographicOperations.FixedTimeEquals(keyToCheck, key);
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        private bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            return password.Length >= 8 &&
                   Regex.IsMatch(password, "[A-Z]") &&
                   Regex.IsMatch(password, "[a-z]") &&
                   Regex.IsMatch(password, "[0-9]") &&
                   Regex.IsMatch(password, @"[\W_]");
        }
        public async Task UnfollowAsyncB(int followerId, int targetUserId)
        {
            // Await kullanmadan çağırıyoruz çünkü DAL metodu void
            _userDal.Unfollow(followerId, targetUserId);

            // Asenkron bir işlem gibi davranması için (Task dönmesi için)
            await Task.CompletedTask;
        }
        public async Task<List<User>> GetFollowersAsyncB(int userId)
        {
            // DAL'dan beni takip edenleri getir
            return await _userDal.GetFollowersAsync(userId);
        }
        public async Task<bool> IsFollowingAsyncB(int followerId, int targetUserId)
        {
            return await _userDal.IsFollowingAsync(followerId, targetUserId);
        }
        public async Task<User> GetUserByUsernameAsyncB(string username)
        {
            return await _userDal.GetUserByUsernameAsync(username);
        }
    }
}