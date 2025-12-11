using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
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

        public void Add(User entity)
        {
            // Kullanıcı eklerken şifre hashleme zorunluluğu olduğu için
            // Add metodunu kapatıyor ve Register'a yönlendiriyoruz.
            throw new InvalidOperationException("Please use 'Register' method to add a new user with secure password hashing.");
        }

        public void Update(User entity)
        {
            // Not: Şifre güncelleme işlemleri için ayrı bir metot (ChangePassword) yazılmalı.
            // Bu metot sadece profil bilgilerini (Bio, Theme vb.) güncellemek için kullanılmalı.
            _userDal.Update(entity);
        }

        public void Delete(User entity) => _userDal.Delete(entity);
        public User GetById(int id) => _userDal.GetById(id);
        public List<User> GetAll() => _userDal.GetAll();

        // --- IUserService Özel Metotları ---

        public void Register(User user, string plainPassword)
        {
            if (!IsValidEmail(user.Email))
                throw new InvalidOperationException("Invalid email format.");

            if (!IsStrongPassword(plainPassword))
                throw new InvalidOperationException("Password must be at least 8 characters long and contain uppercase, lowercase, digit, and special character.");

            // Email kontrolü
            var exists = _userDal.GetAll().Any(u => u.Email == user.Email);
            if (exists)
                throw new InvalidOperationException("A user with this email already exists.");

            user.PasswordHash = HashPassword(plainPassword);
            user.CreatedAtUtc = DateTime.UtcNow;

            // GenericRepository'deki Insert veya Add çağrılabilir, 
            // ama burada _userDal.Add'i direkt çağırıyoruz (bizim yasakladığımız UserManager.Add değil, DAL.Add)
            _userDal.Add(user);
        }

        public User Login(string userEmail, string password)
        {
            if (!IsValidEmail(userEmail))
                throw new InvalidOperationException("Invalid email format.");

            // Kullanıcıyı bul
            var user = _userDal.GetUserByEmail(userEmail); // DAL'da varsa bunu kullanmak daha iyidir.
            if (user == null)
                // GetAll() yerine GetUserByEmail yoksa: _userDal.GetAll().FirstOrDefault(...)
                user = _userDal.GetAll().FirstOrDefault(u => u.Email == userEmail);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (!VerifyPassword(password, user.PasswordHash))
                throw new InvalidOperationException("Incorrect password.");

            user.LastLoginUtc = DateTime.UtcNow;
            _userDal.Update(user);

            return user;
        }

        public void Follow(int followerId, int targetUserId)
        {
            if (followerId == targetUserId)
                throw new InvalidOperationException("You cannot follow yourself.");

            var already = _userDal.IsFollowing(followerId, targetUserId);
            if (already)
                throw new InvalidOperationException("This user is already being followed.");

            _userDal.Follow(followerId, targetUserId);
        }

        // --- Kişiselleştirme Metotları ---

        public void SetTheme(int userId, string theme)
        {
            if (theme != "dark" && theme != "light")
                throw new InvalidOperationException("Theme must be either 'dark' or 'light'.");

            var user = _userDal.GetById(userId);
            if (user == null) throw new InvalidOperationException("User not found.");

            user.ThemePreference = theme; // Theme değil ThemePreference (Entity'de öyle tanımlı)
            _userDal.Update(user);
        }

        public void SetPreferredCategories(int userId, List<string> categories)
        {
            if (categories == null || categories.Count == 0)
                throw new InvalidOperationException("You must select at least one category.");

            var user = _userDal.GetById(userId);
            if (user == null) throw new InvalidOperationException("User not found.");

            // List<string> -> string çevrimi (Virgülle ayırma)
            user.FavoriteGenres = string.Join(",", categories.Distinct());
            _userDal.Update(user);
        }

        public List<User> GetFollowedUsers(int userId) => _userDal.GetFollowedUsers(userId);
        public List<Game> GetUserGames(int userId) => _userDal.GetUserGames(userId);


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
    }
}