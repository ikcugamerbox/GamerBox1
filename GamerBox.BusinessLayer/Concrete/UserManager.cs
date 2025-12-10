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
        private readonly IUserDal _userDal; // user verilerini yönetmek için DAL katmanına erişir.

        public UserManager(IUserDal userDal) // Dependency Injection ile dışarıdan IUserDal nesnesi alınır.
        {
            _userDal = userDal;
        }

        // ---------------- PASSWORD HASHER ----------------
        private string HashPassword(string password)
        {
            using var rng = RandomNumberGenerator.Create(); // güvenli rastgele sayı üretici
            var salt = new byte[16]; // rastgele 16 byte 
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256); // PBKDF2 ile şifreleme. 100000 kez işlem yapılır. 
            var key = pbkdf2.GetBytes(32); //key=şifrenin hashlenmiş hali.

            return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";//salt ve key i base64 stringe çevirip salt.hash formatında döndürür.
        }

        private bool VerifyPassword(string password, string hash)
        {
            var parts = hash.Split('.', 2); // hash i salt ve key olarak ayırır.
            if (parts.Length != 2) return false;

            var salt = Convert.FromBase64String(parts[0]);
            var key = Convert.FromBase64String(parts[1]);
            // salt ve hash i byte array e çevirir.

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
            var keyToCheck = pbkdf2.GetBytes(32); // kullanıcının girdiği şifre tekrar hashlenir.

            return CryptographicOperations.FixedTimeEquals(keyToCheck, key); // iki hash aynıysa şifre doğrudur.
        }

        // ---------------- VALIDATION HELPERS ----------------
        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);// regex ile email formatı kontrol edilir.
        }

        private bool IsStrongPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password)) return false;
            var hasMinLen = password.Length >= 8;// en az 8 karakter
            var hasUpper = Regex.IsMatch(password, "[A-Z]");
            var hasLower = Regex.IsMatch(password, "[a-z]");
            var hasDigit = Regex.IsMatch(password, "[0-9]");
            var hasSpecial = Regex.IsMatch(password, @"[\W_]");// özel karakter içerip içermediğini kontrol eder.
            return hasMinLen && hasUpper && hasLower && hasDigit && hasSpecial;// hepsi sağlanıyorsa güçlü şifre.
        }

        // ---------------- REGISTER ---------------
        public void Register(User user, string plainPassword)
        {
            if (!IsValidEmail(user.Email))
                throw new InvalidOperationException("Invalid email format.");

            if (!IsStrongPassword(plainPassword))
                throw new InvalidOperationException("Password must be at least 8 characters long and contain uppercase, lowercase, digit, and special character.");

            var exists = _userDal.GetAll().Any(u => u.Email == user.Email);
            if (exists)
                throw new InvalidOperationException("A user with this email already exists.");

            user.PasswordHash = HashPassword(plainPassword); // şifre hashlenir.
            user.CreatedAtUtc = DateTime.UtcNow;//oluşturulma zamanı kaydedilir.

            _userDal.Add(user);// kullanıcı database e eklenir.
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

            user.LastLoginUtc = DateTime.UtcNow; // son giriş zamanı güncellenir.
            _userDal.Update(user);

            return user;// giriş başarılıysa user döner .
        }

        // ---------------- FOLLOW ----------------
        public void Follow(int followerId, int targetUserId)
        {
            if (followerId == targetUserId)
                throw new InvalidOperationException("You cannot follow yourself.");

            var already = _userDal.IsFollowing(followerId, targetUserId);
            if (already)
                throw new InvalidOperationException("This user is already being followed.");

            _userDal.Follow(followerId, targetUserId); //takip işlemi database e kaydedilir.
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
            _userDal.Update(user);// tema kaydedilir.
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
        public List<User> GetFollowedUsers(int userId) => _userDal.GetFollowedUsers(userId);// DAL da ne varsa onu döndürür.
        public List<Game> GetUserGames(int userId) => _userDal.GetUserGames(userId);// DAL da ne varsa onu döndürür.

        // ---------------- IGenericService<User> ----------------
        public void Add(User entity) => _userDal.Add(entity);
        public void Update(User entity) => _userDal.Update(entity);
        public void Delete(User entity) => _userDal.Delete(entity);
        public User GetById(int id) => _userDal.GetById(id);
        public List<User> GetAll() => _userDal.GetAll();
        // Tüm CRUD methodlarını UserDal a yönlendirir.
    }
}