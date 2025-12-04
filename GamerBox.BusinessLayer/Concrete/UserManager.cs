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

        // ---------------- PASSWORD HASHER ----------------
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

        // ---------------- VALIDATION HELPERS ----------------
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
        }

        private bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            var hasMinLen = password.Length >= 8;
            var hasUpper = Regex.IsMatch(password, "[A-Z]");
            var hasLower = Regex.IsMatch(password, "[a-z]");
            var hasDigit = Regex.IsMatch(password, "[0-9]");
            var hasSpecial = Regex.IsMatch(password, @"[\W_]");
            return hasMinLen && hasUpper && hasLower && hasDigit && hasSpecial;
        }

        // ---------------- REGISTER ----------------
        public void Register(User user, string plainPassword)
        {
            if (!IsValidEmail(user.Email))
                throw new InvalidOperationException("Invalid email format.");

            if (!IsStrongPassword(plainPassword))
                throw new InvalidOperationException("Password must be at least 8 characters long and contain uppercase, lowercase, digit, and special character.");

            var exists = _userDal.GetAll().Any(u => u.Email == user.Email);
            if (exists)
                throw new InvalidOperationException("A user with this email already exists.");

            user.PasswordHash = HashPassword(plainPassword);
            user.CreatedAtUtc = DateTime.UtcNow;

            _userDal.Add(user);
        }

        // ---------------- LOGIN ----------------
        public User Login(string userEmail, string password)
        {
            if (!IsValidEmail(userEmail))
                throw new InvalidOperationException("Invalid email format.");

            var user = _userDal.GetAll().FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            if (!VerifyPassword(password, user.PasswordHash))
                throw new InvalidOperationException("Incorrect password.");

            user.LastLoginUtc = DateTime.UtcNow;
            _userDal.Update(user);

            return user;
        }

        // ---------------- FOLLOW ----------------
        public void Follow(int followerId, int targetUserId)
        {
            if (followerId == targetUserId)
                throw new InvalidOperationException("You cannot follow yourself.");

            var already = _userDal.IsFollowing(followerId, targetUserId);
            if (already)
                throw new InvalidOperationException("This user is already being followed.");

            _userDal.Follow(followerId, targetUserId);
        }

        // ---------------- PERSONALIZATION ----------------
        public void SetTheme(int userId, string theme)
        {
            if (theme != "dark" && theme != "light")
                throw new InvalidOperationException("Theme must be either 'dark' or 'light'.");

            var user = _userDal.GetById(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.Theme = theme;
            _userDal.Update(user);
        }

        public void SetPreferredCategories(int userId, List<string> categories)
        {
            if (categories == null || categories.Count == 0)
                throw new InvalidOperationException("You must select at least one category.");

            var user = _userDal.GetById(userId);
            if (user == null)
                throw new InvalidOperationException("User not found.");

            user.PreferredCategories = categories.Distinct().ToList();
            _userDal.Update(user);
        }

        // ---------------- IUserService ----------------
        public List<User> GetFollowedUsers(int userId) => _userDal.GetFollowedUsers(userId);
        public List<Game> GetUserGames(int userId) => _userDal.GetUserGames(userId);

        // ---------------- IGenericService<User> ----------------
        public void Add(User entity) => _userDal.Add(entity);
        public void Update(User entity) => _userDal.Update(entity);
        public void Delete(User entity) => _userDal.Delete(entity);
        public User GetById(int id) => _userDal.GetById(id);
        public List<User> GetAll() => _userDal.GetAll();
    }
}